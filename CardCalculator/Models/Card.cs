namespace CardCalculator.Models
{
	public class Card
	{
		public string? Value { get; set; } = null;
		public string? Suit { get; set; } = null;
		public bool isJoker { get; set; } = false;
	}
}
