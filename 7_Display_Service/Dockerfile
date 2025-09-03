# Image de build
FROM golang:1.24.2-alpine AS builder

WORKDIR /app

# Copie et télécharge les fichiers de dépendances
COPY go.mod go.sum ./
RUN go mod download

# Copie et compile le code source
COPY main.go ./
COPY pkg ./pkg

RUN go build -o snake-app main.go


# Image de production
FROM alpine:latest

# Installer ca-certificates pour HTTP si besoin
RUN apk add --no-cache ca-certificates

WORKDIR /app

# Copier du fichier compilé de l'image de build
COPY --from=builder /app/snake-app .

# Ouverture du port 8007 utilisé
EXPOSE 8007

# Lancement de l'exécution
CMD ["./snake-app"]
