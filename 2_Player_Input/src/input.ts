type Direction = "up" | "down" | "left" | "right" | "none";
type Observer = (id: string, directions: Direction[]) => void; // on renvoie un tableau maintenant

export class Input {
  private inputs: string[] = [];
  private observers: Map<string, Observer> = new Map();

  private keyMap: Record<string, Direction> = {
    "ArrowUp": "up",
    "ArrowDown": "down",
    "ArrowLeft": "left",
    "ArrowRight": "right",
    "z": "up",
    "s": "down",
    "q": "left",
    "d": "right"
  };

  subscribe(id: string, callback: Observer) {
    this.observers.set(id, callback);
  }

  unsubscribe(id: string) {
    this.observers.delete(id);
  }

  addInput(key: string) {
    this.inputs.push(key);
  }

  getDirections(): Direction[] {
    const directions = this.inputs
      .map(key => this.keyMap[key])
      .filter((dir): dir is Direction => !!dir);

    this.inputs = []; 
    return directions.length === 1 ? directions : ["none"];
  }

  onUpdate(id: string) {
    const directions = this.getDirections();
    const observer = this.observers.get(id);
    if (observer) {
      observer(id, directions);
    }
  }
}
