namespace CardCalculator.Exceptions
{
	public static class Exceptions
	{
		public static string NoException { get; set; } = "";
		public static string TooManyJokers { get; set; } = "A hand cannot contain more than two Jokers, removing excess Jokers";
		public static string EmptyHand { get; set; } = "There are no cards in your hand";
		public static string UnrecognisedValue { get; set; } = "Card not recognised, take a look at your hand";
		public static string InvalidEntry { get; set; } = "Your cards need to be separated by a comma!";
		public static string DuplicateCard { get; set; } = "Your hand contains a duplicate";
	}
}
