using Pulse = (string Origin, string Destination, bool IsHighPulse);

class Day20
{
	class Module(string name, List<string> destinations)
	{
		public string name = name;
		public List<string> destinations = destinations;

		public virtual IEnumerable<Pulse> ReceivePulse(bool IsHighPulse, string origin) => [];
	}

	class FlipFlop(string name, List<string> destinations) : Module(name, destinations)
	{
		bool On = false;

		public override IEnumerable<Pulse> ReceivePulse(bool IsHighPulse, string origin)
		{
			if (IsHighPulse) return [];

			On = !On;
			return destinations.Select(d => (name, d, On));
		}
	}

	class Conjunction(string name, List<string> destinations) : Module(name, destinations)
	{
		readonly Dictionary<string, bool> Cache = [];

		public void SetOrigins(IEnumerable<string> origins)
		{
			foreach (var origin in origins) Cache[origin] = false;
		}

		public override IEnumerable<Pulse> ReceivePulse(bool IsHighPulse, string origin)
		{
			Cache[origin] = IsHighPulse;
			var sendLowPulse = Cache.Values.AllEqual() && Cache.Values.First();
			return destinations.Select(d => (name, d, !sendLowPulse));
		}
	}

	class Broadcast(string name, List<string> destinations) : Module(name, destinations)
	{
		public override IEnumerable<Pulse> ReceivePulse(bool IsHighPulse, string origin) =>
			destinations.Select(d => (name, d, IsHighPulse));
	}

	private static Dictionary<string, Module> ParseInput(IEnumerable<string> input){
		var modules = input.Select(line =>
		{
			var destinations = line.Split("-> ")[1].Split(", ").ToList();
			return line[0] switch
			{
				'%' => new FlipFlop(line.Split(' ')[0][1..], destinations),
				'&' => new Conjunction(line.Split(' ')[0][1..], destinations),
				'b' => new Broadcast(line.Split(' ')[0], destinations),
				_ => new Module(line.Split(' ')[0], destinations)
			};
		}).ToDictionary(m => m.name);

		foreach (var conjunction in modules.Values.Where(m => m is Conjunction))
		{
			((Conjunction)conjunction).SetOrigins(
				modules.Where(m => m.Value.destinations.Contains(conjunction.name)).Select(m => m.Value.name)
			);
		}

		return modules;
	}

	public static string Part1(string inputFile)
	{
		var modules = ParseInput(Utilities.GetInput(inputFile));

		var lowPulses = 0;
		var highPulses = 0;

		var queue = new List<Pulse>();
		for (var i = 0; i < 1000; i++)
		{
			queue.Add(("button", "broadcaster", false));
			while (queue.Count > 0)
			{
				var (Origin, Destination, IsHighPulse) = queue[0];
				if (IsHighPulse) highPulses++; else lowPulses++;
				queue.AddRange(modules[Destination].ReceivePulse(IsHighPulse, Origin));
				queue.RemoveAt(0);
			}
		}

		return (highPulses * lowPulses).ToString();
	}

	public static string Part2(string inputFile)
	{
		var modules = ParseInput(Utilities.GetInput(inputFile));

		if (modules.Count < 10) return "";

		var queue = new List<Pulse>();
		var buttonPresses = 0;
		var preceders = new Dictionary<string, int>() { ["bh"] = 0, ["jf"] = 0, ["sh"] = 0, ["mz"] = 0 };

		while (preceders.Values.Any(x => x == 0))
		{
			queue.Add(("button", "broadcaster", false));
			buttonPresses++;
			while (queue.Count > 0)
			{
				var (Origin, Destination, IsHighPulse) = queue[0];
				if (preceders.TryGetValue(Origin, out int value) && value == 0 && IsHighPulse)
				{
					preceders[Origin] = buttonPresses;
				}
				queue.AddRange(modules[Destination].ReceivePulse(IsHighPulse, Origin));
				queue.RemoveAt(0);
			}
		}

		return preceders.Values.Aggregate(1L, (a, b) => a / GCD(a, b) * b).ToString();
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
