using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using Hypercube.Mathematics.Extensions;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

/// <summary>
/// Represents a vector with three single-precision floating-point values.
/// Optimized with SIMD.
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Vector3 :
    IEquatable<Vector3>,
    IComparable<Vector3>,
    IComparable<float>,
    IEnumerable<float>,
    ISpanFormattable,
    IAdditionOperators<Vector3, Vector3, Vector3>,
    ISubtractionOperators<Vector3, Vector3, Vector3>,
    IMultiplyOperators<Vector3, Vector3, Vector3>,
    IMultiplyOperators<Vector3, float, Vector3>,
    IDivisionOperators<Vector3, Vector3, Vector3>,
    IDivisionOperators<Vector3, float, Vector3>,
    IUnaryPlusOperators<Vector3, Vector3>,
    IUnaryNegationOperators<Vector3, Vector3>,
    IAdditiveIdentity<Vector3, Vector3>,
    IMultiplicativeIdentity<Vector3, Vector3>,
    IEqualityOperators<Vector3, Vector3, bool>,
    IMinMaxValue<Vector3>
{
    #region Constants
    
    /// <summary>
    /// The number of components in the vector.
    /// </summary>
    public const int Dimensionality = 3;
    
    /// <summary>
    /// A vector where all elements are <see cref="float.NaN"/>.
    /// <code>
    /// NaN, NaN
    /// </code>
    /// </summary>
    public static readonly Vector3 NaN = new(float.NaN);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.PositiveInfinity"/>.
    /// <code>
    /// PositiveInfinity, PositiveInfinity
    /// </code>
    /// </summary>
    public static readonly Vector3 PositiveInfinity = new(float.PositiveInfinity);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.NegativeInfinity"/>.
    /// <code>
    /// NegativeInfinity, NegativeInfinity
    /// </code>
    /// </summary>
    public static readonly Vector3 NegativeInfinity = new(float.NegativeInfinity);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.MaxValue"/>.
    /// <code>
    /// MaxValue, MaxValue
    /// </code>
    /// </summary>
    public static readonly Vector3 Max = new(float.MaxValue);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.MinValue"/>.
    /// <code>
    /// MinValue, MinValue
    /// </code>
    /// </summary>
    public static readonly Vector3 Min = new(float.MinValue);
    
    /// <summary>
    /// A vector where all elements are zero.
    /// <code>
    /// 0, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector3 Zero = new(0f);
    
    /// <summary>
    /// A vector where all elements are one.
    /// <code>
    /// 1, 1, 1
    /// </code>
    /// </summary>
    public static readonly Vector3 One = new(1f);
    
    /// <summary>
    /// A vector where only X element is one.
    /// <code>
    /// 1, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector3 UnitX = new(1f, 0f, 0f);
    
    /// <summary>
    /// A vector where only Y element is one.
    /// <code>
    /// 0, 1, 0
    /// </code>
    /// </summary>
    public static readonly Vector3 UnitY = new(0f, 1f, 0f);
    
    /// <summary>
    /// A vector where only Z element is one.
    /// <code>
    /// 0, 0, 1
    /// </code>
    /// </summary>
    public static readonly Vector3 UnitZ = new(0f, 0f, 1f);
    
    #endregion
    
    public static Vector3 AdditiveIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Zero;
    }

    public static Vector3 MultiplicativeIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => One;
    }

    public static Vector3 MinValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Min;
    }

    public static Vector3 MaxValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Max;
    }

    #region Fields
    
    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public readonly float X;

    /// <summary>
    /// The Y component of the vector.
    /// </summary>
    public readonly float Y;
    
    /// <summary>
    /// The Z component of the vector.
    /// </summary>
    public readonly float Z;
    
    #endregion
    
    public Vector3 Absolute
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Abs(this);
    }

    public Vector3 Rounded
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Round(this);
    }

    public Vector3 Floored
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Floor(this);
    }

    public Vector3 Ceiled
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Ceiling(this);
    }

    public float AspectRatio
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X / Y;
    }
    
    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Dot(this, this);
    }

    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => float.Sqrt(LengthSquared);
    }

    public float LengthFast
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 1f / float.ReciprocalSqrtEstimate(LengthSquared);
    }

    public Vector3 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }

    public Vector3 NormalizedFast
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / LengthFast;
    }

    public float Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y + Z;
    }

    public float Production
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y * Z;
    }
    
    public Vector2 Xy
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X, Y);
    }

    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(index);
    }

    #region Constructors

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(double x, double y, double z) : this((float) x, (float) y, (float) z)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(float scalar) : this(scalar, scalar, scalar)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(double scalar) : this((float)scalar)
    {
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(Vector2 vector, float z = 0) : this(vector.X, vector.Y, z)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(Vector3 vector) : this(vector.X, vector.Y, vector.Z)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(Vector128<float> vector)
    {
        this = Unsafe.As<Vector128<float>, Vector3>(ref vector);
    }

    #endregion
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out float x, out float y, out float z)
    {
        x = X;
        y = Y;
        z = Z;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Get(int index)
    {
        Tools.ThrowIfOutOfRange(index, 0, Dimensionality);
        return Unsafe.Add(ref Unsafe.AsRef(in X), index);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithX(float value) => new(value, Y, Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithY(float value) => new(X, value, Z);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithZ(float value) => new(X, Y, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithXy(Vector2 value) => new(value.X, value.Y, Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 WithXy(Vector3 value) => new(value.X, value.Y, Z);

    #region Cast

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color AsColor(float a = 1f) => new(this, a);
    
    /// <summary>
    /// Returns a new array containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float[] AsArray() => [X, Y, Z];
    
    /// <summary>
    /// Returns a new read-only span containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<float> AsSpan() => AsUnsafeSpan();

    /// <summary>
    /// Returns a mutable <see cref="Span{float}"/> pointing directly to the vector memory.
    /// </summary>
    /// <remarks>
    /// <b>WARNING:</b> This bypasses the readonly constraint.
    /// <para>
    /// For safe, read-only access, use <see cref="AsSpan"/> instead.
    /// </para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<float> AsUnsafeSpan() => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in X), Dimensionality);
    
    /// <summary>
    /// Converts this vector to a SIMD Vector128 representation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector128<float> AsVector128() => Unsafe.As<Vector3, Vector128<float>>(ref Unsafe.AsRef(in this));
    
    #endregion
    
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector3 other) =>
        X.Equals(other.X) &&
        Y.Equals(other.Y) &&
        Z.Equals(other.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AboutEquals(Vector3 other, float tolerance = HyperMath.FloatTolerance) =>
        X.AboutEquals(other.X, tolerance) &&
        Y.AboutEquals(other.Y, tolerance) &&
        Z.AboutEquals(other.Z, tolerance);

    public override bool Equals(object? obj) =>
        obj is Vector3 other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(X, Y, Z);

    #endregion

    #region Formatting

    public override string ToString() =>
        ToString(null, CultureInfo.InvariantCulture);

    public string ToString(string? format, IFormatProvider? provider) =>
        $"{X.ToString(format, provider)}, {Y.ToString(format, provider)}, {Z.ToString(format, provider)}";

    public bool TryFormat(Span<char> destination, out int charsWritten,
        ReadOnlySpan<char> format, IFormatProvider? provider) =>
        destination.TryWrite(provider, $"{X}, {Y}, {Z}", out charsWritten);

    #endregion

    #region IEnumerable

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<float> GetEnumerator()
    {
        yield return X;
        yield return Y;
        yield return Z;
    }

    #endregion

    #region Comparison

    public int CompareTo(Vector3 other) =>
        LengthSquared.CompareTo(other.LengthSquared);

    public int CompareTo(float other) =>
        LengthSquared.CompareTo(other * other);

    #endregion

    #region Static Math

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Abs(Vector3 vector) =>
        new(Vector128.Abs(vector.AsVector128()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Round(Vector3 vector) =>
        new(Vector128.Round(vector.AsVector128()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Floor(Vector3 vector) =>
        new(Vector128.Floor(vector.AsVector128()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Ceiling(Vector3 vector) =>
        new(Vector128.Ceiling(vector.AsVector128()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Clamp(Vector3 vector, Vector3 min, Vector3 max) =>
        new(Vector128.Min(Vector128.Max(vector.AsVector128(), min.AsVector128()), max.AsVector128()));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector3 a, Vector3 b) =>
        Vector128.Dot(a.AsVector128(), b.AsVector128());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector3 a, Vector3 b) =>
        (a - b).LengthSquared;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector3 a, Vector3 b) =>
        (a - b).Length;
   
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceFast(Vector3 a, Vector3 b) =>
        (a - b).LengthFast;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        // TODO: SMID?
        return new Vector3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);
    }

    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector3 a, Vector3 b) => a.AboutEquals(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.AsVector128() + b.AsVector128());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 a, Vector2 b) => a + new Vector3(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 a, Vector128<float> b) => new(a.AsVector128() + b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 a, float b) => a + Vector128.Create(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(float a, Vector3 b) => b + a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 a) => a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.AsVector128() - b.AsVector128());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 a, Vector2 b) => a - new Vector3(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 a, Vector128<float> b) => new(a.AsVector128() - b);
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 a, float b) => a - Vector128.Create(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(float a, Vector3 b) => b - a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 a) => new(Vector128.Negate(a.AsVector128()));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 a, Vector3 b) => new(a.AsVector128() * b.AsVector128());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 a, Vector2 b) => a * new Vector3(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 a, float b) => new(a.AsVector128() * b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(float a, Vector3 b) => b * a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 a, Vector3 b) => new(a.AsVector128() / b.AsVector128());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 a, Vector2 b) => a / new Vector3(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 a, float b) => new(a.AsVector128() / b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(float a, Vector3 b) => new(Vector128.Create(a) / b.AsVector128());
    
    #endregion
}
