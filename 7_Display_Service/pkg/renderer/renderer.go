package renderer

import (
	"displayer/pkg/configs"
	"displayer/pkg/models"
)

// Interface de rendu afin d'être substituable pour un rendu différent
type IRenderer interface {
	Init()
	Close()
	Render(*models.GameState)
}

// Instance de la configuration actuelle
var conf = configs.Instance()
