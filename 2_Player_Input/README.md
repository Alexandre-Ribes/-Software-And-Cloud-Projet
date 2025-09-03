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

|Client (Display Service)               Player Input Service                            Input                             Snake State Service
|        |                                       |                                        |                                       |
|        |Envoie les touches du joueur           |                                        |                                       |
|        |-------------------------------------->|                                        |                                       |
|        |                                       | Vérifie si un Input existe pour cet ID |                                       |
|        |                                       |--------------------------------------->|                                       |
|        |                                       | Crée un nouvel Input si nécessaire     |                                       |
|        |                                       |<---------------------------------------|                                       |
|        |                                       | Ajoute les touches reçues dans Input   |                                       |
|        |                                       |--------------------------------------->|                                       |
|        |                                       | Transforme les touches en directions   |                                       |
|        |                                       |--------------------------------------->|                                       |
|        |                                       | Obtient la liste des directions valides|                                       |
|        |                                       |<---------------------------------------|                                       |
|        |                                       |                      Notifie le service Snake State                            |
|        |                                       |------------------------------------------------------------------------------->|
|        |                                       |                      Transmet les directions au Snake State                    |
|        |                                       |------------------------------------------------------------------------------->|
|        | Reçoit la confirmation de mise à jour |                                        |                                       |
|        |<--------------------------------------|                                        |                                       |







Demarrer le service sous docker
npm install //si le projet n'as pas deja récupéré les dépendances
docker build -t 2_player_input .
docker run -p 8002:8002 --name player-input 2_player_input