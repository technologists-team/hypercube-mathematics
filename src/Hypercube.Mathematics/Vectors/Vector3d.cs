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
public readonly struct Vector3d :
    IEquatable<Vector3d>,
    IComparable<Vector3d>,
    IComparable<double>,
    IEnumerable<double>,
    ISpanFormattable,
    IAdditionOperators<Vector3d, Vector3d, Vector3d>,
    ISubtractionOperators<Vector3d, Vector3d, Vector3d>,
    IMultiplyOperators<Vector3d, Vector3d, Vector3d>,
    IMultiplyOperators<Vector3d, double, Vector3d>,
    IDivisionOperators<Vector3d, Vector3d, Vector3d>,
    IDivisionOperators<Vector3d, double, Vector3d>,
    IUnaryPlusOperators<Vector3d, Vector3d>,
    IUnaryNegationOperators<Vector3d, Vector3d>,
    IAdditiveIdentity<Vector3d, Vector3d>,
    IMultiplicativeIdentity<Vector3d, Vector3d>,
    IEqualityOperators<Vector3d, Vector3d, bool>,
    IMinMaxValue<Vector3d>
{
    #region Constants
    
    /// <summary>
    /// The number of components in the vector.
    /// </summary>
    public const int Dimensionality = 3;
    
    /// <summary>
    /// A vector where all elements are <see cref="double.NaN"/>.
    /// <code>
    /// NaN, NaN
    /// </code>
    /// </summary>
    public static readonly Vector3d NaN = new(double.NaN);
    
    /// <summary>
    /// A vector where all elements are <see cref="double.PositiveInfinity"/>.
    /// <code>
    /// PositiveInfinity, PositiveInfinity
    /// </code>
    /// </summary>
    public static readonly Vector3d PositiveInfinity = new(double.PositiveInfinity);
    
    /// <summary>
    /// A vector where all elements are <see cref="double.NegativeInfinity"/>.
    /// <code>
    /// NegativeInfinity, NegativeInfinity
    /// </code>
    /// </summary>
    public static readonly Vector3d NegativeInfinity = new(double.NegativeInfinity);
    
    /// <summary>
    /// A vector where all elements are <see cref="double.MaxValue"/>.
    /// <code>
    /// MaxValue, MaxValue
    /// </code>
    /// </summary>
    public static readonly Vector3d Max = new(double.MaxValue);
    
    /// <summary>
    /// A vector where all elements are <see cref="double.MinValue"/>.
    /// <code>
    /// MinValue, MinValue
    /// </code>
    /// </summary>
    public static readonly Vector3d Min = new(double.MinValue);
    
    /// <summary>
    /// A vector where all elements are zero.
    /// <code>
    /// 0, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector3d Zero = new(0f);
    
    /// <summary>
    /// A vector where all elements are one.
    /// <code>
    /// 1, 1, 1
    /// </code>
    /// </summary>
    public static readonly Vector3d One = new(1f);
    
    /// <summary>
    /// A vector where only X element is one.
    /// <code>
    /// 1, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector3d UnitX = new(1f, 0f, 0f);
    
    /// <summary>
    /// A vector where only Y element is one.
    /// <code>
    /// 0, 1, 0
    /// </code>
    /// </summary>
    public static readonly Vector3d UnitY = new(0f, 1f, 0f);
    
    /// <summary>
    /// A vector where only Z element is one.
    /// <code>
    /// 0, 0, 1
    /// </code>
    /// </summary>
    public static readonly Vector3d UnitZ = new(0f, 0f, 1f);
    
    #endregion
    
    public static Vector3d AdditiveIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Zero;
    }

    public static Vector3d MultiplicativeIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => One;
    }

    public static Vector3d MinValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Min;
    }

    public static Vector3d MaxValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Max;
    }

    #region Fields
    
    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public readonly double X;

    /// <summary>
    /// The Y component of the vector.
    /// </summary>
    public readonly double Y;
    
    /// <summary>
    /// The Z component of the vector.
    /// </summary>
    public readonly double Z;
    
    #endregion
    
    public Vector3d Absolute
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Abs(this);
    }

    public Vector3d Rounded
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Round(this);
    }

    public Vector3d Floored
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Floor(this);
    }

    public Vector3d Ceiled
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Ceiling(this);
    }

    public double AspectRatio
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X / Y;
    }
    
    public double LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Dot(this, this);
    }

    public double Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => double.Sqrt(LengthSquared);
    }

    public double LengthFast
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 1f / double.ReciprocalSqrtEstimate(LengthSquared);
    }

    public Vector3d Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }

    public Vector3d NormalizedFast
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / LengthFast;
    }

    public double Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y + Z;
    }

    public double Production
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y * Z;
    }

    public double this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(index);
    }

    #region Constructors

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3d(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3d(double scalar) : this(scalar, scalar, scalar)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3d(Vector3d vector) : this(vector.X, vector.Y, vector.Z)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3d(Vector128<double> vector)
    {
        this = Unsafe.As<Vector128<double>, Vector3d>(ref vector);
    }

    #endregion
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out double x, out double y, out double z)
    {
        x = X;
        y = Y;
        z = Z;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double Get(int index)
    {
        Tools.ThrowIfOutOfRange(index, 0, Dimensionality);
        return Unsafe.Add(ref Unsafe.AsRef(in X), index);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3d WithX(double value) => new(value, Y, Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3d WithY(double value) => new(X, value, Z);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3d WithZ(double value) => new(X, Y, value);
    
    #region Cast

    /// <summary>
    /// Returns a new array containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double[] AsArray() => [X, Y, Z];
    
    /// <summary>
    /// Returns a new read-only span containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<double> AsSpan() => AsUnsafeSpan();

    /// <summary>
    /// Returns a mutable <see cref="Span{double}"/> pointing directly to the vector memory.
    /// </summary>
    /// <remarks>
    /// <b>WARNING:</b> This bypasses the readonly constraint.
    /// <para>
    /// For safe, read-only access, use <see cref="AsSpan"/> instead.
    /// </para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<double> AsUnsafeSpan() => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in X), Dimensionality);
    
    /// <summary>
    /// Converts this vector to a SIMD Vector128 representation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector128<double> AsVector128() => Unsafe.As<Vector3d, Vector128<double>>(ref Unsafe.AsRef(in this));
    
    #endregion
    
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector3d other) =>
        X.Equals(other.X) &&
        Y.Equals(other.Y) &&
        Z.Equals(other.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AboutEquals(Vector3d other, double tolerance = HyperMath.FloatTolerance) =>
        X.AboutEquals(other.X, tolerance) &&
        Y.AboutEquals(other.Y, tolerance) &&
        Z.AboutEquals(other.Z, tolerance);

    public override bool Equals(object? obj) =>
        obj is Vector3d other && Equals(other);

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

    public IEnumerator<double> GetEnumerator()
    {
        yield return X;
        yield return Y;
        yield return Z;
    }

    #endregion

    #region Comparison

    public int CompareTo(Vector3d other) =>
        LengthSquared.CompareTo(other.LengthSquared);

    public int CompareTo(double other) =>
        LengthSquared.CompareTo(other * other);

    #endregion

    #region Static Math

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d Abs(Vector3d vector) =>
        new(Vector128.Abs(vector.AsVector128()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d Round(Vector3d vector) =>
        new(Vector128.Round(vector.AsVector128()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d Floor(Vector3d vector) =>
        new(Vector128.Floor(vector.AsVector128()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d Ceiling(Vector3d vector) =>
        new(Vector128.Ceiling(vector.AsVector128()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d Clamp(Vector3d vector, Vector3d min, Vector3d max) =>
        new(Vector128.Min(Vector128.Max(vector.AsVector128(), min.AsVector128()), max.AsVector128()));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Dot(Vector3d a, Vector3d b) =>
        Vector128.Dot(a.AsVector128(), b.AsVector128());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double DistanceSquared(Vector3d a, Vector3d b) =>
        (a - b).LengthSquared;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Distance(Vector3d a, Vector3d b) =>
        (a - b).Length;
   
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double DistanceFast(Vector3d a, Vector3d b) =>
        (a - b).LengthFast;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d Cross(Vector3d a, Vector3d b)
    {
        // TODO: SMID?
        return new Vector3d(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);
    }

    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector3d a, Vector3d b) => a.AboutEquals(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector3d a, Vector3d b) => !(a == b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator +(Vector3d a, Vector3d b) => new(a.AsVector128() + b.AsVector128());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator +(Vector3d a, Vector128<double> b) => new(a.AsVector128() + b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator +(Vector3d a, double b) => a + Vector128.Create(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator +(double a, Vector3d b) => b + a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator +(Vector3d a) => a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator -(Vector3d a, Vector3d b) => new(a.AsVector128() - b.AsVector128());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator -(Vector3d a, Vector128<double> b) => new(a.AsVector128() - b);
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator -(Vector3d a, double b) => a - Vector128.Create(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator -(double a, Vector3d b) => b - a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator -(Vector3d a) => new(Vector128.Negate(a.AsVector128()));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator *(Vector3d a, Vector3d b) => new(a.AsVector128() * b.AsVector128());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator *(Vector3d a, double b) => new(a.AsVector128() * b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator *(double a, Vector3d b) => b * a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator /(Vector3d a, Vector3d b) => new(a.AsVector128() / b.AsVector128());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3d operator /(Vector3d a, double b) => new(a.AsVector128() / b);

    #endregion
}
