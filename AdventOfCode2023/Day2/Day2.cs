class Day2
{
	private static (int R, int G, int B) ParseSet(string set) =>
		set.Split(",").Select(entry => entry.Trim().Split(" "))
			.Aggregate((R: 0, G: 0, B: 0), (acc, entry) => entry[1] switch
				{
					"red" => acc with { R = acc.R + int.Parse(entry[0]) },
					"green" => acc with { G = acc.G + int.Parse(entry[0]) },
					"blue" => acc with { B = acc.B + int.Parse(entry[0]) },
					_ => throw new Exception($"Unexpected color {entry[1]}"),
				}
		);

	public static string Part1(string inputFile) => Utilities.GetInput(inputFile)
		.Where(line => line.Split(":").Last().Split(";").Select(ParseSet)
			.All(dice => dice.R <= 12 && dice.G <= 13 && dice.B <= 14))
		.Sum(line => int.Parse(line.Split(":").First().Split(" ").Last()))
		.ToString();

	public static string Part2(string inputFile) => Utilities.GetInput(inputFile)
		.Select(line => line.Split(":").Last().Split(";").Select(ParseSet)
			.Aggregate((R: 0, G: 0, B: 0), (acc, entry) =>
				(R: int.Max(acc.R, entry.R), G: int.Max(acc.G, entry.G), B: int.Max(acc.B, entry.B))
			))
		.Sum(game => game.R * game.G * game.B)
		.ToString();
}
