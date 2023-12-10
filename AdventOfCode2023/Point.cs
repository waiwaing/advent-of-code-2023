namespace AdventOfCode2023;

public class Point(int x, int y) : IEquatable<Point>
{
	public int x = x, y = y;

	public Point Up() => new(x - 1, y);
	public Point Down() => new(x + 1, y);
	public Point Left() => new(x, y - 1);
	public Point Right() => new(x, y + 1);

	public static bool operator ==(Point p1, Point p2) => p1.Equals(p2);
	public static bool operator !=(Point p1, Point p2) => !p1.Equals(p2);

	public bool Equals(Point? other) => (other is not null) && (x == other.x) && (y == other.y);

	public override bool Equals(object? obj)
	{
		if (obj == null || obj is not Point) return false;
		return Equals(obj as Point);
	}

	public override int GetHashCode() => 31 * x + 53 * y;

	public bool Inside(int minX, int maxX, int minY, int maxY) => x >= minX && x <= maxX && y >= minY && y <= maxY;
	public bool Inside(Point upperLeft, Point lowerRight) => Inside(upperLeft.x, lowerRight.x, upperLeft.y, lowerRight.y);

	public IEnumerable<Point> AdjacentPoints() => [
		new(x - 1, y - 1), new(x - 1, y), new(x - 1, y + 1),
		new(x, y - 1), new(x, y + 1),
		new(x + 1, y - 1), new(x + 1, y), new(x + 1, y + 1)
	];

	public (Point start, Point end) ContiguousOnLine(Predicate<Point> predicate)
	{
		var startOfLine = this;
		var endOfLine = this;

		while (predicate(startOfLine.Left())) { startOfLine = startOfLine.Left(); }
		while (predicate(endOfLine.Right())) { endOfLine = endOfLine.Right(); }

		return (startOfLine, endOfLine);
	}
}