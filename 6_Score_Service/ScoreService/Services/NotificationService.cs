using ScoreService.IServices;
using ScoreService.Models;

namespace ScoreService.Services
{
	public class NotificationService(HttpClient httpClient) : INotificationService
	{
		private readonly HttpClient _httpClient = httpClient;

		public async Task NotifySubscribersAsync(string gameId, ScoreModel score)
		{
			if (!SubscriptionStore.Subscriptions.ContainsKey(gameId)) return;

			foreach (var subscriber in SubscriptionStore.Subscriptions[gameId])
			{
				try
				{
					await _httpClient.PostAsJsonAsync(subscriber, score);
				}
				catch (Exception ex)
				{
					// Loggage de l'erreur
					Console.WriteLine($"Erreur de notification {subscriber} : {ex.Message}");
				}
			}
		}
	}
}