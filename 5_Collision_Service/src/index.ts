import express from "express";
import { CollisionService, Pt } from "./CollisionService.js";

const app = express();
app.use(express.json());

const DISPLAY_BASE_URL = process.env.DISPLAY_BASE_URL || "http://localhost:8007"; //localhost ou variable d'environnement

const collision = new CollisionService(DISPLAY_BASE_URL) as any;; //instance de la classe

app.post("/collision/:id/on-move", async (req, res) => { //post pour notifier la mort ou non du snake
  const { id } = req.params; //id de la partie
  const body = req.body as Pt[];
  const dead = collision.checkCollision(body);
  if (dead) {
    await collision.notifyDead(id);
    //res.json( "dead" );
    console.log("dead");
  } 
  else{
    //res.json( "not dead" );
    console.log("not dead");
  }
  res.end();
});

app.listen(8005, () => {
  console.log("Collision service running on :8005");
});