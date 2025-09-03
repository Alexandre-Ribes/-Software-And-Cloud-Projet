using ScoreService.Models;

namespace ScoreService.IServices
{
	public interface INotificationService
	{
		Task NotifySubscribersAsync(string gameId, ScoreModel score);
	}
}