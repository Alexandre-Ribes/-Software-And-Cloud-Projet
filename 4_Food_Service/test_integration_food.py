import asyncio
import httpx

FOOD_SERVICE_URL = "http://localhost:8004"
GAME_ID = "partie_test"

# --- Simule un Display Service ---
from fastapi import FastAPI
from pydantic import BaseModel
import uvicorn

display_app = FastAPI(title="Fake Display Service")

class FoodEvent(BaseModel):
    eventName: str
    gameId: str
    food: dict

received_events = []

@display_app.post("/display/on-food-eaten")
async def on_food_eaten(event: FoodEvent):
    print(f"[Display] Événement reçu: {event}")
    received_events.append(event)
    return {"status": "ok"}

async def run_display_service():
    config = uvicorn.Config(display_app, host="0.0.0.0", port=8010, log_level="info")
    server = uvicorn.Server(config)
    await server.serve()

# --- Tests d'intégration ---
async def test_food_service():
    async with httpx.AsyncClient() as client:
        display_url = "http://localhost:8010/display/on-food-eaten"
        
        # Subscribe le Display Service au Food Service
        r = await client.post(f"{FOOD_SERVICE_URL}/food/{GAME_ID}/subscribe", json={"route": display_url})
        print("[Test] Subscribe:", r.json())
        
        # Simuler un déplacement du serpent
        r = await client.post(f"{FOOD_SERVICE_URL}/food/{GAME_ID}/on-move", json={"x": 5, "y": 5})
        print("[Test] On-Move:", r.json())
        
        # Manger la nourriture
        r = await client.delete(f"{FOOD_SERVICE_URL}/food/{GAME_ID}")
        print("[Test] Eat Food:", r.json())

        # Vérifier que l'événement est bien reçu
        await asyncio.sleep(1)  # attendre la notification
        print(f"[Test] Événements reçus par Display: {received_events}")

# --- Main ---
async def main():
    # Lancer le fake Display Service en tâche séparée
    display_task = asyncio.create_task(run_display_service())
    
    # Attendre 1 seconde pour que le serveur démarre
    await asyncio.sleep(1)
    
    # Lancer le test Food Service
    await test_food_service()
    
    # Arrêter le serveur après le test
    display_task.cancel()
    try:
        await display_task
    except asyncio.CancelledError:
        print("[Test] Display Service arrêté.")

if __name__ == "__main__":
    asyncio.run(main())
