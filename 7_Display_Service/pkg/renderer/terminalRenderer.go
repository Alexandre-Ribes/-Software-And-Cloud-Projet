package renderer

import (
	"displayer/pkg/models"
	"fmt"
	"encoding/json"
)

// Rendu du jeu via un Terminal
type TerminalRenderer struct {
}

func (TerminalRenderer) Init() {
	// Aucune initialisation n'est requise
}

func (TerminalRenderer) Close() {
	// Aucune fermeture n'est requise
}

func (TerminalRenderer) Render(state *models.GameState) {
	// Efface l'écran
	fmt.Print("\033[H\033[2J")

	// Affichage du score et du statut (en cours ou non)
	fmt.Printf("Score: %d %s\n", state.Score, map[bool]string{true: "(RUNNING)", false: ""}[state.IsRunning])

	// Création de la grille du jeu
	width, height := conf.ScreenSize, conf.ScreenSize
	grid := make([][]rune, height)

	for y := range height {
		grid[y] = make([]rune, width)
		for x := range width {
			grid[y][x] = '.' // Remplissage de la grille avec des points (case vide)
		}
	}

	// Ajout de la nourriture à la grille
	if state.Food.Y >= 0 && state.Food.Y < height && state.Food.X >= 0 && state.Food.X < width {
		grid[state.Food.Y][state.Food.X] = '*'
	}

	// Remplissage de la grille avec les positions du serpents
	for _, p := range state.Position {
		if p.Y >= 0 && p.Y < height && p.X >= 0 && p.X < width {
			grid[p.Y][p.X] = 'O'
		}
	}

	// Affichage de la grille dans le terminal
	for y := range height {
		for x := range width {
			fmt.Printf("%c", grid[y][x])
		}
		fmt.Println()
	}

	// Affichage de l'état du jeu pour le debug
	if conf.Debug {
		jsonState, err := json.Marshal(state)
		if err != nil {
			panic(err)
		}
		fmt.Println(string(jsonState))
	}
}