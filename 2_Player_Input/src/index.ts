import express, { Request, Response, NextFunction } from 'express';
import router from './routes.js';

const app = express();

// Middleware pour parser le JSON
app.use(express.json());

// Middleware de log simple
app.use((req: Request, res: Response, next: NextFunction) => {
  console.log(`[${req.method}] ${req.url}`);
  next();
});

// Routes principales
app.use('/input', router);

// Route de test
app.get('/', (req: Request, res: Response) => {
  res.send('🎮 Player Input Service is running!');
});

// Gestion des erreurs
app.use((err: Error, req: Request, res: Response, next: NextFunction) => {
  console.error('❌ Internal error:', err.message);
  res.status(500).json({ error: 'Internal Server Error' });
});

// Démarrage du serveur
const PORT = process.env.PORT || 8002;
app.listen(PORT, () => {
  console.log(`🚀 Player Input service listening on port ${PORT}`);
});
