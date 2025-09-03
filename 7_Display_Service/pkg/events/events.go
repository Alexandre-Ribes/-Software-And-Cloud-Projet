package events

import (
	"bytes"
	"encoding/json"
	"log"
	"net/http"
)

// Interface de manageur d'evenements
type IEventManager interface {
	Subscribe(id string, path string)
	Unsubscribe(id string, path string)
	Emit(id string, data any) error
}

var instance *eventManager

// Récupère l'instance du gestionnaire d'evenements utilisé
func Instance() IEventManager {
	if instance == nil {
		instance = &eventManager{
			Subscribers: map[string][]string{},
		}
	}
	return instance
}

// Implémentation du gestionnaire d'events
type eventManager struct {
	Subscribers map[string][]string // Map d'identifiants et de chemins
}

// Abonnement d'un chemin à un Identifiant de partie
func (e *eventManager) Subscribe(id string, path string) {
	e.Subscribers[id] = append(e.Subscribers[id], path)
}

// Déabonnement d'un chemin à un Identifiant de partie
func (e *eventManager) Unsubscribe(id string, path string) {
	e.Subscribers[id] = append(e.Subscribers[id], path)
}

// Envoi d'un évènement vers tous les clients abonnées
func (e *eventManager) Emit(id string, data any) error {
	jsonData, err := json.Marshal(data)
	if err != nil {
		return err
	}

	urls := e.Subscribers[id]
	for _, url := range urls {
		resp, err := http.Post(url, "application/json", bytes.NewBuffer(jsonData))
		if err != nil {
			return err
		}
		defer resp.Body.Close()

		if resp.StatusCode != 200 {
			log.Printf("Failed to emit event to %s with status %d", url, resp.StatusCode)
		}
	}

	return nil
}
