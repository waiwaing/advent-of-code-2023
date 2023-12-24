using AdventOfCode2023;

class Day16
{
	public class Particle(int x, int y, char direction) : Point(x, y), IEquatable<Particle>
	{
		public char Direction = direction;
		public Particle(Point point, char direction) : this(point.x, point.y, direction) { }

		public static bool operator ==(Particle p1, Particle p2) => p1.Equals(p2);
		public static bool operator !=(Particle p1, Particle p2) => !p1.Equals(p2);

		public bool Equals(Particle? other) =>
			(other is not null) && (x == other.x) && (y == other.y) && Direction == other.Direction;
		public override bool Equals(object? obj) => obj != null && obj is Particle && Equals(obj as Particle);

		public override int GetHashCode() => 31 * x + 53 * y + 59 * Direction;

		public IEnumerable<Particle> Move(char currentCell) => (currentCell, Direction) switch
		{
			('.', 'N') or ('/', 'E') or ('\\', 'W') or ('|', 'N') => [new(Up(), 'N')],
			('.', 'E') or ('/', 'N') or ('\\', 'S') or ('-', 'E') => [new(Right(), 'E')],
			('.', 'S') or ('/', 'W') or ('\\', 'E') or ('|', 'S') => [new(Down(), 'S')],
			('.', 'W') or ('/', 'S') or ('\\', 'N') or ('-', 'W') => [new(Left(), 'W')],
			('|', 'E') or ('|', 'W') => [new(Up(), 'N'), new(Down(), 'S')],
			('-', 'N') or ('-', 'S') => [new(Right(), 'E'), new(Left(), 'W')],
			_ => throw new Exception("unexpected movement"),
		};
	}

	public static int CalculateEnergy(List<string> grid, Particle startingParticle)
	{
		var particles = new List<Particle> { startingParticle };
		var seenParticles = new HashSet<Particle>();

		while (particles.Count > 0)
		{
			seenParticles.UnionWith(particles);
			particles = particles
				.SelectMany(particle => particle.Move(grid[particle.x][particle.y]))
				.Where(particle => particle.Inside(0, grid.Count - 1, 0, grid[0].Length - 1))
				.Where(particle => !seenParticles.Contains(particle))
				.ToList();
		}

		return seenParticles.Select(particle => (particle.x, particle.y)).Distinct().Count();
	}

	public static string Part1(string inputFile) =>
		CalculateEnergy([.. Utilities.GetInput(inputFile)], new(0, 0, 'E')).ToString();

	public static string Part2(string inputFile)
	{
		var grid = Utilities.GetInput(inputFile).ToList();
		var startingParticles = Enumerable.Range(0, grid.Count).SelectMany<int, Particle>(i =>
			[new(grid.Count - 1, i, 'N'), new(i, 0, 'E'), new(0, i, 'S'), new(i, grid.Count - 1, 'W')]
		);
		return startingParticles.Select(p => CalculateEnergy(grid, p)).Max().ToString();
	}
}
