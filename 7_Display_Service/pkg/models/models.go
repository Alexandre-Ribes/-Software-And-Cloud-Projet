package models

type Point = struct {
	X int `json:"x"`
	Y int `json:"y"`
}

type GameState struct {
	ID        string
	Position  []Point
	Food      Point
	Score     uint
	IsRunning bool
}

func (state *GameState) GetLength() int {
	return len(state.Position)
}

func (state *GameState) GetHead() Point {
	return state.Position[0]
}
