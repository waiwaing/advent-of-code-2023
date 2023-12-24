using System.Data;

using Item = (string X, string M, string A, string S);

class Day19
{
	public record struct ItemRange(int MinX, int MaxX, int MinM, int MaxM, int MinA, int MaxA, int MinS, int MaxS);

	public static bool ProcessWorkflow(Dictionary<string, string[]> workflows, string key, Item item)
	{
		var decision = workflows[key].First(rule =>
		{
			var conditional = rule.Split(":")[0].Replace("x", item.X).Replace("m", item.M).Replace("a", item.A).Replace("s", item.S);
			return !rule.Contains(':') || (bool)new DataTable().Compute(conditional, "");
		}).Split(":").Last();

		return decision switch
		{
			"A" => true,
			"R" => false,
			_ => ProcessWorkflow(workflows, decision, item),
		};
	}

	private static void ProcessDecision(List<ItemRange> validRanges, string decision, Dictionary<string, string[]> workflows, ItemRange itemRange)
	{
		switch (decision)
		{
			case "A": validRanges.Add(itemRange); break;
			case "R": break;
			default: validRanges.AddRange(FindValidRanges(workflows, decision, itemRange)); break;
		}
	}

	public static IEnumerable<ItemRange> FindValidRanges(Dictionary<string, string[]> workflows, string key, ItemRange itemRange)
	{
		var result = new List<ItemRange>();
		foreach (var rule in workflows[key])
		{
			var split = rule.Split(":").ToList();
			var rangeToProcess = itemRange; // Default passing case if no conditional

			if (rule.Contains(':'))
			{
				var value = int.Parse(split[0][2..]);
				(rangeToProcess, itemRange) = split[0][0..2] switch
				{ // first element: passing conditional, second element: failing conditional
					"x>" => (itemRange with { MinX = value + 1 }, itemRange with { MaxX = value }),
					"x<" => (itemRange with { MaxX = value - 1 }, itemRange with { MinX = value }),
					"m>" => (itemRange with { MinM = value + 1 }, itemRange with { MaxM = value }),
					"m<" => (itemRange with { MaxM = value - 1 }, itemRange with { MinM = value }),
					"a>" => (itemRange with { MinA = value + 1 }, itemRange with { MaxA = value }),
					"a<" => (itemRange with { MaxA = value - 1 }, itemRange with { MinA = value }),
					"s>" => (itemRange with { MinS = value + 1 }, itemRange with { MaxS = value }),
					"s<" => (itemRange with { MaxS = value - 1 }, itemRange with { MinS = value }),
					_ => throw new Exception("uh oh")
				};
			}

			ProcessDecision(result, split.Last(), workflows, rangeToProcess); // Process passing case
		}

		return result;
	}

	public static string Part1(string inputFile)
	{
		var input = Utilities.GetInput(inputFile).Where(line => line.Length > 0).ToList();
		var workflows = input.Where(line => line[0] != '{').ToDictionary(
			(line) => line.Split('{')[0],
			(line) => line.Split('{')[1].Split([',', '}'])
		);

		var items = input.Where(line => line[0] == '{').Select(line =>
		{
			var elements = line.Split(',', '{', '}').Where(e => e.Length > 0).Select(e => e[2..]).ToList();
			return (X: elements[0], M: elements[1], A: elements[2], S: elements[3]);
		});

		return items.Where(item => ProcessWorkflow(workflows, "in", item))
			.Sum(item => int.Parse(item.X) + int.Parse(item.M) + int.Parse(item.A) + int.Parse(item.S))
			.ToString();
	}

	public static string Part2(string inputFile)
	{
		var workflows = Utilities.GetInput(inputFile).Where(line => line.Length > 0 && line[0] != '{').ToDictionary(
			(line) => line.Split('{')[0],
			(line) => line.Split('{')[1].Split([',', '}']).Where(x => x.Length > 0).ToArray()
		);

		return FindValidRanges(workflows, "in", new ItemRange(1, 4000, 1, 4000, 1, 4000, 1, 4000))
			.Select(ir => (long)(ir.MaxX - ir.MinX + 1) * (ir.MaxM - ir.MinM + 1) * (ir.MaxA - ir.MinA + 1) * (ir.MaxS - ir.MinS + 1))
			.Sum().ToString();
	}
}
