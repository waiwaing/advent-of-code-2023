class Day11
{
	public static string Part1(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var xes = input.SelectMany((row, x) => Enumerable.Repeat(x, row.Count(t => t == '#'))).ToList();
		var yes = Enumerable.Range(0, input[0].Length)
			.SelectMany(y => Enumerable.Repeat(y, input.Count(row => row[y] == '#'))).ToList();

		var emptyX = Enumerable.Range(0, input.Length).Except(xes).ToList();
		var emptyY = Enumerable.Range(0, input[0].Length).Except(yes).ToList();

		var dX = xes.Select((x1, i) => xes.Skip(i + 1).Sum(x2 => x2 - x1 + emptyX.Count(x => x > x1 && x < x2)));
		var dY = yes.Select((y1, i) => yes.Skip(i + 1).Sum(y2 => y2 - y1 + emptyY.Count(y => y > y1 && y < y2)));

		return (dX.Sum() + dY.Sum()).ToString();
	}

	public static string Part2(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var xes = input.SelectMany((row, x) => Enumerable.Repeat(x, row.Count(t => t == '#'))).ToList();
		var yes = Enumerable.Range(0, input[0].Length)
			.SelectMany(y => Enumerable.Repeat(y, input.Count(row => row[y] == '#'))).ToList();

		var emptyX = Enumerable.Range(0, input.Length).Except(xes).ToList();
		var emptyY = Enumerable.Range(0, input[0].Length).Except(yes).ToList();
		var expansion = 1_000_000 - 1;

		var dX = xes.Select((x1, i) => xes.Skip(i).Sum(x2 => (long)x2 - x1 + expansion * emptyX.Count(x => x > x1 && x < x2)));
		var dY = yes.Select((y1, i) => yes.Skip(i).Sum(y2 => (long)y2 - y1 + expansion * emptyY.Count(y => y > y1 && y < y2)));

		return (dX.Sum() + dY.Sum()).ToString();
	}
}
