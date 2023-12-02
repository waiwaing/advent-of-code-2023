class Day2
{
	public static void Part1(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var result = input
			.Where(line =>
				line.Split(":").Last().Split(";").Select(set =>
					{
						var entries = set.Split(",").Select(entry => entry.Trim().Split(" "));
						return entries.Aggregate(new { Red = 0, Green = 0, Blue = 0 }, (acc, entry) =>
							entry[1] switch
								{
									"red" => acc with { Red = acc.Red + int.Parse(entry[0]) },
									"green" => acc with { Green = acc.Green + int.Parse(entry[0]) },
									"blue" => acc with { Blue = acc.Blue + int.Parse(entry[0]) },
									_ => throw new Exception($"Unexpected color {entry[1]}"),
								}
							);
					}).All(dice => dice.Red <= 12 && dice.Green <= 13 && dice.Blue <= 14)
			).Sum(line => int.Parse(line.Split(":").First().Split(" ").Last()));

		Console.WriteLine(result);
	}


	public static void Part2(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var result = input
			.Select(line =>
				line.Split(":").Last().Split(";").Select(set =>
					{
						var entries = set.Split(",").Select(entry => entry.Trim().Split(" "));
						return entries.Aggregate(new { Red = 0, Green = 0, Blue = 0 }, (acc, entry) =>
							entry[1] switch
								{
									"red" => acc with { Red = acc.Red + int.Parse(entry[0]) },
									"green" => acc with { Green = acc.Green + int.Parse(entry[0]) },
									"blue" => acc with { Blue = acc.Blue + int.Parse(entry[0]) },
									_ => throw new Exception($"Unexpected color {entry[1]}"),
								}
							);
					}).Aggregate(new { Red = 0, Green = 0, Blue = 0 }, (acc, entry) => new
					{
						Red = int.Max(acc.Red, entry.Red),
						Green = int.Max(acc.Green, entry.Green),
						Blue = int.Max(acc.Blue, entry.Blue)
					})
			).Select(game => game.Red * game.Blue * game.Green)
			.Sum();

		Console.WriteLine(result);
	}
}
