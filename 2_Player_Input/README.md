Alexis Louisfert 
service d'input

LLD

diagramme de classe avec une seule classe


|       Input                                       |
|---------------------------------------------------|
| - inputs: string[]                                |
| - observers: Map<string, Observer>                |
| - keyMap: Record<string, Direction>               |
|---------------------------------------------------|
| + subscribe(id: string, callback: Observer): void |
| + unsubscribe(id: string): void                   |
| + addInput(key: string): void                     |
| + getDirections(): Direction[]                    |
| + onUpdate(id: string): void                      |

diagramme de sequence

participant Client as Display Service
participant PlayerInput as Player Input Service
participant Input as Input
participant SnakeState as Snake State Service

Client->>PlayerInput: POST /:id/on-update avec touches
PlayerInput->>Input: Vérifie si Input existe pour l'ID
alt Input inexistant
    PlayerInput->>Input: Crée un nouvel Input
end
PlayerInput->>Input: Ajoute les touches reçues
PlayerInput->>Input: Transforme touches en directions valides
Input->>Input: getDirections() -> directions[]
Input->>PlayerInput: directions[]
PlayerInput->>Input: onUpdate(id) (déclenche Observer)
Input->>SnakeState: Observer envoie directions au Snake State Service
SnakeState-->>PlayerInput: 200 OK
PlayerInput-->>Client: 200 OK { status: 'updated', sent: directions }







Demarrer le service sous docker
npm install //si le projet n'as pas deja récupéré les dépendances
docker build -t 2_player_input .
docker run -p 8002:8002 --name player-input 2_player_input