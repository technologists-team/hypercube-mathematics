using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Shapes;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public readonly struct Circle : IEquatable<Circle>
{
    public static readonly Circle NaN = new(Vector2.NaN, float.NaN);
    public static readonly Circle Zero = new(Vector2.Zero, 0);
    public static readonly Circle Unit = new(Vector2.Zero, 1);

    public readonly Vector2 Position;
    public readonly float Radius;

    public float Diameter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 2 * Radius;
    }
    
    public float Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Radius * Radius * HyperMath.PIf;
    }
    
    public float Circumference
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 2 * HyperMath.PIf * Radius;
    }
    
    public Rect2 BoundingBox
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(
            Position.X - Radius,
            Position.Y + Radius,
            Position.X + Radius,
            Position.Y - Radius);
    }
    
    public bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => !float.IsNaN(Position.X) &&
               !float.IsNaN(Position.Y) &&
               !float.IsNaN(Radius);
    }
    
    public Circle(Vector2 position, float radius)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(radius);
        
        Position = position;
        Radius = radius;
    }

    public Circle(Circle circle)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(circle.Radius);
        
        Position = circle.Position;
        Radius = circle.Radius;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out Vector2 position, out float radius)
    {
        position = Position;
        radius = Radius;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Circle WithPosition(Vector2 position)
    {
        return new Circle(position, Radius);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Circle WithRadius(float radius)
    {
        return new Circle(Position, radius);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Circle Translate(Vector2 direction)
    {
        return WithPosition(Position + direction);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Circle Scale(float factor)
    {
        return WithRadius(Radius * factor);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(Vector2 point)
    {
        return (point - Position).LengthSquared <= Radius * Radius;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(Circle other)
    {
        return (other.Position - Position).LengthSquared <= (Radius - other.Radius) * (Radius - other.Radius);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Intersects(Circle other)
    {
        var rad = Radius + other.Radius;
        return (other.Position - Position).LengthSquared <= rad * rad;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 NearestPoint(Vector2 point)
    {
        var dir = point - Position;
        var lenSquared = dir.LengthSquared;
        if (lenSquared <= Radius * Radius) 
            return point;
        
        var len = 1 / float.ReciprocalSqrtEstimate(lenSquared);
        return Position + dir / len * Radius;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"[({Position}), ({Radius})]";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Circle other)
    {
        return Position.Equals(other.Position) &&
               Radius.AboutEquals(other.Radius);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Circle other, float tolerance)
    {
        return Position.Equals(other.Position, tolerance) &&
               Radius.AboutEquals(other.Radius, tolerance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Circle other && Equals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Position, Radius);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Circle a, Circle b)
    {
        return a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Circle a, Circle b)
    {
        return !a.Equals(b);
    }
}