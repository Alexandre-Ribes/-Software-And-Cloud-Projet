import asyncio
import httpx

BASE_URL = "http://localhost:8004/food"
GAME_ID = "testGame1"
SUBSCRIBER_URL = "http://localhost:8004/mockCallback"  # simule un subscriber

async def test_food_service():
    async with httpx.AsyncClient() as client:

        # Subscribe
        print("==> Test: Subscribe")
        response = await client.post(f"{BASE_URL}/{GAME_ID}/subscribe", json={"route": SUBSCRIBER_URL})
        print(response.status_code, response.json())

        # on-move (simuler un déplacement du serpent)
        print("\n==> Test: On-Move")
        move = {"x": 0, "y": 0}
        response = await client.post(f"{BASE_URL}/{GAME_ID}/on-move", json=move)
        print(response.status_code, response.json())

        # Get food
        print("\n==> Test: Get Food")
        response = await client.get(f"{BASE_URL}/{GAME_ID}")
        print(response.status_code, response.json())

        # Eat food
        print("\n==> Test: Eat Food")
        response = await client.delete(f"{BASE_URL}/{GAME_ID}")
        print(response.status_code, response.json())

        # Unsubscribe
        print("\n==> Test: Unsubscribe")
        response = await client.post(f"{BASE_URL}/{GAME_ID}/unsubscribe", json={"route": SUBSCRIBER_URL})
        print(response.status_code, response.json())

if __name__ == "__main__":
    asyncio.run(test_food_service())
