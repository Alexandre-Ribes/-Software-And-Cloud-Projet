package main

import (
	"displayer/pkg/configs"
	"displayer/pkg/handlers"
	"displayer/pkg/initializer"
	"displayer/pkg/keys"
	"displayer/pkg/renderer"
	"displayer/pkg/state"
	"fmt"
	"log"
	"net/http"
	"time"
)

var stateManager state.IStateManager = state.Instance()
var config = configs.Instance()

func main() {
	// Routes proposées par l'API REST
	http.HandleFunc("/display/{id}/on-move", handlers.HandleOnMove)
	http.HandleFunc("/display/{id}/on-eat", handlers.HandleOnEat)
	http.HandleFunc("/display/{id}/on-score-change", handlers.HandleOnScoreChange)
	http.HandleFunc("/display/{id}/on-dead", handlers.HandleOnDead)

	http.HandleFunc("/display/{id}/subscribe", handlers.HandleSubscribe)
	http.HandleFunc("/display/{id}/unsubscribe", handlers.HandleUnsubscribe)

	// Goroutine de rendu du jeu
	gameId, err := initializer.InitGame(initializer.DefaultConfig())
	if err != nil {
		log.Printf("The following error occurred while initializing the game: %v", err)
	}
	stateManager.Init(gameId)

	go GameLoop(gameId)

	// Lancement du serveur HTTP
	log.Println("Serveur sur http://localhost:" + fmt.Sprint(config.Port))
	log.Fatal(http.ListenAndServe(":"+fmt.Sprint(config.Port), nil))
}

// Boucle principale de gestion du jeu
func GameLoop(gameId string) {
	render := renderer.TerminalRenderer{}
	render.Init()
	defer render.Close()

	keyListener := keys.NewKeyListener()
	defer keyListener.Close()

	// Boucle de jeu
	for {
		// Pause pour faire tourner le jeu à 1 FPS
		time.Sleep(1 * time.Second)

		// Emission de l'évènement du clavier
		keyListener.EmitKeys(gameId)

		stateManager.Lock(gameId)

		// Mise à jour du rendu du jeu selon l'état actuel
		state := stateManager.Get(gameId)
		render.Render(state)

		isRunning := state.IsRunning

		stateManager.Unlock(gameId)

		// Cas de fin de partie
		if !isRunning {
			break
		}
	}

	// Signalement au serveur distant que la partie est terminée
	err := initializer.EndGame(initializer.DefaultConfig())
	if err != nil {
		log.Printf("The following error occurred while ending the game: %v", err)
	}
}
