namespace ScoreService.Models
{
	public class ScoreModel
	{
		public int Score { get; set; } = 0;
		public string? GameId { get; set; }
		public DateTime LastUpdated { get; set; }

		public void Increment()
		{
			Score++;
			LastUpdated = DateTime.UtcNow;
		}
	}
}