class Day9
{
	private static int GetLastDelta(IEnumerable<int> elements)
	{
		var deltas = elements.Zip(elements.Skip(1)).Select(pair => pair.Second - pair.First);
		return deltas.AllEqual() ? deltas.First() : deltas.Last() + GetLastDelta(deltas);
	}

	private static int GetFirstDelta(IEnumerable<int> elements)
	{
		var deltas = elements.Zip(elements.Skip(1)).Select(pair => pair.Second - pair.First);
		return deltas.AllEqual() ? deltas.First() : deltas.First() - GetFirstDelta(deltas);
	}

	public static string Part1(string inputFile) => Utilities.GetInput(inputFile).Sum(row =>
		{
			var elements = Utilities.IntsFrom(row).ToList();
			return elements.Last() + GetLastDelta(elements);
		}).ToString();

	public static string Part2(string inputFile) => Utilities.GetInput(inputFile).Sum(row =>
		{
			var elements = Utilities.IntsFrom(row);
			return elements.First() - GetFirstDelta(elements);
		}).ToString();
}
