namespace ScoreService
{
	public class SubscriptionStore
	{
		// clé = GameId, valeur = liste d'URL abonnés
		public static Dictionary<string, List<string>> Subscriptions = new();
	}
}