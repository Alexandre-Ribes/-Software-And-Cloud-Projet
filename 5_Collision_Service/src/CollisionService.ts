export type Pt = { x: number; y: number };

export class CollisionService {
    constructor(private displayBaseUrl: string = "http://localhost:8007") {}
    checkCollision(body: Pt[]): boolean {

        // grille 16x16 (0..15)
        const head = body.shift();
        if (!head) return false;
        if (head.x < 0 || head.x > 15 || head.y < 0 || head.y > 15)
            return true; // touche un mur

        // touche son propre corps
        if (body.some(p => p.x === head.x && p.y === head.y)) 
            return true;
        return false;
    }

    async notifyDead(id: string) {
        console.log("Collision");
        // notifier le service Display
        try {
            await fetch(`${this.displayBaseUrl}/display/${id}/on-dead`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ dead: true })
            });
        } catch (error) {
            console.error(error);
        }
    }
}