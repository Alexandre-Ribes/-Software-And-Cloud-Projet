# Collision Service (Snake) — README

## Service de Nicolas Habares

Microservice **minimaliste** en TypeScript/Express qui détermine si le serpent est **mort** après un déplacement, et notifie un service **Display** lorsqu’il meurt.

## ▶️ Fonction
- **Entrée** : un **tableau** de positions `{x,y}` où **le 1er élément est la tête**, les suivants le corps.
- **Règles** :
  - Grille **16×16** (indices `0..15`).
  - Mort si la tête sort des bornes, ou si elle chevauche le corps.
- **Sortie** : `{ "dead": boolean }`
- **Notification** (si `dead=true`) : `POST http://localhost:8007/display/:id/on-dead` avec `{ "dead": true }`.

---

## 🐳 Docker

### Dockerfile (déjà présent)
Build + run :
```bash
docker build -t collision-service .
docker run --name collision --network app-net -p 8005:8005 \
  -e DISPLAY_BASE_URL=http://display:8007 \
  collision-service

OU (si on reste sur localhost, on a pas de docker compose)
docker run --name collision -p 8005:8005 collision-service
```


---

## Tests éventuels sans docker

## 🧰 Prérequis
- **Node.js ≥ 18** (pour `fetch` natif)
- **npm**  
- (TS) : `@types/express`, `@types/node`

---

## 📦 Installation & exécution (local)

```bash
# Installer les dépendances
npm install

# Dev (exécute src/index.ts via tsx)
npm run dev

# Build TypeScript -> dist/
npm run build

# Start (production, exécute dist/index.js)
npm start
```

Logs attendus : `Collision service running on :8005`

---

## 🚦 API

### POST `/collision/:id/on-move`
**Body (JSON)** : tableau de points, **tête en premier**.
```json
[
  { "x": 7, "y": 9 },
  { "x": 7, "y": 9 },
  { "x": 7, "y": 8 }
]
```

**Réponse**
```json
{ "dead": true }
```
ou
```rien
```

### Exemples (curl)

Vivant :
```bash
curl -X POST http://localhost:8005/collision/game123/on-move   -H "Content-Type: application/json"   -d '[{"x":7,"y":6},{"x":7,"y":9},{"x":7,"y":8}]'
```

Mort (mur) :
```bash
curl -X POST http://localhost:8005/collision/game123/on-move   -H "Content-Type: application/json"   -d '[{"x":16,"y":5},{"x":7,"y":5},{"x":6,"y":5}]'
```

Mort (auto-collision) :
```bash
curl -X POST http://localhost:8005/collision/game123/on-move   -H "Content-Type: application/json"   -d '[{"x":7,"y":8},{"x":7,"y":9},{"x":7,"y":8}]'
```

> 💡 Si `dead=true`, le service tente aussi un POST vers `http://localhost:8007/display/:id/on-dead`.


## 📂 Structure (simple)
```
src/
  index.ts            # serveur Express (+ route /collision/:id/on-move)
  CollisionService.ts # logique collision + notifyDead()
package.json
tsconfig.json
```

