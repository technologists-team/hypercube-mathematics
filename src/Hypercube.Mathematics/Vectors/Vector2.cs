﻿using System.Collections;
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
public readonly partial struct Vector2 : IEquatable<Vector2>, IComparable<Vector2>, IComparable<float>, IEnumerable<float>, ISpanFormattable
{
    /// <value>
    /// Vector (float.NaN, float.NaN).
    /// </value>
    public static readonly Vector2 NaN = new(float.NaN);
    
    /// <value>
    /// Vector (float.PositiveInfinity, float.PositiveInfinity).
    /// </value>
    public static readonly Vector2 PositiveInfinity = new(float.PositiveInfinity);
    
    /// <value>
    /// Vector (float.NegativeInfinity, float.NegativeInfinity).
    /// </value>
    public static readonly Vector2 NegativeInfinity = new(float.NegativeInfinity);
    
    /// <value>
    /// Vector (0, 0).
    /// </value>
    public static readonly Vector2 Zero = new(0);
    
    /// <value>
    /// Vector (1, 1).
    /// </value>
    public static readonly Vector2 One = new(1);
    
    /// <value>
    /// Vector (1, 0).
    /// </value>
    public static readonly Vector2 UnitX = new(1, 0);
    
    /// <value>
    /// Vector (0, 1).
    /// </value>
    public static readonly Vector2 UnitY = new(0, 1);
    
    /// <summary>
    /// Vector X component.
    /// </summary>
    public readonly float X;
    
    /// <summary>
    /// Vector Y component.
    /// </summary>
    public readonly float Y;

    /// <summary>
    /// Returns the ratio of X to Y.
    /// </summary>
    public float AspectRatio
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
    public float LengthSquared
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
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 1f / MathF.ReciprocalSqrtEstimate(LengthSquared);
    }
    
    /// <summary>
    /// Copy of scaled to unit length.
    /// </summary>
    public Vector2 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }

    public float Angle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MathF.Atan2(Y, X);
    }

    /// <summary>
    /// Summation of all vector components.
    /// </summary>
    public float Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y;
    }

    /// <summary>
    /// Production of all vector components.
    /// </summary>
    public float Production 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y;
    }
    
    public float this[int index]
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

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    public Vector2(float value)
    {
        X = value;
        Y = value;
    }
    
    public Vector2(double x, double y)
    {
        X = (float) x;
        Y = (float) y;
    }
    
    public Vector2(double value)
    {
        X = (float) value;
        Y = (float) value;
    }

    public Vector2(Vector2 vector2)
    {
        X = vector2.X;
        Y = vector2.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 WithX(float value)
    {
        return new Vector2(value, Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 WithY(float value)
    {
        return new Vector2(X, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float DistanceSquared(Vector2 value)
    {
        return DistanceSquared(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Distance(Vector2 value)
    {
        return Distance(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Dot(Vector2 value)
    {
        return Dot(this, value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Reflect(Vector2 normal)
    {
        return Reflect(this, normal);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 MoveTowards(Vector2 target, float distance)
    {
        return MoveTowards(this, target, distance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Rotate(float angle)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);
        return new Vector2(
            cos * X - sin * Y,
            sin * X + cos * Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Clamp(Vector2 min, Vector2 max)
    {
        return Clamp(this, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Clamp(float min, float max)
    {
        return Clamp(this, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Lerp(Vector2 value, float amount)
    {
        return Lerp(this, value, amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Max(Vector2 value)
    {
        return Max(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Min(Vector2 value)
    {
        return Min(this, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Abs()
    {
        return Abs(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Round()
    {
        return Round(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Round(int digits)
    {
        return Round(this, digits);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Ceiling()
    {
        return Ceiling(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Floor()
    {
        return Floor(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector2 other)
    {
        return LengthSquared.CompareTo(other.LengthSquared);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(float other)
    {
        return LengthSquared.CompareTo(other * other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<float> GetEnumerator()
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
    public bool Equals(Vector2 other)
    {
        return X.AboutEquals(other.X) &&
               Y.AboutEquals(other.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Vector2 other && Equals(other);
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
    public static Vector2 operator +(Vector2 a, Vector2 valueB)
    {
        return new Vector2(a.X + valueB.X, a.Y + valueB.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(float a, Vector2 valueB)
    {
        return new Vector2(valueB.X + a, valueB.Y + a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 a, float valueB)
    {
        return new Vector2(a.X + valueB, a.Y + valueB);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 a)
    {
        return new Vector2(-a.X, -a.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 a, Vector2 valueB)
    {
        return new Vector2(a.X - valueB.X, a.Y - valueB.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(float a, Vector2 valueB)
    {
        return new Vector2(valueB.X - a, valueB.Y - a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 a, float valueB)
    {
        return new Vector2(a.X - valueB, a.Y - valueB);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 a, Vector2 valueB)
    {
        return new Vector2(a.X * valueB.X, a.Y * valueB.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(float a, Vector2 valueB)
    {
        return new Vector2(valueB.X * a, valueB.Y * a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 a, float valueB)
    {
        return new Vector2(a.X * valueB, a.Y * valueB);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 a, Vector2 valueB)
    {
        return new Vector2(a.X / valueB.X, a.Y / valueB.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(float a, Vector2 valueB)
    {
        return new Vector2(valueB.X / a, valueB.Y / a);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 a, float valueB)
    {
        return new Vector2(a.X / valueB, a.Y / valueB);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2 a, Vector2 valueB)
    {
        return a.Equals(valueB);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2 a, Vector2 valueB)
    {
        return !a.Equals(valueB);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Vector2 valueA, Vector2 valueB)
    {
        return valueA.CompareTo(valueB) == -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Vector2 valueA, Vector2 valueB)
    {
        return valueA.CompareTo(valueB) == 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Vector2 valueA, Vector2 valueB)
    {
        return valueA.CompareTo(valueB) is -1 or 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Vector2 valueA, Vector2 valueB)
    {
        return valueA.CompareTo(valueB) is 1 or 0;
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Vector2 valueA, int valueB)
    {
        return valueA.CompareTo(valueB) == -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Vector2 valueA, int valueB)
    {
        return valueA.CompareTo(valueB) == 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Vector2 valueA, int valueB)
    {
        return valueA.CompareTo(valueB) is -1 or 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Vector2 valueA, int valueB)
    {
        return valueA.CompareTo(valueB) is 1 or 0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector2 valueA, Vector2 valueB)
    {
        return (valueA - valueB).LengthSquared;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector2 valueA, Vector2 valueB)
    {
        return (valueA - valueB).Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector2 valueA, Vector2 valueB)
    {
        return valueA.X * valueB.X + valueA.Y * valueB.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Reflect(Vector2 value, Vector2 normal)
    {
        return value - 2.0f * (Dot(value, normal) * normal);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 MoveTowards(Vector2 current, Vector2 target, float distance)
    {
        return new Vector2(
            HyperMath.MoveTowards(current.X, target.X, distance),
            HyperMath.MoveTowards(current.Y, target.Y, distance));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Lerp(Vector2 vectorA, Vector2 vectorB, float amount)
    {
        return new Vector2(
            float.Lerp(vectorA.X, vectorB.X, amount),
            float.Lerp(vectorA.Y, vectorB.Y, amount));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
    {
        return new Vector2(
            float.Clamp(value.X, min.X, max.X),
            float.Clamp(value.Y, min.Y, max.Y));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Clamp(Vector2 value, float min, float max)
    {
        return new Vector2(
            float.Clamp(value.X, min, max),
            float.Clamp(value.Y, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Max(Vector2 valueA, Vector2 valueB)
    {
        return new Vector2(
            MathF.Max(valueA.X, valueB.X),
            MathF.Max(valueA.Y, valueB.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Min(Vector2 valueA, Vector2 valueB)
    {
        return new Vector2(
            MathF.Min(valueA.X, valueB.X),
            MathF.Min(valueA.Y, valueB.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Abs(Vector2 value)
    {
        return new Vector2(
            Math.Abs(value.X),
            Math.Abs(value.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Round(Vector2 value)
    {
        return new Vector2(
            Math.Round(value.X),
            Math.Round(value.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Round(Vector2 value, int digits)
    {
        return new Vector2(
            Math.Round(value.X, digits),
            Math.Round(value.Y, digits));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Ceiling(Vector2 value)
    {
        return new Vector2(
            Math.Ceiling(value.X),
            Math.Ceiling(value.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Floor(Vector2 value)
    {
        return new Vector2(
            Math.Floor(value.X),
            Math.Floor(value.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Sign(Vector2 value)
    {
        return new Vector2(
            Math.Sign(value.X),
            Math.Sign(value.Y));
    }
}