# Utilise l'image officielle Python légère
FROM python:3.10.11

# Crée un répertoire de travail dans le conteneur
WORKDIR /app

# Copie le fichier des dépendances
COPY requirements.txt .

# Installe les dépendances
RUN pip install --no-cache-dir -r requirements.txt

# Copie le code source du service dans le conteneur
COPY . .

# Expose le port utilisé par le Food Service
EXPOSE 8004

# Commande de démarrage du service
CMD ["uvicorn", "FoodService:app", "--host", "0.0.0.0", "--port", "8004"]
