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
public readonly struct Vector5 :
    IEquatable<Vector5>,
    IComparable<Vector5>,
    IComparable<float>,
    IEnumerable<float>,
    ISpanFormattable,
    IAdditionOperators<Vector5, Vector5, Vector5>,
    ISubtractionOperators<Vector5, Vector5, Vector5>,
    IMultiplyOperators<Vector5, Vector5, Vector5>,
    IMultiplyOperators<Vector5, float, Vector5>,
    IDivisionOperators<Vector5, Vector5, Vector5>,
    IDivisionOperators<Vector5, float, Vector5>,
    IUnaryPlusOperators<Vector5, Vector5>,
    IUnaryNegationOperators<Vector5, Vector5>,
    IAdditiveIdentity<Vector5, Vector5>,
    IMultiplicativeIdentity<Vector5, Vector5>,
    IEqualityOperators<Vector5, Vector5, bool>,
    IMinMaxValue<Vector5>
{
    #region Constants
    
    /// <summary>
    /// The number of components in the vector.
    /// </summary>
    public const int Dimensionality = 5;
    
    /// <summary>
    /// A vector where all elements are <see cref="float.NaN"/>.
    /// <code>
    /// NaN, NaN
    /// </code>
    /// </summary>
    public static readonly Vector5 NaN = new(float.NaN);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.PositiveInfinity"/>.
    /// <code>
    /// PositiveInfinity, PositiveInfinity
    /// </code>
    /// </summary>
    public static readonly Vector5 PositiveInfinity = new(float.PositiveInfinity);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.NegativeInfinity"/>.
    /// <code>
    /// NegativeInfinity, NegativeInfinity
    /// </code>
    /// </summary>
    public static readonly Vector5 NegativeInfinity = new(float.NegativeInfinity);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.MaxValue"/>.
    /// <code>
    /// MaxValue, MaxValue
    /// </code>
    /// </summary>
    public static readonly Vector5 Max = new(float.MaxValue);
    
    /// <summary>
    /// A vector where all elements are <see cref="float.MinValue"/>.
    /// <code>
    /// MinValue, MinValue
    /// </code>
    /// </summary>
    public static readonly Vector5 Min = new(float.MinValue);
    
    /// <summary>
    /// A vector where all elements are zero.
    /// <code>
    /// 0, 0, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector5 Zero = new(0f);
    
    /// <summary>
    /// A vector where all elements are one.
    /// <code>
    /// 1, 1, 1, 1
    /// </code>
    /// </summary>
    public static readonly Vector5 One = new(1f);
    
    /// <summary>
    /// A vector where only X element is one.
    /// <code>
    /// 1, 0, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector5 UnitX = new(1f, 0f, 0f, 0f, 0f);
    
    /// <summary>
    /// A vector where only Y element is one.
    /// <code>
    /// 0, 1, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector5 UnitY = new(0f, 1f, 0f, 0f, 0f);
    
    /// <summary>
    /// A vector where only Z element is one.
    /// <code>
    /// 0, 0, 1, 0
    /// </code>
    /// </summary>
    public static readonly Vector5 UnitZ = new(0f, 0f, 1f, 0f, 0f);
    
    /// <summary>
    /// A vector where only Z element is one.
    /// <code>
    /// 0, 0, 1, 0
    /// </code>
    /// </summary>
    public static readonly Vector5 UnitW = new(0f, 0f, 0f, 1f, 0f);    
    
    #endregion
    
    public static Vector5 AdditiveIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Zero;
    }

    public static Vector5 MultiplicativeIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => One;
    }

    public static Vector5 MinValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Min;
    }

    public static Vector5 MaxValue
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
    
    /// <summary>
    /// The V component of the vector.
    /// </summary>
    public readonly float V;
    
    #endregion

    public Vector5 Absolute
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Abs(this);
    }

    public Vector5 Rounded
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Round(this);
    }

    public Vector5 Floored
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Floor(this);
    }

    public Vector5 Ceiled
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

    public Vector5 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }

    public Vector5 NormalizedFast
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
    
    public Vector4 Xyzw
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X, Y, Z, W);
    }
    
    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(index);
    }

    #region Constructors

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5(float x, float y, float z, float w, float v)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
        V = v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5(double x, double y, double z, double w, double v) : this((float) x, (float) y, (float) z, (float) w, (float) v)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5(float scalar) : this(scalar, scalar, scalar, scalar, scalar)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5(double scalar) : this((float) scalar)
    {
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5(Vector3 vector, float w = 0, float v = 0) : this(vector.X, vector.Y, vector.Z, w, v)
    {
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5(Vector4 vector, float v = 0) : this(vector.X, vector.Y, vector.Z, vector.W, v)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5(Vector5 vector) : this(vector.X, vector.Y, vector.Z, vector.W, vector.V)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5(Vector512<float> vector)
    {
        this = Unsafe.As<Vector512<float>, Vector5>(ref vector);
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
    public Vector5 WithX(float value) => new(value, Y, Z, W, V);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5 WithY(float value) => new(X, value, Z, W, V);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5 WithZ(float value) => new(X, Y, value, W, V);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5 WithW(float value) => new(X, Y, Z, value, V);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5 WithV(float value) => new(X, Y, Z, W, value);
    
    #region Cast
    
    /// <summary>
    /// Returns a new array containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float[] AsArray() => [X, Y, Z, W, V];
    
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
    private Vector512<float> AsVector512() => Unsafe.As<Vector5, Vector512<float>>(ref Unsafe.AsRef(in this));
    
    #endregion
    
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector5 other) =>
        X.Equals(other.X) &&
        Y.Equals(other.Y) &&
        Z.Equals(other.Z) &&
        W.Equals(other.W) &&
        V.Equals(other.V);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AboutEquals(Vector5 other, float tolerance = HyperMath.FloatTolerance) =>
        X.AboutEquals(other.X, tolerance) &&
        Y.AboutEquals(other.Y, tolerance) &&
        Z.AboutEquals(other.Z, tolerance) &&
        W.AboutEquals(other.W, tolerance) &&
        V.AboutEquals(other.V, tolerance);

    public override bool Equals(object? obj) =>
        obj is Vector5 other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(X, Y, Z, W, V);

    #endregion
    
    #region Formatting

    public override string ToString() =>
        ToString(null, CultureInfo.InvariantCulture);

    public string ToString(string? format, IFormatProvider? provider) =>
        $"{X.ToString(format, provider)}, {Y.ToString(format, provider)}, {Z.ToString(format, provider)}";

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
        destination.TryWrite(provider, $"{X}, {Y}, {Z}, {W}, {V}", out charsWritten);

    #endregion
    
    #region IEnumerable

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<float> GetEnumerator()
    {
        yield return X;
        yield return Y;
        yield return Z;
        yield return W;
        yield return V;
    }

    #endregion

    #region Comparison

    public int CompareTo(Vector5 other) =>
        LengthSquared.CompareTo(other.LengthSquared);

    public int CompareTo(float other) =>
        LengthSquared.CompareTo(other * other);

    #endregion
    
    #region Static Math

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 Abs(Vector5 vector) =>
        new(Vector512.Abs(vector.AsVector512()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 Floor(Vector5 vector) =>
        new(Vector512.Floor(vector.AsVector512()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 Ceiling(Vector5 vector) =>
        new(Vector512.Ceiling(vector.AsVector512()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 Round(Vector5 vector) =>
        new(Vector512.Round(vector.AsVector512()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 Clamp(Vector5 vector, Vector5 min, Vector5 max) =>
        new(Vector512.Min(Vector512.Max(vector.AsVector512(), min.AsVector512()), max.AsVector512()));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector5 a, Vector5 b) =>
        Vector512.Dot(a.AsVector512(), b.AsVector512());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector5 a, Vector5 b) =>
        (a - b).LengthSquared;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector5 a, Vector5 b) =>
        (a - b).Length;
   
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceFast(Vector5 a, Vector5 b) =>
        (a - b).LengthFast;
    
    #endregion
    
    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector5 a, Vector5 b) => a.AboutEquals(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector5 a, Vector5 b) => !(a == b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator +(Vector5 a, Vector5 b) => new(a.AsVector512() + b.AsVector512());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator +(Vector5 a, Vector512<float> b) => new(a.AsVector512() + b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator +(Vector5 a, float b) => a + Vector512.Create(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator +(float a, Vector5 b) => b + a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator +(Vector5 a) => a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator -(Vector5 a, Vector5 b) => new(a.AsVector512() - b.AsVector512());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator -(Vector5 a, Vector512<float> b) => new(a.AsVector512() - b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator -(Vector5 a, float b) => a - Vector512.Create(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator -(float a, Vector5 b) => b - a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator -(Vector5 a) => new(Vector512.Negate(a.AsVector512()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator *(Vector5 a, Vector5 b) => new(a.AsVector512() * b.AsVector512());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator *(Vector5 a, float b) => new(a.AsVector512() * b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator *(float a, Vector5 b) => b * a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator /(Vector5 a, Vector5 b) => new(a.AsVector512() / b.AsVector512());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator /(Vector5 a, float b) => new(a.AsVector512() / b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector5((float x, float y, float z, float w, float v) a) => new(a.x, a.y, a.z, a.w, a.v);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (float x, float y, float z, float w, float v)(Vector5 a) => (a.X, a.Y, a.Z, a.W, a.V);

    #endregion
}
