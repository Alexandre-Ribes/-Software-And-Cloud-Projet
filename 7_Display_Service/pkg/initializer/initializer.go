package initializer

import (
	"bytes"
	"displayer/pkg/configs"
	"encoding/json"
	"errors"
	"net/http"
)

// Configuration de l'initialisation de la partie
type InitializeConfig struct {
	DefaultId           string
	StartupServiceRoute string
	subscriptions       map[string]string // Dictionnaire entre la route d'abonnement et la route de réception
}

// Ajoute une nouvelle route d'abonnement
func (c *InitializeConfig) AddSubscription(route, receptionPath string) {
	c.subscriptions[route] = receptionPath
}

// Retourne une configuration par defaut pour faciliter le lancement dans le cas présent
func DefaultConfig() InitializeConfig {
	return InitializeConfig{
		DefaultId:           "default-id",
		StartupServiceRoute: "http://localhost:8001/game/",
		subscriptions: map[string]string{
			"http://localhost:8003/snake/":     "/display/{id}/on-move",
			"http://localhost:8004/food/":      "/display/{id}/on-eat",
			"http://localhost:8005/collision/": "/display/{id}/on-dead",
			"http://localhost:8006/score/":     "/display/{id}/on-score-change",
		},
	}
}

// Fonction d'initialisation de la partie en s'abonnant aux différents services
func InitGame(conf InitializeConfig) (string, error) {
	// Récupération de l'ID de la nouvelle partie
	resp, err := http.Post(conf.StartupServiceRoute+"start", "application/json", nil)
	if err != nil {
		return conf.DefaultId, err
	}
	defer resp.Body.Close()

	// Lecture du corps de la réponse
	var respData map[string]string
	if err := json.NewDecoder(resp.Body).Decode(&respData); err != nil {
		return conf.DefaultId, err
	}

	// Récupération de l'ID de la partie
	gameId := respData["id"]
	if gameId == "" {
		return conf.DefaultId, errors.New("failed to get game id")
	}

	// Abonnement aux services
	routePrefix := configs.Instance().GetHost() + "/display/" + gameId + "/"

	// Abonnement à tous les services en ignorant les erreurs
	for route, receptionPath := range conf.subscriptions {
		_ = subscribeTo(receptionPath, gameId, routePrefix+route)
	}

	return gameId, nil
}

// Termine la partie en contactant le service de startup
func EndGame(conf InitializeConfig, gameId string) error {
	resp, err := http.Post(conf.StartupServiceRoute+gameId+"/stop", "application/json", nil)
	if err != nil {
		return err
	}
	defer resp.Body.Close()

	return nil
}

// Abonnement à un service distant
func subscribeTo(serverUrl, gameId, route string) error {
	url := serverUrl + gameId + "/subscribe"

	jsonData, err := json.Marshal(map[string]string{"route": route})
	if err != nil {
		return err
	}

	resp, err := http.Post(url, "application/json", bytes.NewBuffer(jsonData))
	if err != nil {
		return err
	}
	defer resp.Body.Close()

	return nil
}
