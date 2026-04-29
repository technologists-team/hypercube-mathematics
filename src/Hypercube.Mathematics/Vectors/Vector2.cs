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
/// Represents a vector with two single-precision floating-point values.
/// Optimized with SIMD.
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Vector2 :
    IEquatable<Vector2>,
    IComparable<Vector2>,
    IComparable<float>,
    IEnumerable<float>,
    ISpanFormattable, 
    IAdditionOperators<Vector2, Vector2, Vector2>,
    ISubtractionOperators<Vector2, Vector2, Vector2>,
    IMultiplyOperators<Vector2, Vector2, Vector2>,
    IMultiplyOperators<Vector2, float, Vector2>,
    IDivisionOperators<Vector2, Vector2, Vector2>,
    IDivisionOperators<Vector2, float, Vector2>,
    IUnaryPlusOperators<Vector2, Vector2>,
    IUnaryNegationOperators<Vector2, Vector2>,
    IAdditiveIdentity<Vector2, Vector2>,
    IMultiplicativeIdentity<Vector2, Vector2>,
    IEqualityOperators<Vector2, Vector2, bool>,
    IMinMaxValue<Vector2>
{
    #region Constants
    
    /// <summary>
    /// The number of components in the vector.
    /// </summary>
    public const int Dimensionality = 2;
    
    /// <summary>
    /// A vector where all elements are <see cref="float.NaN"/>.
    /// <code>
    /// NaN, NaN
    /// </code>
    /// </summary>
    public static readonly Vector2 NaN = new(float.NaN);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.PositiveInfinity"/>.
    /// <code>
    /// PositiveInfinity, PositiveInfinity
    /// </code>
    /// </summary>
    public static readonly Vector2 PositiveInfinity = new(float.PositiveInfinity);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.NegativeInfinity"/>.
    /// <code>
    /// NegativeInfinity, NegativeInfinity
    /// </code>
    /// </summary>
    public static readonly Vector2 NegativeInfinity = new(float.NegativeInfinity);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.MaxValue"/>.
    /// <code>
    /// MaxValue, MaxValue
    /// </code>
    /// </summary>
    public static readonly Vector2 Max = new(float.MaxValue);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.MinValue"/>.
    /// <code>
    /// MinValue, MinValue
    /// </code>
    /// </summary>
    public static readonly Vector2 Min = new(float.MinValue);
    
    /// <summary>
    /// A vector where all elements are zero.
    /// <code>
    /// 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector2 Zero = new(0f);
    
    /// <summary>
    /// A vector where all elements are one.
    /// <code>
    /// 1, 1
    /// </code>
    /// </summary>
    public static readonly Vector2 One = new(1f);
    
    /// <summary>
    /// A vector where only X element is one.
    /// <code>
    /// 1, 0
    /// </code>
    /// </summary>
    public static readonly Vector2 UnitX = new(1f, 0f);
    
    /// <summary>
    /// A vector where only Y element is one.
    /// <code>
    /// 0, 1
    /// </code>
    /// </summary>
    public static readonly Vector2 UnitY = new(0f, 1f);
    
    #endregion
    
    public static Vector2 AdditiveIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Zero;
    }

    public static Vector2 MultiplicativeIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => One;
    }

    public static Vector2 MinValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Min;
    }

    public static Vector2 MaxValue
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
    
    #endregion

    public Vector2 Absolute
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Abs(this);
    }

    public Vector2 Rounded
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Round(this);
    }

    public Vector2 Floored
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Floor(this);
    }

    public Vector2 Ceiled
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

    public Vector2 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }
    
    public (float, Vector2) LengthNormalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var length = Length;
            return (length, this / length);
        }
    }
    
    public Vector2 NormalizedFast
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / LengthFast;
    }

    public float Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y;
    }

    public float Production
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y;
    }

    public float Angle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => float.Atan2(Y, X);
    }

    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(index);
    }
    
    #region Constructors
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2(double x, double y) : this((float) x, (float) y)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2(float scalar) : this(scalar, scalar)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2(double scalar) : this((float) scalar)
    {
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2(Vector2 vector) : this(vector.X, vector.Y)
    {
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2(Vector2d vector) : this(vector.X, vector.Y)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2(Vector64<float> vector)
    {
        this = Unsafe.As<Vector64<float>, Vector2>(ref vector);
    }
    
    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out float x, out float y)
    {
        x = X;
        y = Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Get(int index)
    {
        Tools.ThrowIfOutOfRange(index, 0, Dimensionality - 1);
        return Unsafe.Add(ref Unsafe.AsRef(in X), index);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 WithX(float value) => new(value, Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 WithY(float value) => new(X, value);

    #region Cast
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Angle AsAngle() => new(Angle);

    /// <summary>
    /// Returns a new array containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float[] AsArray() => [X, Y];
    
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
    /// Converts this vector to a SIMD Vector64 representation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector64<float> AsVector64() => Unsafe.As<Vector2, Vector64<float>>(ref Unsafe.AsRef(in this));
    
    #endregion

    #region Transformation

    /// <summary>
    /// Rotate vector CCW.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Rotate(float angle)
        => Rotate(float.Cos(angle), float.Sin(angle));

    /// <summary>
    /// Rotate vector CCW.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Rotate(Angle angle)
        => Rotate((float) angle.Cos, (float) angle.Sin);

    /// <summary>
    /// Rotate vector CCW.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Rotate(Vector2Angle vector)
        => Rotate(vector.Cos, vector.Sin);
    
    /// <summary>
    /// Rotate vector CCW.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Rotate(float cos, float sin)
    {
        // [ cos; -sin ]
        // [ sin;  cos ]
        return new Vector2(X * cos - Y * sin, X * sin + Y * cos);
    }

    /// <summary>
    /// Rotate vector CW.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 InvRotate(float angle)
        => InvRotate(float.Cos(angle), float.Sin(angle));

    /// <summary>
    /// Rotate vector CW.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 InvRotate(Angle angle)
        => InvRotate((float) angle.Cos, (float) angle.Sin);

    /// <summary>
    /// Rotate vector CW.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 InvRotate(Vector2Angle vector)
        => InvRotate(vector.Cos, vector.Sin);
    
    /// <summary>
    /// Rotate vector CW.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 InvRotate(float cos, float sin)
    {
        // [  cos; sin ]
        // [ -sin; cos ]
        return new Vector2(X * cos + Y * sin, Y * cos - X * sin);
    }

    #endregion
    
    #region Equality
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector2 other) =>
        X.Equals(other.X) &&
        Y.Equals(other.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AboutEquals(Vector2 other, float tolerance = HyperMath.FloatTolerance) =>
        X.AboutEquals(other.X, tolerance) &&
        Y.AboutEquals(other.Y, tolerance);

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Vector2 other &&
        Equals(other);
    
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() =>
        HashCode.Combine(X, Y);

    #endregion

    #region String Formating
    
    /// <inheritdoc/>
    public override string ToString() =>
        ToString(null, CultureInfo.InvariantCulture);
    
    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider) =>
        $"{X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)}";
    
    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) => destination.TryWrite(provider, $"{X}, {Y}", out charsWritten);
    
    #endregion

    #region IEnumerable
    
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<float> GetEnumerator()
    {
        yield return X;
        yield return Y;
    }
    
    #endregion

    #region Comparation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector2 other) => LengthSquared.CompareTo(other.LengthSquared);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(float other) => LengthSquared.CompareTo(other * other);

    #endregion

    #region Math

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 Clamp(Vector2 min, Vector2 max) => Clamp(this, min, max);

    #endregion
    
    #region Static Math

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Abs(Vector2 vector) =>
        new(Vector64.Abs(vector.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Round(Vector2 vector) =>
        new(Vector64.Round(vector.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Floor(Vector2 vector) =>
        new(Vector64.Floor(vector.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Ceiling(Vector2 vector) =>
        new(Vector64.Ceiling(vector.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Clamp(Vector2 vector, Vector2 min, Vector2 max) =>
        new(Vector64.Min(Vector64.Max(vector.AsVector64(), min.AsVector64()), max.AsVector64()));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector2 a, Vector2 b) =>
        Vector64.Dot(a.AsVector64(), b.AsVector64());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector2 a, Vector2 b) =>
        (a - b).LengthSquared;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector2 a, Vector2 b) =>
        (a - b).Length;
   
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceFast(Vector2 a, Vector2 b) =>
        (a - b).LengthFast;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Reflect(Vector2 v, Vector2 n) =>
        v - 2f * Dot(v, n) * n;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        => new(Vector64.Lerp(a.AsVector64(), b.AsVector64(), Vector64.Create(t)));
    
    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2 a, Vector2 b) => a.AboutEquals(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.AsVector64() + b.AsVector64());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 a, Vector64<float> b) => new(a.AsVector64() + b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 a, float b) => a + Vector64.Create(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(float a, Vector2 b) => b + a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 a) => a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.AsVector64() - b.AsVector64());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 a, Vector64<float> b) => new(a.AsVector64() - b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 a, float b) => a - Vector64.Create(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(float a, Vector2 b) => b - a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 a) => new(Vector64.Negate(a.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.AsVector64() * b.AsVector64());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 a, float b) => new(a.AsVector64() * b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(float a, Vector2 b) => b * a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.AsVector64() / b.AsVector64());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 a, float b) => new(a.AsVector64() / b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2((float x, float y) a) => new(a.x, a.y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (float x, float y)(Vector2 a) => (a.X, a.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector2i(Vector2 a) => new((int) a.X, (int) a.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector2d(Vector2 a) => new(a.X, a.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector2Angle(Vector2 a) => new(a.X, a.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector3(Vector2 a) => new(a);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector3i(Vector2 a) => new((int) a.X, (int) a.Y, 0);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector3d(Vector2 a) => new(a.X, a.Y, 0);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector4(Vector2 a) => new(a);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector5(Vector2 a) => new(a);
    
    #endregion
}
