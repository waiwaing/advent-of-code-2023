class Day14
{
	public static char[] RollStones(char[] line)
	{
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] == 'O')
			{
				line[i] = '.';
				for (int j = i; j >= 0; j--)
				{
					if (j == 0 || line[j - 1] != '.')
					{
						line[j] = 'O';
						break;
					}
				}
			}
		}

		return line;
	}

	public static int ScoreLine(string line) =>
		line.Select((character, i) => character == 'O' ? line.Length - i : 0).Sum();

	public static List<char[]> RotateGridClockwise(List<char[]> grid)
	{
		var newGrid = Enumerable.Range(0, grid.Count).Select(_ => new char[grid.Count]).ToList();

		for (int i = 0; i < grid.Count; i++)
		{
			for (int j = 0; j < grid.Count; j++)
			{
				newGrid[grid.Count - j - 1][i] = grid[i][j];
			}
		}

		return newGrid;
	}

	public static string Part1(string inputFile)
	{
		var cols = Utilities.TransposeGrid([.. Utilities.GetInput(inputFile)]);
		return cols.Select(col => string.Join("", RollStones([.. col]))).Sum(ScoreLine).ToString();
	}

	public static string Part2(string inputFile)
	{
		var cols = Utilities.TransposeGrid([.. Utilities.GetInput(inputFile)])
			.Select<string, char[]>(t => [.. t]).ToList();
		Dictionary<string, long> cache = [];
		var i = 0L;

		while (true)
		{
			cols = RotateGridClockwise(cols.Select(RollStones).ToList());

			var serialisedGrid = string.Join("", cols.SelectMany(col => col.Select(t => t)));
			if (!cache.TryAdd(serialisedGrid, i) && i < 3_000_000_000)
			{
				var loopSize = i - cache[serialisedGrid];
				var multiples = (4_000_000_000 - cache[serialisedGrid]) / loopSize - 10;
				i += multiples * loopSize;
			}

			i++;
			if (i == 4_000_000_000) break;
		}

		return cols.Select(col => ScoreLine(string.Join("", col))).Sum().ToString();
	}
}
