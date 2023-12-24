class Day15
{
	private static int Hash(string input) => input.Aggregate(0, (acc, c) => (acc + c) * 17 % 256);

	public static string Part1(string inputFile) => Utilities.GetInput(inputFile)[0].Split(",").Sum(Hash).ToString();

	public static string Part2(string inputFile)
	{
		var instructions = Utilities.GetInput(inputFile)[0].Split(",").ToList();
		var hashmap = new Dictionary<int, List<(string Label, int Lens)>>();

		instructions.ForEach(instruction =>
		{
			var label = instruction.Split(['=', '-'])[0];
			var box = Hash(label);
			hashmap.TryGetValue(box, out List<(string Label, int Lens)>? contents);

			var lensIndex = contents?.FindIndex(lens => lens.Label == label);
			_ = int.TryParse(instruction.Last().ToString(), out var newLens);

			switch (instruction.Contains('-'), lensIndex)
			{
				case (true, > -1): contents!.RemoveAt((int)lensIndex); break;
				case (false, > -1): contents![(int)lensIndex] = new(label, newLens); break;
				case (false, -1): contents!.Add(new(label, newLens)); break;
				case (false, null): hashmap[box] = [new(label, newLens)]; break;
			}
		});

		return hashmap.Sum(kv =>
			kv.Value.Select((lens, index) => (kv.Key + 1) * (index + 1) * lens.Lens).Sum()
		).ToString();
	}
}
