class Day5
{
	private static IEnumerable<long> LongsFrom(string input) =>
		input.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(long.Parse);

	private static List<IEnumerable<(long destStart, long sourceStart, long rangeLength)>> GetMaps(IEnumerable<string> input)
	{
		var maps = new List<IEnumerable<(long destStart, long sourceStart, long rangeLength)>>();
		var currentMap = new List<(long destStart, long sourceStart, long rangeLength)>();

		input.ToList().ForEach(line =>
		{
			if (line.Trim() == "")
			{
				maps.Add(currentMap);
				currentMap = [];
			}
			else if ("0123456789".Contains(line[0]))
			{
				var parsed = LongsFrom(line).ToList();
				currentMap.Add((parsed[0], parsed[1], parsed[2]));
			}
		});
		maps.Add(currentMap);

		return maps;
	}

	public static string Part1(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var seeds = LongsFrom(input[0].Replace("seeds:", ""));
		var maps = GetMaps(input.Skip(1));

		return seeds.Min(seed => maps.Aggregate(seed, (curValue, map) =>
			{
				var (destStart, sourceStart, _) = map.FirstOrDefault
					(tuple => tuple.sourceStart <= curValue && curValue <= (tuple.sourceStart + tuple.rangeLength),
					(destStart: curValue, sourceStart: curValue, rangeLength: 1));
				return curValue - sourceStart + destStart;
			})).ToString();
	}

	private static IEnumerable<long> Range(long start, long count)
	{
		var end = start + count;
		for (var current = start; current < end; ++current)
		{
			yield return current;
		}
	}

	public static string Part2(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var seeds = LongsFrom(input[0].Replace("seeds:", "")).Chunk(2);
		var maps = GetMaps(input.Skip(1));

		return seeds.Min(seedPair =>
			Range(seedPair.First(), seedPair.Last()).Min(seed => maps.Aggregate(seed, (curValue, map) =>
				{
					var (destStart, sourceStart, _) = map.FirstOrDefault
						(tuple => tuple.sourceStart <= curValue && curValue <= (tuple.sourceStart + tuple.rangeLength),
						(destStart: curValue, sourceStart: curValue, rangeLength: 1));
					return curValue - sourceStart + destStart;
				}))
		).ToString();
	}
}
