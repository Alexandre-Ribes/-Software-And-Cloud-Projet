

using Microsoft.AspNetCore.Mvc;

namespace GameManager.Controllers
{
	[Route("[controller]")] // game
	[ApiController]

	public class GameController : ControllerBase
	{

		[HttpGet("start")] // game/start
		public ActionResult<string> Start()
		{
			Game gameSession = new Game().GetInstance();

			// Retourne une erreur HTTP si la session est null (pas normal lors de la création)
			if (gameSession == null)
			{
				return NotFound();
			}

			return gameSession.GetSessionId();
		}

		[HttpGet("{id}/stop")] // game/{id}/stop
		public ActionResult Stop(string id)
		{
			// retourne une erreur si l'id est null
			if (id == null)
			{
				return NotFound();
			}

			Game gameSession = new Game().GetInstance(id);

			// Retourne une erreur HTTP si aucune session ne correspond à l'id mentionné
			if (gameSession == null)
			{
				return NotFound();
			}

			gameSession.Stop(); ;
			return Ok();
		}

		[HttpGet("{id}/status")] // game/{id}/status
		public ActionResult<bool> GetStatus(string id)
		{
			// retourne une erreur si l'id est null
			if (id == null)
			{
				return NotFound();
			}
			Game gameSession = new Game().GetInstance(id);

			// Retourne une erreur HTTP si aucune session ne correspond à l'id mentionné
			if (gameSession == null)
			{
				return NotFound();
			}

			return gameSession.GetIsRunning();
		}

	}
}
