// test_integration_food.js
const axios = require('axios');

const FOOD_SERVICE_URL = "http://localhost:8004";
const GAME_ID = "partie_test";
const DISPLAY_URL = "http://localhost:8010/display/on-food-eaten";

let receivedEvents = [];

// Simuler le Display Service en Express
const express = require('express');
const displayApp = express();
displayApp.use(express.json());

displayApp.post('/display/on-food-eaten', (req, res) => {
    console.log("[Display] Événement reçu:", req.body);
    receivedEvents.push(req.body);
    res.json({ status: "ok" });
});

const displayServer = displayApp.listen(8010, () => {
    console.log("Display Service en cours d'exécution sur le port 8010");
});

async function runTest() {
    try {
        // 1️⃣ Subscribe le Display Service au Food Service
        let r = await axios.post(`${FOOD_SERVICE_URL}/food/${GAME_ID}/subscribe`, { route: DISPLAY_URL });
        console.log("[Test] Subscribe:", r.data);

        // 2️⃣ Simuler un déplacement du serpent
        r = await axios.post(`${FOOD_SERVICE_URL}/food/${GAME_ID}/on-move`, { x: 5, y: 5 });
        console.log("[Test] On-Move:", r.data);

        // 3️⃣ Manger la nourriture
        r = await axios.delete(`${FOOD_SERVICE_URL}/food/${GAME_ID}`);
        console.log("[Test] Eat Food:", r.data);

        // 4️⃣ Attendre la notification
        await new Promise(resolve => setTimeout(resolve, 1000));
        console.log("[Test] Événements reçus par Display:", receivedEvents);
    } catch (err) {
        console.error(err);
    } finally {
        displayServer.close(() => console.log("[Test] Display Service arrêté."));
    }
}

runTest();
