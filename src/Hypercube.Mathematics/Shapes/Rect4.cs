using System.Diagnostics;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Shapes;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public readonly struct Rect4 : IEquatable<Rect4>
{
    public readonly Vector2 Point0;
    public readonly Vector2 Point1;
    public readonly Vector2 Point2;
    public readonly Vector2 Point3;

    /// <summary>
    /// Creates rectangle from four points.
    /// </summary>
    public Rect4(Vector2 point0, Vector2 point1, Vector2 point2, Vector2 point3)
    {
        Point0 = point0;
        Point1 = point1;
        Point2 = point2;
        Point3 = point3;
    }

    /// <summary>
    /// Gets the bounding box that contains all four points.
    /// </summary>
    public Rect2 GetBoundingBox()
    {
        var minX = Math.Min(Math.Min(Point0.X, Point1.X), Math.Min(Point2.X, Point3.X));
        var minY = Math.Min(Math.Min(Point0.Y, Point1.Y), Math.Min(Point2.Y, Point3.Y));
        var maxX = Math.Max(Math.Max(Point0.X, Point1.X), Math.Max(Point2.X, Point3.X));
        var maxY = Math.Max(Math.Max(Point0.Y, Point1.Y), Math.Max(Point2.Y, Point3.Y));
        return new Rect2(minX, minY, maxX, maxY);
    }

    /// <summary>
    /// Gets rectangle area (works correctly for convex quadrilaterals)
    /// </summary>
    public float GetArea()
    {
        // Using shoelace formula for quadrilateral
        return 0.5f * Math.Abs(
            Point0.X * Point1.Y + Point1.X * Point2.Y + 
            Point2.X * Point3.Y + Point3.X * Point0.Y -
            Point0.Y * Point1.X - Point1.Y * Point2.X - 
            Point2.Y * Point3.X - Point3.Y * Point0.X
        );
    }

    /// <summary>
    /// Checks if point is inside the rectangle (works for convex quadrilaterals).
    /// </summary>
    public bool Contains(Vector2 point)
    {
        // Using cross product method for convex polygons
        var b1 = Cross(Point0, Point1, point) < 0;
        var b2 = Cross(Point1, Point2, point) < 0;
        var b3 = Cross(Point2, Point3, point) < 0;
        var b4 = Cross(Point3, Point0, point) < 0;
        return b1 == b2 && b2 == b3 && b3 == b4;
    }

    public bool Equals(Rect4 other)
    {
        return Point0.Equals(other.Point0) && 
               Point1.Equals(other.Point1) && 
               Point2.Equals(other.Point2) && 
               Point3.Equals(other.Point3);
    }

    public override bool Equals(object? obj)
    {
        return obj is Rect4 other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Point0.GetHashCode();
            hashCode = (hashCode * 397) ^ Point1.GetHashCode();
            hashCode = (hashCode * 397) ^ Point2.GetHashCode();
            hashCode = (hashCode * 397) ^ Point3.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString()
    {
        return $"[{Point0}, {Point1}, {Point2}, {Point3}]";
    }

    private static float Cross(Vector2 a, Vector2 b, Vector2 p)
    {
        return (b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X);
    }

    /// <summary>
    /// Creates rectangle from min and max points (axis-aligned).
    /// </summary>
    public static Rect4 FromMinMax(Vector2 min, Vector2 max)
    {
        return new Rect4(
            new Vector2(min.X, min.Y),
            new Vector2(max.X, min.Y),
            new Vector2(max.X, max.Y),
            new Vector2(min.X, max.Y)
        );
    }

    /// <summary>
    /// Creates rectangle from center and size.
    /// </summary>
    public static Rect4 FromCenterSize(Vector2 center, Vector2 size)
    {
        var halfSize = size / 2;
        return FromMinMax(center - halfSize, center + halfSize);
    }

    public static bool operator ==(Rect4 left, Rect4 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rect4 left, Rect4 right)
    {
        return !left.Equals(right);
    }
}