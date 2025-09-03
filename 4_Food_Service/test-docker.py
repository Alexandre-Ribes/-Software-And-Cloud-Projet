import time
import requests
import subprocess

# Config
IMAGE_NAME = "alexribes/food-service:latest"  # ton image Docker Hub
CONTAINER_NAME = "food-service-test"
PORT = "8004"
BASE_URL = f"http://localhost:{PORT}"

def run_docker():
    """Lance le container Food Service en arrière-plan"""
    subprocess.run([
        "docker", "run", "-d",
        "--name", CONTAINER_NAME,
        "-p", f"{PORT}:8004",  # map du port exposé dans le container
        IMAGE_NAME
    ], check=True)

def stop_docker():
    """Stoppe et supprime le container si existant"""
    subprocess.run(["docker", "stop", CONTAINER_NAME], check=False)
    subprocess.run(["docker", "rm", CONTAINER_NAME], check=False)

def wait_for_api(timeout=15):
    """Attend que l'API soit disponible"""
    for _ in range(timeout):
        try:
            res = requests.get(f"{BASE_URL}/food/test-game")
            if res.status_code in (200, 404):
                print("✅ API disponible")
                return True
        except requests.exceptions.ConnectionError:
            pass
        time.sleep(1)
    raise TimeoutError("⏳ L’API n’a pas démarré dans le temps imparti")

def test_food_service():
    game_id = "test-game"

    # 1️⃣ Subscribe (crée un fruit + abonne un service fictif)
    res = requests.post(f"{BASE_URL}/food/{game_id}/subscribe",
                        json={"route": "http://localhost:9999/fake"})
    print("Subscribe:", res.json())

    # 2️⃣ Simuler un mouvement du snake
    res = requests.post(f"{BASE_URL}/food/{game_id}/on-move",
                        json={"x": 5, "y": 5})
    print("On-move:", res.json())

    # 3️⃣ Récupérer la position actuelle de la nourriture
    res = requests.get(f"{BASE_URL}/food/{game_id}")
    print("Get-food:", res.json())

    # 4️⃣ Simuler que la nourriture est mangée
    res = requests.delete(f"{BASE_URL}/food/{game_id}")
    print("Eat-food:", res.json())

if __name__ == "__main__":
    # Nettoyer avant test
    stop_docker()
    
    # Lancer le container
    run_docker()
    try:
        wait_for_api()
        test_food_service()
    finally:
        stop_docker()
