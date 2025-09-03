using Microsoft.AspNetCore.Mvc;
using ScoreService.IServices;
using ScoreService.Models;

namespace ScoreService.Controllers
{
	[ApiController]
	//[Route("api/[controller]")]
	[Route("api/score")]
	public class ScoreController : Controller
	{
		//private static List<ScoreModel> _scores = [];
		private static List<ScoreModel> _scores = new List<ScoreModel>
		{
			new ScoreModel { GameId = "game123", Score = 42, LastUpdated = DateTime.UtcNow },
			new ScoreModel { GameId = "game456", Score = 100, LastUpdated = DateTime.UtcNow }
		};

		private readonly INotificationService _notificationService;

		public ScoreController(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		// GET: api/score/{id}
		[HttpGet("{id}")]
		public ActionResult<int> GetScoreByGame(string id)
		{
			var gameScore = _scores.Find(s => s.GameId == id);

			if (gameScore == null) return NotFound();

			return Ok(gameScore.Score);
		}

		// POST: api/score/{id}/increment
		[HttpPost("{id}/increment")]
		public async Task<ActionResult> IncrementScore(string id)
		{
			var score = _scores.FirstOrDefault(s => s.GameId == id);

			if (score == null) return NotFound(new { message = $"Aucun score trouvé pour le GameId {id}" });

			score.Increment();

			// Notifie les abonnés
			await _notificationService.NotifySubscribersAsync(id, score);

			return Ok(score);
		}

		// POST: api/score/{id}/subscribe
		[HttpPost("{id}/subscribe")]
		public ActionResult Subscribe(string id, [FromBody] SubscriptionRequestModel request)
		{
			if (string.IsNullOrWhiteSpace(request.Route))
			{
				return BadRequest(new { message = "La route de callback est obligatoire." });
			}

			if (!SubscriptionStore.Subscriptions.ContainsKey(id))
			{
				SubscriptionStore.Subscriptions[id] = new List<string>();
			}

			if (!SubscriptionStore.Subscriptions[id].Contains(request.Route))
			{
				SubscriptionStore.Subscriptions[id].Add(request.Route);
			}

			return Ok(new { message = $"Microservice abonné à la partie {id}", route = request.Route });
		}

		// POST: api/score/{id}/unsubscribe
		[HttpPost("{id}/unsubscribe")]
		public ActionResult Unsubscribe(string id, [FromBody] SubscriptionRequestModel request)
		{
			if (string.IsNullOrWhiteSpace(request.Route))
			{
				return BadRequest(new { message = "La route de callback est obligatoire." });
			}

			if (SubscriptionStore.Subscriptions.ContainsKey(id))
			{
				SubscriptionStore.Subscriptions[id].Remove(request.Route);
			}

			return Ok(new { message = $"Microservice désabonné de la partie {id}", route = request.Route });
		}
	}
}