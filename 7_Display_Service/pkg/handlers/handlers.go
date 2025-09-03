package handlers

import (
	"displayer/pkg/events"
	"displayer/pkg/models"
	"displayer/pkg/state"
	"encoding/json"
	"fmt"
	"net/http"
)

var stateManager state.IStateManager = state.Instance()

func HandleSubscribe(w http.ResponseWriter, r *http.Request) {
	id := r.PathValue("id")

	var bodyData map[string]string
	if err := json.NewDecoder(r.Body).Decode(&bodyData); err != nil {
		fmt.Fprint(w, "Bad body")
		return
	}

	route := bodyData["route"]

	events.Instance().Subscribe(id, route)

	fmt.Fprint(w, "OK")
}

func HandleUnsubscribe(w http.ResponseWriter, r *http.Request) {
	id := r.PathValue("id")

	var bodyData map[string]string
	if err := json.NewDecoder(r.Body).Decode(&bodyData); err != nil {
		fmt.Fprint(w, "Bad body")
		return
	}

	route := bodyData["route"]

	events.Instance().Unsubscribe(id, route)

	fmt.Fprint(w, "OK")
}

func HandleOnMove(w http.ResponseWriter, r *http.Request) {
	id := r.PathValue("id")

	var bodyData []models.Point
	if err := json.NewDecoder(r.Body).Decode(&bodyData); err != nil {
		fmt.Fprint(w, "Bad body")
		return
	}

	stateManager.Lock(id)
	stateManager.Get(id).Position = bodyData
	stateManager.Unlock(id)

	fmt.Fprint(w, "OK")
}

func HandleOnEat(w http.ResponseWriter, r *http.Request) {
	id := r.PathValue("id")

	var newFoodPosition models.Point
	if err := json.NewDecoder(r.Body).Decode(&newFoodPosition); err != nil {
		fmt.Fprint(w, "Bad body")
		return
	}

	stateManager.Lock(id)
	stateManager.Get(id).Food = newFoodPosition
	stateManager.Unlock(id)

	fmt.Fprint(w, "OK")
}

func HandleOnScoreChange(w http.ResponseWriter, r *http.Request) {
	id := r.PathValue("id")

	var scoreData map[string]int
	if err := json.NewDecoder(r.Body).Decode(&scoreData); err != nil {
		fmt.Fprint(w, "Bad body")
		return
	}

	if scoreData["score"] < 0 {
		fmt.Fprint(w, "Bad score (< 0)")
		return
	}

	stateManager.Lock(id)
	stateManager.Get(id).Score = uint(scoreData["score"])
	stateManager.Unlock(id)

	fmt.Fprint(w, "OK")
}

func HandleOnDead(w http.ResponseWriter, r *http.Request) {
	id := r.PathValue("id")

	stateManager.Lock(id)
	stateManager.Get(id).IsRunning = false
	stateManager.Unlock(id)

	fmt.Fprint(w, "OK")
}
