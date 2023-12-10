using AdventOfCode2023;

class Day10
{
	enum NodeState { Unknown, Inside, Outside, MainLoop }

	class ConnectedNode(int x, int y, char pipe) : Point(x, y)
	{
		public bool NConnected { get; set; } = "|LJS".Contains(pipe);
		public bool SConnected { get; set; } = "|7FS".Contains(pipe);
		public bool EConnected { get; set; } = "-LFS".Contains(pipe);
		public bool WConnected { get; set; } = "-7JS".Contains(pipe);

		public NodeState State { get; set; } = NodeState.Unknown;


		public void ValidateConnections(Dictionary<(int x, int y), ConnectedNode> grid)
		{
			NConnected &= grid.GetValueOrDefault((x - 1, y))?.SConnected == true && (SConnected || EConnected || WConnected);
			SConnected &= grid.GetValueOrDefault((x + 1, y))?.NConnected == true && (NConnected || EConnected || WConnected);
			EConnected &= grid.GetValueOrDefault((x, y + 1))?.WConnected == true && (NConnected || SConnected || WConnected);
			WConnected &= grid.GetValueOrDefault((x, y - 1))?.EConnected == true && (NConnected || SConnected || EConnected);
		}

		public IEnumerable<ConnectedNode> Connections(Dictionary<(int x, int y), ConnectedNode> grid) =>
			((IEnumerable<ConnectedNode?>)[
				NConnected ? grid.GetValueOrDefault((x - 1, y)) : null, SConnected ? grid.GetValueOrDefault((x + 1, y)) : null,
				EConnected ? grid.GetValueOrDefault((x, y + 1)) : null, WConnected ? grid.GetValueOrDefault((x, y - 1)) : null,
			]).Where(x => x is not null)!;
	}

	public static string Part1(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var grid = input.SelectMany((row, x) => row.Select((pipe, y) => new ConnectedNode(x, y, pipe)))
			.ToDictionary(node => (node.x, node.y), node => node);
		var start = grid.Values.First(node => node.NConnected && node.SConnected && node.EConnected && node.WConnected);
		grid.Values.ToList().ForEach(node => node.ValidateConnections(grid));

		var visited = new HashSet<ConnectedNode>();
		var queue = new Queue<(int Index, ConnectedNode Node)>([(0, start)]);
		var current = queue.Peek();

		while (queue.Count != 0)
		{
			current = queue.Dequeue();
			visited.Add(current.Node);
			current.Node.Connections(grid).Where(n => !visited.Contains(n) && !queue.Any(q => q.Node == n))
				.Select(n => (Index: current.Index + 1, Node: n)).ToList().ForEach(queue.Enqueue);
		}

		return current.Index.ToString();
	}

	public static string Part2(string inputFile)
	{
		var input = Utilities.GetInput(inputFile);
		var grid = input.SelectMany((row, x) => row.Select((pipe, y) => new ConnectedNode(x, y, pipe)))
			.ToDictionary(node => (node.x, node.y), node => node);
		var current = grid.Values.First(node => node.NConnected && node.SConnected && node.EConnected && node.WConnected);
		grid.Values.ToList().ForEach(node => node.ValidateConnections(grid));

		while (current is not null)
		{
			current.State = NodeState.MainLoop;
			current = current.Connections(grid).FirstOrDefault(t => t.State != NodeState.MainLoop);
		}

		for (var x = 0; x < input.Length; x++)
		{
			var inside = false;
			for (var y = 0; y < input[0].Length; y++)
			{
				var cell = grid[(x, y)];
				if (cell.State == NodeState.Unknown) {
					cell.State = inside ? NodeState.Inside : NodeState.Outside;
				} else if (cell.State == NodeState.MainLoop && cell.NConnected) {
					inside = !inside;
				}
			}
		}

		return grid.Values.Where(t => t.State == NodeState.Inside).Count().ToString();
	}
}
