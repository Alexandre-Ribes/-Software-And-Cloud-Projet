LLD

diagramme de classe avec une seule classe

+---------------------------------------------------+
|       Input                                       |
+---------------------------------------------------+
| - inputs: string[]                                |
| - observers: Map<string, Observer>                |
| - keyMap: Record<string, Direction>               |
+---------------------------------------------------+
| + subscribe(id: string, callback: Observer): void |
| + unsubscribe(id: string): void                   |
| + addInput(key: string): void                     |
| + getDirections(): Direction[]                    |
| + onUpdate(id: string): void                      |
+---------------------------------------------------+

Client (Display Service)     Player Input Service       Input        Snake State Service
        |                          |                     |                    |
        | POST /:id/on-update      |                     |                    |
        |------------------------->|                     |                    |
        |                          | inputMap[id]?       |                    |
        |                          |-------------------->|                    |
        |                          |  new Input()        |                    |
        |                          |<--------------------|                    |
        |                          | addInput(key)       |                    |
        |                          |-------------------->|                    |
        |                          | getDirections()     |                    |
        |                          |-------------------->|                    |
        |                          | directions[]        |                    |
        |                          |<--------------------|                    |
        |                          | onUpdate(id)        |                    |
        |                          |-------------------->| observer(id, directions)|
        |                          |                     |------------------->|
        |                          |                     | sendDirection(direction[])|
        |                          |                     |<-------------------|
        | 200 OK { status, sent }  |                     |                    |
        |<-------------------------|                     |                    |










Demarrer le service sous docker
npm install //si le projet n'as pas deja récupéré les dépendances
docker build -t 2_player_input .
docker run -p 8002:8002 --name player-input 2_player_input