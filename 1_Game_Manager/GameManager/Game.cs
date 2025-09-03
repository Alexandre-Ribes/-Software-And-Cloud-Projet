using System.Collections.Concurrent;

namespace GameManager
{
	public class Game
	{
		private static readonly ConcurrentDictionary<string, Game> instances = new();
		private string sessionId;
		private bool isRunning;

		public Game(string? sessionId = null)
		{
			if (sessionId != null)
				this.sessionId = sessionId;
		}

		public Game GetInstance(string? sessionId = null)
		{
			// Vérifie si l'instance existe
			if (sessionId != null)
			{
				if (instances.TryGetValue(sessionId, out var existInstance))
				{
					return existInstance;
				}
			}

			// Si l'instance n'existe pas et que sessionId n'est pas vide on retourne une erreur
			if (sessionId != null)
				return null;

			// Sinon démarre une nouvelle session de jeu
			string? id = null;
			while (id == null || instances.TryGetValue(id, out var test) == true)
			{
				id = new Random().Next(10000, 90000).ToString();
			}

			Game instance = new Game(id);
			instance.isRunning = true;
			instances[id] = instance;
			return instance;
		}


		public void Stop()
		{
			this.isRunning = false;
		}

		public string GetSessionId()
		{
			return sessionId;
		}

		public bool GetIsRunning()
		{
			return isRunning;
		}

	}
}
