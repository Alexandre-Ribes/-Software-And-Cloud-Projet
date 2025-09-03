package state

import (
	"displayer/pkg/models"
	"sync"
)

// Interface du gestionnaire des états
type IStateManager interface {
	Lock(string)
	Unlock(string)
	Get(string) *models.GameState
	Set(string, models.GameState)
	Init(string)
}

var instance IStateManager

// Récupère l'instance du gestionnaire des états utilisé
func Instance() IStateManager {
	if instance == nil {
		instance = &stateManager{
			States: map[string]state{},
		}
	}
	return instance
}

// Gestionnaire des états de partie
type stateManager struct {
	States map[string]state
}

// Etat de la partie accompagné d'un lock pour sécuriser la concurrence
type state struct {
	GameState *models.GameState
	Lock      *sync.Mutex
}

// Verrouille l'accès à l'état de la partie
func (m *stateManager) Lock(id string) {
	m.States[id].Lock.Lock()
}

// Déverrouille l'accès à l'état de la partie
func (m *stateManager) Unlock(id string) {
	m.States[id].Lock.Unlock()
}

// Récupération de l'état de la partie
func (m *stateManager) Get(id string) *models.GameState {
	return m.States[id].GameState
}

// Met à jour l'état de la partie
func (m *stateManager) Set(id string, gameState models.GameState) {
	state := m.States[id]

	state.GameState = &gameState

	m.States[id] = state
}

// Initialise l'état de départ pour une nouvelle partie
func (m *stateManager) Init(id string) {
	m.States[id] = state{
		GameState: &models.GameState{
			ID: id,
			Position: []models.Point{
				{X: 7, Y: 7},
			},
			Food:      models.Point{X: -1, Y: -1},
			Score:     0,
			IsRunning: true,
		},
		Lock: &sync.Mutex{},
	}
}
