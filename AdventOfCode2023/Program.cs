using System.Runtime.CompilerServices;

static class Utilities
{
	public static string[] GetInput(string inputFile, [CallerFilePath] string filepath = "")
	{
		var components = filepath.Split("/");
		components[^1] = inputFile;
		return File.ReadAllLines(string.Join("/", components));
	}

	public static List<int> RangeAsList(int start, int length) => Enumerable.Range(start, length).ToList();

	public static IEnumerable<int> IntsFrom(string input) =>
		input.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse);

	public static bool AllEqual<T>(this IEnumerable<T> enumerable) => enumerable.Distinct().Count() <= 1;

	public static List<string> TransposeGrid(List<string> grid) =>
		Enumerable.Range(0, grid[0].Length).Select(i => string.Join("", grid.Select(line => line[i].ToString()))).ToList();

}

class Program
{
	public static void Main(string[] _)
	{
		var day = Type.GetType("Day14");
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