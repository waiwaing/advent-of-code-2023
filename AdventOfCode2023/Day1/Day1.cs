using System.Text;

class Day1
{
	public static void Part1()
	{
		var input = File.ReadAllLines("Day1/input.txt");
		var result = input.Sum(line =>
		{
			var firstDigit = line.First(c => int.TryParse(c.ToString(), out _));
			var lastDigit = line.Last(c => int.TryParse(c.ToString(), out _));

			return int.Parse($"{firstDigit}{lastDigit}");
		});

		Console.WriteLine(result);
	}

	private static readonly Dictionary<string, string> numbers = new() {
		{"one", "1"},
		{"two", "2"},
		{"three", "3"},
		{"four", "4"},
		{"five", "5"},
		{"six", "6"},
		{"seven", "7"},
		{"eight", "8"},
		{"nine", "9"}
	};

	public static void Part2()
	{
		var input = File.ReadAllLines("Day1/input.txt");
		var result = input.Sum(line =>
		{
			var sb = new StringBuilder(line);
			foreach (var keyValuePair in numbers)
			{
				sb.Replace(keyValuePair.Key, keyValuePair.Key + keyValuePair.Value + keyValuePair.Key);
			}
			var newString = sb.ToString();

			var firstDigit = newString.First(c => int.TryParse(c.ToString(), out _));
			var lastDigit = newString.Last(c => int.TryParse(c.ToString(), out _));

 			return int.Parse($"{firstDigit}{lastDigit}");
		});

		Console.WriteLine(result);
	}
}
