package keys

import (
	"displayer/pkg/events"
	"log"
	"sync"

	"github.com/eiannone/keyboard"
)

// Interface de l'observeur des touches
type IKeyListener interface {
	GetKeys() []rune
	Close()
	EmitKeys(id string) error
}

// Structure de l'observeur des touches en asynchrone
type KeyListener struct {
	mu   sync.Mutex
	keys []rune
	done chan struct{}
}

// Initialise l'observeur des touches asynchrone
func NewKeyListener() IKeyListener {
	kl := &KeyListener{
		keys: make([]rune, 0),
		done: make(chan struct{}),
	}

	// Ouverture du clavier
	if err := keyboard.Open(); err != nil {
		// Une erreur aura toujours lieu sur Docker à cause de l'absence de clavier
		// En cette raison, log.Println est préféré à panic
		log.Println(err)
	}

	// Goroutine asynchrone pour lire les touches
	go func() {
		for {
			select {
			case <-kl.done: // Détection de la fermeture
				return
			default:
				char, _, err := keyboard.GetKey()
				if err != nil {
					continue
				}
				kl.mu.Lock()
				kl.keys = append(kl.keys, char)
				kl.mu.Unlock()
			}
		}
	}()

	return kl
}

// Retourne les touches appuyées depuis le dernier appel
func (kl *KeyListener) GetKeys() []rune {
	kl.mu.Lock()
	defer kl.mu.Unlock()

	copied := make([]rune, len(kl.keys))
	copy(copied, kl.keys)
	kl.keys = kl.keys[:0] // Vide le slice de touches

	return copied
}

// Ferme la surveillance des touches
func (kl *KeyListener) Close() {
	close(kl.done)
	keyboard.Close()
}

// Envoie les touches appuyées comme évènement via l'EventManager
func (kl *KeyListener) EmitKeys(id string) error {
	eventManager := events.Instance()

	pressedKey := kl.GetKeys()
	return eventManager.Emit(id, pressedKey)
}
