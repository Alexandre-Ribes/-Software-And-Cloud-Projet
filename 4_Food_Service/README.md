# Food Service – AlexandreRibes

## Contexte

Ce microservice fait partie du projet **Snake en Microservices**.

Il est responsable de la **gestion de la nourriture** :

-   Générer un fruit aléatoirement sur la grille (sans chevaucher le serpent).
-   Supprimer la nourriture quand elle est mangée.
-   Publier un événement `food.eaten` aux autres services abonnés (Score, Display).
-   S’abonner au service **Snake** pour être notifié des déplacements.

## Installation

### 1\. Installer les dépendances

pip install fastapi uvicorn httpx

## Lancer le service

Démarrer le Food Service sur le port **8004** :

uvicorn food\_service:app --reload --host 0.0.0.0 --port 8004

Le service sera disponible sur :

[http://localhost:8004/](http://localhost:8004/)

## Routes disponibles

### 1\. Abonnement

POST /food/{gameId}/subscribe

Body: { "route": "<url\_du\_service\_abonné>" }

-   Ajoute un abonné (ex: Display, Score).
-   Génère automatiquement le premier fruit pour la partie.
-   Abonne également le Food Service au Snake.

### 2\. Désabonnement

POST /food/{gameId}/unsubscribe

Body: { "route": "<url\_du\_service\_abonné>" }

-   Supprime l’abonné.

### 3\. Mouvement du serpent (reçu du Snake)

POST /food/{gameId}/on-move

Body: { "x": 5, "y": 10 }

-   Reçoit la position du serpent.
-   Génère un fruit s’il n’existe pas encore.

### 4\. Récupérer la nourriture

GET /food/{gameId}

-   Retourne la position actuelle de la nourriture.

Réponse :

{ "food": { "x": 7, "y": 3 } }

### 5\. Manger la nourriture

DELETE /food/{gameId}

-   Supprime la nourriture existante.
-   Notifie les abonnés avec un événement `food.eaten`.

Réponse :

{ "eventName": "food.eaten", "gameId": "partie1", "food": { "x": 7, "y": 3 } }

## Tests

Un script de test est fourni : `test_food_service.py`.

### Lancer les tests :

python test\_food\_service.py

Ce script teste :

1.  subscribe → abonnement + création du fruit
2.  on-move → réception d’un déplacement du serpent
3.  get food → récupération de la nourriture
4.  eat food → suppression + notification
5.  unsubscribe → désabonnement