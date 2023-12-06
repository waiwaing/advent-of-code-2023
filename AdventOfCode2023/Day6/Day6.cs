class Day6
{
	private static IEnumerable<int> IntsFrom(string input) =>
		input.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse);

	public static string Part1(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var races = Enumerable.Zip(IntsFrom(input[0].Split(":")[1]), IntsFrom(input[1].Split(":")[1]));

		return races.Select(race =>
		{
			var discriminant = Math.Sqrt(race.First * race.First - 4 * (race.Second + 0.00001));
			var minX = (int)Math.Ceiling((race.First - discriminant) / 2);
			var maxX = (int)((race.First + discriminant) / 2);

			return maxX - minX + 1;
		}).Aggregate((x, y) => x * y).ToString();
	}

	public static string Part2(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var time = long.Parse(input[0].Split(":")[1].Replace(" ", ""));
		var distance = long.Parse(input[1].Split(":")[1].Replace(" ", ""));

		var discriminant = Math.Sqrt(time * time - 4 * (distance + 0.00001));
		var minX = (long)Math.Ceiling((time - discriminant) / 2);
		var maxX = (long)((time + discriminant) / 2);

		return (maxX - minX + 1).ToString();
	}
}
