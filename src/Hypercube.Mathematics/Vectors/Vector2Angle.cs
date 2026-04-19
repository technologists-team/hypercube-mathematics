using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

/// <summary>
/// Designed specifically to optimize mathematical operations in physics simulations.
/// It is similar to <see cref="Vector2"/>, as it stores two floats and can be substituted for it.
/// Created to facilitate development, it features explicit fields for trigonometric function values, a
/// well as methods for convenient calculations.
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Vector2Angle
{
    public static readonly Vector2Angle Zero = new(1, 0);

    /// <summary>
    /// The number of components in the vector.
    /// </summary>
    public const int Dimensionality = 2;

    /// <summary>
    /// The cos [X] component.
    /// </summary>
    public readonly float Cos;
    
    /// <summary>
    /// The sin [Y] component.
    /// </summary>
    public readonly float Sin;

    public float Angle => float.Atan2(Sin, Cos);

    public Vector2Angle(float angle)
    {
        Cos = float.Cos(angle);
        Sin = float.Sin(angle);
    }
    
    public Vector2Angle(double angle)
    {
        Cos = (float) double.Cos(angle);
        Sin = (float) double.Sin(angle);
    }
    
    public Vector2Angle(float cos, float sin)
    {
        Cos = cos;
        Sin = sin;
    }
    
    public Vector2Angle(double cos, double sin)
    {
        Cos = (float) cos;
        Sin = (float) sin;
    }
    
    public Vector2Angle(Vector2Angle vector)
    {
        Cos = vector.Cos;
        Sin = vector.Sin;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out float cos, out float sin)
    {
        cos = Cos;
        sin = Sin;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Angle WithCos(float value) => new(value, Sin);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Angle WithSin(float value) => new(Cos, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float[] AsArray() => [Cos, Sin];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<float> AsSpan() => AsUnsafeSpan();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<float> AsUnsafeSpan() => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in Cos), Dimensionality);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Angle AsAngle() => new(Angle);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Angle AddAngle(Vector2Angle other)
    {
        // cos(a + b)
        // sin(a + b)
        return new Vector2Angle(
            Cos * other.Cos - Sin * other.Sin,
            Sin * other.Cos + Cos * other.Sin
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Angle SubtractAngle(Vector2Angle other)
    {
        // cos(a - b)
        // sin(a - b)
        return new Vector2Angle(
            Cos * other.Cos + Sin * other.Sin,
            Sin * other.Cos - Cos * other.Sin
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Angle(Vector2Angle vector) => vector.AsAngle();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector2(Vector2Angle vector) => new(vector.Cos, vector.Sin);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2Angle((float cos, float sin) tuple) => new(tuple.cos, tuple.sin);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (float cos, float sin)(Vector2Angle vector) => (vector.Cos, vector.Sin);
}
