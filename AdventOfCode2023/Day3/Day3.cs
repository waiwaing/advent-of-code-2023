using AdventOfCode2023;

class Day3
{
	private static int AsInt(Point start, Point end, string[] input) =>
		int.Parse(string.Join("", Enumerable.Range(start.y, end.y - start.y + 1).Select(y =>
			input[start.x][y]
		)));

	public static string Part1(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);

		bool isNumber(Point p) => p.x >= 0 && p.x < input.Length && p.y >= 0 && p.y < input[p.x].Length &&
			int.TryParse(input[p.x][p.y].ToString(), out _);
		var nonSymbols = ".0123456789";

		return Enumerable
			.Range(0, input.Length).SelectMany(x => Enumerable.Range(0, input[x].Length).Select(y => new Point(x, y)))
			.Where(isNumber)
			.Select(p => p.ContiguousOnLine(isNumber))
			.Distinct()
			.Where(pointRange => Enumerable.Range(pointRange.start.y, pointRange.end.y - pointRange.start.y + 1).SelectMany(y =>
					new Point(pointRange.start.x, y).AdjacentPoints()
				).Distinct()
				 .Where(point => point.Inside(0, input.Length - 1, 0, input[0].Length - 1))
				 .Where(point => !(point.x == pointRange.start.x && point.y >= pointRange.start.y && point.y <= pointRange.end.y))
				 .Any(point => !nonSymbols.Contains(input[point.x][point.y])))
			.Sum(pointRange => AsInt(pointRange.start, pointRange.end, input))
			.ToString();
	}

	public static string Part2(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		bool isNumber(Point p) => p.x >= 0 && p.x < input.Length && p.y >= 0 && p.y < input[p.x].Length &&
			int.TryParse(input[p.x][p.y].ToString(), out _);

		return Enumerable
			.Range(0, input.Length).SelectMany(x => Enumerable.Range(0, input[x].Length).Select(y => new Point(x, y)))
			.Where(point => input[point.x][point.y] == '*')
			.Sum(asterisk =>
			{
				var gears = asterisk.AdjacentPoints()
					.Where(isNumber)
					.Select(point => point.ContiguousOnLine(isNumber))
					.Distinct()
					.Select(pointRange => AsInt(pointRange.start, pointRange.end, input));

				return gears.Count() == 2 ? gears.First() * gears.Last() : 0;
			})
			.ToString();
	}
}
