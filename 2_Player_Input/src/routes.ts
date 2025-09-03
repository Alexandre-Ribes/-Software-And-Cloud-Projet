import { Router } from 'express';
import { Input } from './input.js';
import { sendDirectionToSnakeState } from './snakeClient.js';

const router = Router();
const inputMap = new Map<string, Input>();

router.post('/:id/subscribe', (req, res) => {
  const { id } = req.params;
  if (!inputMap.has(id)) inputMap.set(id, new Input());
  inputMap.get(id)!.subscribe(id, sendDirectionToSnakeState);
  res.json({ status: 'subscribed' });
});

router.post('/:id/unsubscribe', (req, res) => {
  const { id } = req.params;
  inputMap.get(id)?.unsubscribe(id);
  res.json({ status: 'unsubscribed' });
});

router.post('/:id/on-update', (req, res) => {
  const { id } = req.params;
  const keys: string[] = req.body; 

  let input = inputMap.get(id);
  if (!input) {
    input = new Input();
    inputMap.set(id, input);
    input.subscribe(id, sendDirectionToSnakeState);
  }

  keys.forEach(key => input.addInput(key));
  const directions = input.getDirections(); 
  input.onUpdate(id); 

  res.json({ status: 'updated', sent: directions }); 
});

export default router;
