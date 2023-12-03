namespace AdventOfCode2023;

public class Point(int x, int y)
{
	public int x = x, y = y;

	public Point Up() => new(x - 1, y);
	public Point Down() => new(x + 1, y);
	public Point Left() => new(x, y - 1);
	public Point Right() => new(x, y + 1);


	public override bool Equals(object? obj)
	{
		if (obj == null || GetType() != obj.GetType()) return false;
		return x == (obj as Point)!.x && y == (obj as Point)!.y;
	}

	public override int GetHashCode() => 3 * x + 5 * y;

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