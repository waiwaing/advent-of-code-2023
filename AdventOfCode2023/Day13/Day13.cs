class Day13
{
	public static int? FindSymmetryIndex(List<string> grid, int indexToIgnore = -1)
	{
		for (int symmetryIndex = 0; symmetryIndex < grid.Count - 1; symmetryIndex++)
		{
			if (symmetryIndex == indexToIgnore - 1) continue;
			var valid = true;

			for (int rowA = symmetryIndex; valid && rowA >= 0; rowA--)
			{
				int rowB = symmetryIndex + symmetryIndex - rowA + 1;
				if (rowB >= grid.Count) return symmetryIndex + 1;
				if (!grid[rowA].Equals(grid[rowB])) valid = false;
			}
			if (valid) return symmetryIndex + 1;
		}
		return null;
	}

	public static List<List<string>> InputToGrids(IEnumerable<string> input) =>
		input.Aggregate(new List<List<string>>([[]]), (acc, line) =>
		{
			if (line.Trim() == "") acc.Add([]); else acc.Last().Add(line);
			return acc;
		});

	public static List<string> SmudgeGrid(List<string> grid, int smudge)
	{
		var col = smudge % grid[0].Length;
		return grid.Select((row, index) =>
			index == (smudge / grid[0].Length) ? row[..col] + (row[col] == '.' ? '#' : '.') + row[(col + 1)..] : row
		).ToList();
	}

	public static string Part1(string inputFile) => 
		InputToGrids(Utilities.GetInput(inputFile)).Select(grid =>
			{
				var horizontalSymmetryIndex = FindSymmetryIndex(grid);
				if (horizontalSymmetryIndex is not null) return 100 * (int) horizontalSymmetryIndex;
				return (int) FindSymmetryIndex(Utilities.TransposeGrid(grid))!;
			}
		).Sum().ToString();

    public static string Part2(string inputFile) => 
		InputToGrids(Utilities.GetInput(inputFile)).Select(grid =>
			{
				var origHSI = FindSymmetryIndex(grid);
				var origCSI = FindSymmetryIndex(Utilities.TransposeGrid(grid));

				for (int smudge = 0; smudge < grid.Count * grid[0].Length; smudge++)
				{
					var smudgedGrid = SmudgeGrid(grid, smudge);

					var hsi = FindSymmetryIndex(smudgedGrid, origHSI ?? -1);
					if (hsi is not null) return 100 * (int) hsi;
					var csi = FindSymmetryIndex(Utilities.TransposeGrid(smudgedGrid), origCSI ?? -1);
					if (csi is not null) return (int) csi;
				}

				throw new Exception("No symmetry");
			}
		).Sum().ToString();
}
