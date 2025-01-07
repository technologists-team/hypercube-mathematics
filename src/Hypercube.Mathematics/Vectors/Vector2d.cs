using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

/// <summary>
/// Represents a vector with two single-precision floating-point values.
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential), DebuggerDisplay("({X}, {Y})")]
public readonly partial struct Vector2d : IEquatable<Vector2d>, IComparable<Vector2d>, IComparable<double>, IEnumerable<double>, ISpanFormattable
{
    /// <value>
    /// Vector (double.NaN, double.NaN).
    /// </value>
    public static readonly Vector2d NaN = new(double.NaN);
    
    /// <value>
    /// Vector (double.PositiveInfinity, double.PositiveInfinity).
    /// </value>
    public static readonly Vector2d PositiveInfinity = new(double.PositiveInfinity);
    
    /// <value>
    /// Vector (double.NegativeInfinity, double.NegativeInfinity).
    /// </value>
    public static readonly Vector2d NegativeInfinity = new(double.NegativeInfinity);
    
    /// <value>
    /// Vector (0, 0).
    /// </value>
    public static readonly Vector2d Zero = new(0);
    
    /// <value>
    /// Vector (1, 1).
    /// </value>
    public static readonly Vector2d One = new(1);
    
    /// <value>
    /// Vector (1, 0).
    /// </value>
    public static readonly Vector2d UnitX = new(1, 0);
    
    /// <value>
    /// Vector (0, 1).
    /// </value>
    public static readonly Vector2d UnitY = new(0, 1);
    
    /// <summary>
    /// Vector X component.
    /// </summary>
    public readonly double X;
    
    /// <summary>
    /// Vector Y component.
    /// </summary>
    public readonly double Y;

    /// <summary>
    /// Returns the ratio of X to Y.
    /// </summary>
    public double AspectRatio
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X / Y;
    }
    
    /// <summary>
    /// Gets the square of the vector length (magnitude).
    /// </summary>
    /// <remarks>
    /// Allows you to avoid using the rather expensive sqrt operation.
    /// (On ARM64 hardware <see cref="Length"/> may use the FRSQRTE instruction, which would take away this advantage).
    /// </remarks>
    /// <seealso cref="Length"/>
    public double LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * X + Y * Y;
    }
    
    /// <summary>
    /// Gets the length (magnitude) of the vector.
    /// </summary>
    /// <remarks>
    /// On ARM64 hardware this may use the FRSQRTE instruction
    /// which performs a single Newton-Raphson iteration.
    /// On hardware without specialized support
    /// sqrt is used, which makes the method less fast.
    /// </remarks>
    /// <seealso cref="LengthSquared"/>
    public double Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 1f / Math.ReciprocalSqrtEstimate(LengthSquared);
    }
    
    /// <summary>
    /// Copy of scaled to unit length.
    /// </summary>
    public Vector2d Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }

    public double Angle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Math.Atan2(Y, X);
    }

    /// <summary>
    /// Summation of all vector components.
    /// </summary>
    public double Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y;
    }

    /// <summary>
    /// Production of all vector components.
    /// </summary>
    public double Production 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y;
    }
    
    public double this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return index switch
            {
                0 => X,
                1 => Y,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public Vector2d(double x, double y)
    {
        X = x;
        Y = y;
    }
    
    public Vector2d(double value)
    {
        X = value;
        Y = value;
    }
    
    public Vector2d(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public Vector2d(int value)
    {
        X = value;
        Y = value;
    }

    public Vector2d(Vector2d vector2)
    {
        X = vector2.X;
        Y = vector2.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d WithX(double value)
    {
        return new Vector2d(value, Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d WithY(double value)
    {
        return new Vector2d(X, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double DistanceSquared(Vector2d value)
    {
        return DistanceSquared(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double Distance(Vector2d value)
    {
        return Distance(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double Dot(Vector2d value)
    {
        return Dot(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Reflect(Vector2d normal)
    {
        return Reflect(this, normal);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d MoveTowards(Vector2d target, double distance)
    {
        return MoveTowards(this, target, distance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Rotate(double angle)
    {
        var cos = Math.Cos(angle);
        var sin = Math.Sin(angle);
        return new Vector2d(
            cos * X - sin * Y,
            sin * X + cos * Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Clamp(Vector2d min, Vector2d max)
    {
        return Clamp(this, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Clamp(double min, double max)
    {
        return Clamp(this, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Lerp(Vector2d value, double amount)
    {
        return Lerp(this, value, amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Max(Vector2d value)
    {
        return Max(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Min(Vector2d value)
    {
        return Min(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Abs()
    {
        return Abs(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Round()
    {
        return Round(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Round(int digits)
    {
        return Round(this, digits);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Ceiling()
    {
        return Ceiling(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d Floor()
    {
        return Floor(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector2d other)
    {
        return LengthSquared.CompareTo(other.LengthSquared);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(double other)
    {
        return LengthSquared.CompareTo(other * other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<double> GetEnumerator()
    {
        yield return X;
        yield return Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector2d other)
    {
        return X.AboutEquals(other.X) &&
               Y.AboutEquals(other.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Vector2d other && Equals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"{X}, {Y}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return $"{X}, {Y}".ToString(formatProvider);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return destination.TryWrite(provider, $"{X}, {Y}", out charsWritten);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator +(Vector2d a, Vector2d valueB)
    {
        return new Vector2d(a.X + valueB.X, a.Y + valueB.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator +(double a, Vector2d valueB)
    {
        return new Vector2d(valueB.X + a, valueB.Y + a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator +(Vector2d a, double valueB)
    {
        return new Vector2d(a.X + valueB, a.Y + valueB);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator -(Vector2d a)
    {
        return new Vector2d(-a.X, -a.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator -(Vector2d a, Vector2d valueB)
    {
        return new Vector2d(a.X - valueB.X, a.Y - valueB.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator -(double a, Vector2d valueB)
    {
        return new Vector2d(valueB.X - a, valueB.Y - a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator -(Vector2d a, double valueB)
    {
        return new Vector2d(a.X - valueB, a.Y - valueB);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator *(Vector2d a, Vector2d valueB)
    {
        return new Vector2d(a.X * valueB.X, a.Y * valueB.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator *(double a, Vector2d valueB)
    {
        return new Vector2d(valueB.X * a, valueB.Y * a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator *(Vector2d a, double valueB)
    {
        return new Vector2d(a.X * valueB, a.Y * valueB);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator /(Vector2d a, Vector2d valueB)
    {
        return new Vector2d(a.X / valueB.X, a.Y / valueB.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator /(double a, Vector2d valueB)
    {
        return new Vector2d(valueB.X / a, valueB.Y / a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator /(Vector2d a, double valueB)
    {
        return new Vector2d(a.X / valueB, a.Y / valueB);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2d a, Vector2d valueB)
    {
        return a.Equals(valueB);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2d a, Vector2d valueB)
    {
        return !a.Equals(valueB);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Vector2d valueA, Vector2d valueB)
    {
        return valueA.CompareTo(valueB) == -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Vector2d valueA, Vector2d valueB)
    {
        return valueA.CompareTo(valueB) == 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Vector2d valueA, Vector2d valueB)
    {
        return valueA.CompareTo(valueB) is -1 or 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Vector2d valueA, Vector2d valueB)
    {
        return valueA.CompareTo(valueB) is 1 or 0;
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Vector2d valueA, int valueB)
    {
        return valueA.CompareTo(valueB) == -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Vector2d valueA, int valueB)
    {
        return valueA.CompareTo(valueB) == 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Vector2d valueA, int valueB)
    {
        return valueA.CompareTo(valueB) is -1 or 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Vector2d valueA, int valueB)
    {
        return valueA.CompareTo(valueB) is 1 or 0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double DistanceSquared(Vector2d valueA, Vector2d valueB)
    {
        return (valueA - valueB).LengthSquared;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Distance(Vector2d valueA, Vector2d valueB)
    {
        return (valueA - valueB).Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Dot(Vector2d valueA, Vector2d valueB)
    {
        return valueA.X * valueB.X + valueA.Y * valueB.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Reflect(Vector2d value, Vector2d normal)
    {
        return value - 2.0f * (Dot(value, normal) * normal);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d MoveTowards(Vector2d current, Vector2d target, double distance)
    {
        return new Vector2d(
            HyperMath.MoveTowards(current.X, target.X, distance),
            HyperMath.MoveTowards(current.Y, target.Y, distance));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Lerp(Vector2d vectorA, Vector2d vectorB, double amount)
    {
        return new Vector2d(
            double.Lerp(vectorA.X, vectorB.X, amount),
            double.Lerp(vectorA.Y, vectorB.Y, amount));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Clamp(Vector2d value, Vector2d min, Vector2d max)
    {
        return new Vector2d(
            double.Clamp(value.X, min.X, max.X),
            double.Clamp(value.Y, min.Y, max.Y));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Clamp(Vector2d value, double min, double max)
    {
        return new Vector2d(
            double.Clamp(value.X, min, max),
            double.Clamp(value.Y, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Max(Vector2d valueA, Vector2d valueB)
    {
        return new Vector2d(
            Math.Max(valueA.X, valueB.X),
            Math.Max(valueA.Y, valueB.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Min(Vector2d valueA, Vector2d valueB)
    {
        return new Vector2d(
            Math.Min(valueA.X, valueB.X),
            Math.Min(valueA.Y, valueB.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Abs(Vector2d value)
    {
        return new Vector2d(
            Math.Abs(value.X),
            Math.Abs(value.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Round(Vector2d value)
    {
        return new Vector2d(
            Math.Round(value.X),
            Math.Round(value.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Round(Vector2d value, int digits)
    {
        return new Vector2d(
            Math.Round(value.X, digits),
            Math.Round(value.Y, digits));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Ceiling(Vector2d value)
    {
        return new Vector2d(
            Math.Ceiling(value.X),
            Math.Ceiling(value.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Floor(Vector2d value)
    {
        return new Vector2d(
            Math.Floor(value.X),
            Math.Floor(value.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Sign(Vector2d value)
    {
        return new Vector2d(
            Math.Sign(value.X),
            Math.Sign(value.Y));
    }
}