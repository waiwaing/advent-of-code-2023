class Day7
{

	private static int GetGroupsScore(List<int> groups) => groups switch
	{
	[5] => 7,
	[4, 1] => 6,
	[3, 2] => 5,
	[3, 1, 1] => 4,
	[2, 2, 1] => 3,
	[2, 1, 1, 1] => 2,
	[1, 1, 1, 1, 1] => 1,
		_ => throw new Exception("oops")
	};

	private static int GetHandTypeScore(string hand) =>
		GetGroupsScore([.. hand.GroupBy(c => c).Select(t => t.Count()).OrderDescending()]);

	private static int GetHandTypeScoreJ(string hand)
	{
		var jokers = hand.Count(t => t == 'J');
		if (jokers == 5) return 7;

		var groups = hand.Where(t => t != 'J').GroupBy(c => c).Select(t => t.Count()).OrderDescending().ToList();
		groups[0] += jokers;
		return GetGroupsScore(groups);
	}

	private static long GetHandOrderScore(string hand, string ordering) =>
		(long)Enumerable.Range(0, 5).Select(i => ordering.IndexOf(hand[i]) * Math.Pow(14, 5 - i)).Sum();

	public static string Part1(string inputFile) =>
		Utilities.GetInput(inputFile).Select(line => line.Split(" "))
			.Select(t => (hand: t[0], bid: int.Parse(t[1])))
			.OrderBy(h => GetHandTypeScore(h.hand)).ThenBy(h => GetHandOrderScore(h.hand, "23456789TJQKA"))
			.Aggregate((score: 0, index: 1), (acc, hand) => (score: acc.score + hand.bid * acc.index, index: acc.index + 1))
			.score.ToString();

	public static string Part2(string inputFile) =>
		Utilities.GetInput(inputFile).Select(line => line.Split(" "))
			.Select(t => (hand: t[0], bid: int.Parse(t[1])))
			.OrderBy(h => GetHandTypeScoreJ(h.hand)).ThenBy(h => GetHandOrderScore(h.hand, "J23456789TQKA"))
			.Aggregate((score: 0, index: 1), (acc, hand) => (score: acc.score + hand.bid * acc.index, index: acc.index + 1))
			.score.ToString();
}
