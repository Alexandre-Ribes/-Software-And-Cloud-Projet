from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Dict, List
import random
import httpx

app = FastAPI(title="Food Service - AlexandreRibes")

# --- Modèles ---
class RouteModel(BaseModel):
    route: str

class MoveModel(BaseModel):
    x: int
    y: int

class Position(BaseModel):
    x: int
    y: int

# --- Manager du Food Service ---
class FoodManager:
    def __init__(self):
        self.foods: Dict[str, Position] = {}  # gameId -> position nourriture
        self.snakes: Dict[str, List[Position]] = {}  # gameId -> positions serpent
        self.subscribers: List[str] = []  # routes abonnés

    def subscribe(self, route: str):
        if route not in self.subscribers:
            self.subscribers.append(route)

    def unsubscribe(self, route: str):
        if route in self.subscribers:
            self.subscribers.remove(route)

    def update_snake_position(self, game_id: str, pos: Position):
        if game_id not in self.snakes:
            self.snakes[game_id] = []
        self.snakes[game_id].append(pos)

    def generate_food(self, game_id: str, grid_width=16, grid_height=16, avoid: List[Position]=[]) -> Position:
        snake_pos = self.snakes.get(game_id, [])
        avoid_positions = avoid + snake_pos
        while True:
            pos = Position(x=random.randint(0, grid_width-1), y=random.randint(0, grid_height-1))
            if pos not in avoid_positions:
                self.foods[game_id] = pos
                return pos

    def get_food(self, game_id: str):
        return self.foods.get(game_id)

    async def eat_food(self, game_id: str):
        if game_id not in self.foods:
            return None
        food = self.foods.pop(game_id)
        # Notifier les abonnés
        async with httpx.AsyncClient() as client:
            for subscriber in self.subscribers:
                try:
                    await client.post(subscriber, json={
                        "eventName": "food.eaten",
                        "gameId": game_id,
                        "food": {"x": food.x, "y": food.y}
                    })
                except Exception as e:
                    print(f"Impossible de notifier {subscriber}: {e}")
        return food

food_manager = FoodManager()

# --- URL du Snake Service ---
SNAKE_SERVICE_URL = "http://localhost:8003"  # à adapter selon ton Snake Service

# --- Routes ---
@app.post("/food/{game_id}/subscribe")
async def subscribe(game_id: str, route: RouteModel):
    food_manager.subscribe(route.route)
    
    # Créer le premier fruit si nécessaire
    food = food_manager.get_food(game_id) or food_manager.generate_food(game_id)
    
    # --- Abonnement automatique au Snake Service ---
    FOOD_ON_MOVE_URL = f"http://localhost:8004/food/{game_id}/on-move"
    async with httpx.AsyncClient() as client:
        try:
            await client.post(f"{SNAKE_SERVICE_URL}/snake/{game_id}/subscribe", json={"route": FOOD_ON_MOVE_URL})
            print(f"Food Service abonné au Snake pour la partie {game_id}")
        except Exception as e:
            print(f"Impossible de s'abonner au Snake: {e}")
    
    return {"gameId": game_id, "food": {"x": food.x, "y": food.y}}

@app.post("/food/{game_id}/unsubscribe")
async def unsubscribe(game_id: str, route: RouteModel):
    food_manager.unsubscribe(route.route)
    return {"route": route.route}

@app.post("/food/{game_id}/on-move")
async def on_move(game_id: str, move: MoveModel):
    pos = Position(x=move.x, y=move.y)
    food_manager.update_snake_position(game_id, pos)
    # Générer nourriture si elle n'existe pas encore et éviter la position du serpent
    food = food_manager.get_food(game_id) or food_manager.generate_food(game_id, avoid=[pos])
    return {"gameId": game_id, "food": {"x": food.x, "y": food.y}}

@app.delete("/food/{game_id}")
async def eat_food(game_id: str):
    food = await food_manager.eat_food(game_id)
    if not food:
        raise HTTPException(status_code=404, detail="not_found")
    return {"eventName": "food.eaten", "gameId": game_id, "food": {"x": food.x, "y": food.y}}

@app.get("/food/{game_id}")
def get_food(game_id: str):
    food = food_manager.get_food(game_id)
    if not food:
        raise HTTPException(status_code=404, detail="not_found")
    return {"food": {"x": food.x, "y": food.y}}
