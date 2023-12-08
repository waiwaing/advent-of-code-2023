using System.Runtime.CompilerServices;

class Utilities
{
	public static string[] GetInput(string inputFile, [CallerFilePath] string filepath = "")
	{
		var components = filepath.Split("/");
		components[^1] = inputFile;
		return File.ReadAllLines(string.Join("/", components));
	}

	public static List<int> RangeAsList(int start, int length) => Enumerable.Range(start, length).ToList();
}

class Program
{
	public static void Main(string[] _)
	{
		var day = Type.GetType("Day7");
		List<(string part, string filename)> invocations = [
			("Part1", "test_input.txt"), ("Part1", "input.txt"),
			("Part2", "test_input_2.txt"), ("Part2", "input.txt"),
		];

		foreach (var (part, filename) in invocations)
		{
			Console.WriteLine($"{day} {part} {filename}: {day!.GetMethod(part)!.Invoke(null, [filename])}");
		}
	}
}