using System.Runtime.CompilerServices;

class Utilities
{
	public static string[] GetInput(string inputFile, [CallerFilePath] string filepath = "")
	{
		var components = filepath.Split("/");
		components[^1] = inputFile;
		return File.ReadAllLines(string.Join("/", components));
	}

}

class Program
{
	public static void Main(string[] _)
	{
		Day2.Part1("test_input.txt");
		Day2.Part1("input.txt");
		Day2.Part2("test_input.txt");
		Day2.Part2("input.txt");
	}
}