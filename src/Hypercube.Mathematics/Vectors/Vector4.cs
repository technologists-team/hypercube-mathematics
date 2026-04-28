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

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Vector4 :
    IEquatable<Vector4>,
    IComparable<Vector4>,
    IComparable<float>,
    IEnumerable<float>,
    ISpanFormattable,
    IAdditionOperators<Vector4, Vector4, Vector4>,
    ISubtractionOperators<Vector4, Vector4, Vector4>,
    IMultiplyOperators<Vector4, Vector4, Vector4>,
    IMultiplyOperators<Vector4, float, Vector4>,
    IDivisionOperators<Vector4, Vector4, Vector4>,
    IDivisionOperators<Vector4, float, Vector4>,
    IUnaryPlusOperators<Vector4, Vector4>,
    IUnaryNegationOperators<Vector4, Vector4>,
    IAdditiveIdentity<Vector4, Vector4>,
    IMultiplicativeIdentity<Vector4, Vector4>,
    IEqualityOperators<Vector4, Vector4, bool>,
    IMinMaxValue<Vector4>
{
    #region Constants
    
    /// <summary>
    /// The number of components in the vector.
    /// </summary>
    public const int Dimensionality = 4;
    
    /// <summary>
    /// A vector where all elements are <see cref="float.NaN"/>.
    /// <code>
    /// NaN, NaN
    /// </code>
    /// </summary>
    public static readonly Vector4 NaN = new(float.NaN);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.PositiveInfinity"/>.
    /// <code>
    /// PositiveInfinity, PositiveInfinity
    /// </code>
    /// </summary>
    public static readonly Vector4 PositiveInfinity = new(float.PositiveInfinity);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.NegativeInfinity"/>.
    /// <code>
    /// NegativeInfinity, NegativeInfinity
    /// </code>
    /// </summary>
    public static readonly Vector4 NegativeInfinity = new(float.NegativeInfinity);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.MaxValue"/>.
    /// <code>
    /// MaxValue, MaxValue
    /// </code>
    /// </summary>
    public static readonly Vector4 Max = new(float.MaxValue);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.MinValue"/>.
    /// <code>
    /// MinValue, MinValue
    /// </code>
    /// </summary>
    public static readonly Vector4 Min = new(float.MinValue);
    
    /// <summary>
    /// A vector where all elements are zero.
    /// <code>
    /// 0, 0, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector4 Zero = new(0f);
    
    /// <summary>
    /// A vector where all elements are one.
    /// <code>
    /// 1, 1, 1, 1
    /// </code>
    /// </summary>
    public static readonly Vector4 One = new(1f);
    
    /// <summary>
    /// A vector where only X element is one.
    /// <code>
    /// 1, 0, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector4 UnitX = new(1f, 0f, 0f, 0f);
    
    /// <summary>
    /// A vector where only Y element is one.
    /// <code>
    /// 0, 1, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector4 UnitY = new(0f, 1f, 0f, 0f);
    
    /// <summary>
    /// A vector where only Z element is one.
    /// <code>
    /// 0, 0, 1, 0
    /// </code>
    /// </summary>
    public static readonly Vector4 UnitZ = new(0f, 0f, 1f, 0f);
    
    /// <summary>
    /// A vector where only Z element is one.
    /// <code>
    /// 0, 0, 1, 0
    /// </code>
    /// </summary>
    public static readonly Vector4 UnitW = new(0f, 0f, 0f, 1f);    
    
    #endregion
    
    public static Vector4 AdditiveIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Zero;
    }

    public static Vector4 MultiplicativeIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => One;
    }

    public static Vector4 MinValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Min;
    }

    public static Vector4 MaxValue
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
    
    /// <summary>
    /// The W component of the vector.
    /// </summary>
    public readonly float W;
    
    #endregion

    public Vector4 Absolute
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Abs(this);
    }

    public Vector4 Rounded
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Round(this);
    }

    public Vector4 Floored
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Floor(this);
    }

    public Vector4 Ceiled
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Ceiling(this);
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

    public Vector4 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }

    public Vector4 NormalizedFast
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / LengthFast;
    }

    public Vector2 Xy
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X, Y);
    }
    
    public Vector3 Xyz
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X, Y, Z);
    }
    
    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(index);
    }
    
    #region Constructors

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4(float scalar) : this(scalar, scalar, scalar, scalar)
    {
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4(Vector2 vector, float z = 0, float w = 0) : this(vector.X, vector.Y, z, w)
    {
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4(Vector3 vector, float w = 0) : this(vector.X, vector.Y, vector.Z, w)
    {
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4(Vector4 vector) : this(vector.X, vector.Y, vector.Z, vector.W)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4(Vector256<float> vector)
    {
        this = Unsafe.As<Vector256<float>, Vector4>(ref vector);
    }

    #endregion
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out float x, out float y, out float z, out float w)
    {
        x = X;
        y = Y;
        z = Z;
        w = W;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Get(int index)
    {
        Tools.ThrowIfOutOfRange(index, 0, Dimensionality);
        return Unsafe.Add(ref Unsafe.AsRef(in X), index);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithX(float value) => new(value, Y, Z, W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithY(float value) => new(X, value, Z, W);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithZ(float value) => new(X, Y, value, W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 WithW(float value) => new(X, Y, Z, value);
    
    #region Cast
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color AsColor() => new(this);
    
    /// <summary>
    /// Returns a new array containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float[] AsArray() => [X, Y, Z, W];
    
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
    private Vector256<float> AsVector256() => Unsafe.As<Vector4, Vector256<float>>(ref Unsafe.AsRef(in this));
    
    #endregion
    
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector4 other) =>
        X.Equals(other.X) &&
        Y.Equals(other.Y) &&
        Z.Equals(other.Z) &&
        W.Equals(other.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AboutEquals(Vector4 other, float tolerance = HyperMath.FloatTolerance) =>
        X.AboutEquals(other.X, tolerance) &&
        Y.AboutEquals(other.Y, tolerance) &&
        Z.AboutEquals(other.Z, tolerance) &&
        W.AboutEquals(other.W, tolerance);

    public override bool Equals(object? obj) =>
        obj is Vector4 other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(X, Y, Z, W);

    #endregion

    #region Formatting

    public override string ToString() =>
        ToString(null, CultureInfo.InvariantCulture);

    public string ToString(string? format, IFormatProvider? provider) =>
        $"{X.ToString(format, provider)}, {Y.ToString(format, provider)}, {Z.ToString(format, provider)}, {W.ToString(format, provider)}";

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
        destination.TryWrite(provider, $"{X}, {Y}, {Z}, {W}", out charsWritten);

    #endregion

    #region IEnumerable

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<float> GetEnumerator()
    {
        yield return X;
        yield return Y;
        yield return Z;
        yield return W;
    }

    #endregion

    #region Comparison

    public int CompareTo(Vector4 other) =>
        LengthSquared.CompareTo(other.LengthSquared);

    public int CompareTo(float other) =>
        LengthSquared.CompareTo(other * other);

    #endregion

    #region Static Math

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Abs(Vector4 vector) =>
        new(Vector256.Abs(vector.AsVector256()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Floor(Vector4 vector) =>
        new(Vector256.Floor(vector.AsVector256()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Ceiling(Vector4 vector) =>
        new(Vector256.Ceiling(vector.AsVector256()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Round(Vector4 vector) =>
        new(Vector256.Round(vector.AsVector256()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Clamp(Vector4 vector, Vector4 min, Vector4 max) =>
        new(Vector256.Min(Vector256.Max(vector.AsVector256(), min.AsVector256()), max.AsVector256()));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector4 a, Vector4 b) =>
        Vector256.Dot(a.AsVector256(), b.AsVector256());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector4 a, Vector4 b) =>
        (a - b).LengthSquared;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector4 a, Vector4 b) =>
        (a - b).Length;
   
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceFast(Vector4 a, Vector4 b) =>
        (a - b).LengthFast;
    
    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector4 a, Vector4 b) => a.AboutEquals(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector4 a, Vector4 b) => !(a == b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 a, Vector4 b) => new(a.AsVector256() + b.AsVector256());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 a, Vector256<float> b) => new(a.AsVector256() + b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 a, float b) => a + Vector256.Create(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(float a, Vector4 b) => b + a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 a) => a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 a, Vector4 b) => new(a.AsVector256() - b.AsVector256());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 a, Vector256<float> b) => new(a.AsVector256() - b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 a, float b) => a - Vector256.Create(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(float a, Vector4 b) => b - a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 a) => new(Vector256.Negate(a.AsVector256()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 a, Vector4 b) => new(a.AsVector256() * b.AsVector256());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 a, float b) => new(a.AsVector256() * b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(float a, Vector4 b) => b * a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 a, Vector4 b) => new(a.AsVector256() / b.AsVector256());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 a, float b) => new(a.AsVector256() / b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4((float x, float y, float z, float w) a) => new(a.x, a.y, a.z, a.w);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (float x, float y, float z, float w)(Vector4 a) => (a.X, a.Y, a.Z, a.W);

    #endregion
}
