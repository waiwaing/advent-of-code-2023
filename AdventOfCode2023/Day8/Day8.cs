class Day8
{
	public static string Part1(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var lr = input[0];
		var nodes = input.Skip(2).ToDictionary(line => line[0..3], line => (Left: line[7..10], Right: line[12..15]));

		var currentNode = "AAA";
		var steps = 0;
		while (currentNode != "ZZZ")
		{
			currentNode = lr[steps % lr.Length] == 'L' ? nodes[currentNode].Left : nodes[currentNode].Right;
			steps += 1;
		}

		return steps.ToString();
	}

	public static string Part2(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var instructions = input[0];
		var nodes = input.Skip(2).ToDictionary(line => line[0..3], line => (Left: line[7..10], Right: line[12..15]));

		return nodes.Where(t => t.Key[2] == 'A').Select(t => t.Key).Select(node =>
		{
			var steps = 0;
			while (node[2] != 'Z')
			{
				node = instructions[steps % instructions.Length] == 'L' ? nodes[node].Left : nodes[node].Right;
				steps += 1;
			}
			return (long)steps;
		}).Aggregate((a, b) => a / GCD(a, b) * b).ToString();
	}

	// https://stackoverflow.com/a/41766138
	private static long GCD(long a, long b)
	{
		while (a != 0 && b != 0)
		{
			if (a > b) a %= b;
			else b %= a;
		}

		return a | b;
	}
}
