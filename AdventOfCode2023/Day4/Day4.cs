class Day4
{
	public static string Part1(string inputFile) =>
		Utilities.GetInput(inputFile)
			.Select(line =>
			{
				var parsed = line.Split(": ").Last().Split("|");
				return (winning: Utilities.IntsFrom(parsed[0]), have: Utilities.IntsFrom(parsed[1]));
			})
			.Sum(res => (int)Math.Pow(2, res.have.Count(res.winning.Contains) - 1))
			.ToString();

	public static string Part2(string inputFile)
	{
		var state = Utilities.GetInput(inputFile)
			.Select((line, i) =>
			{
				var parsed = line.Split(": ").Last().Split("|");
				return (copies: 1, matches: Utilities.IntsFrom(parsed[1]).Count(Utilities.IntsFrom(parsed[0]).Contains));
			}).ToArray();

		Utilities.RangeAsList(0, state.Length).ForEach(i => Utilities.RangeAsList(0, state[i].matches).ForEach(j =>
				state[i + j + 1].copies += state[i].copies
		));

		return state.Sum(t => t.copies).ToString();
	}
}
