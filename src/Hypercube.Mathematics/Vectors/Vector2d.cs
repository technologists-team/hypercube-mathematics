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
/// Represents a vector with two double-precision floating-point values.
/// Optimized with SIMD.
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("${ToString()}")]
public readonly struct Vector2d :
    IEquatable<Vector2d>,
    IComparable<Vector2d>,
    IComparable<double>,
    IEnumerable<double>,
    ISpanFormattable, 
    IAdditionOperators<Vector2d, Vector2d, Vector2d>,
    ISubtractionOperators<Vector2d, Vector2d, Vector2d>,
    IMultiplyOperators<Vector2d, Vector2d, Vector2d>,
    IMultiplyOperators<Vector2d, double, Vector2d>,
    IDivisionOperators<Vector2d, Vector2d, Vector2d>,
    IDivisionOperators<Vector2d, double, Vector2d>,
    IUnaryPlusOperators<Vector2d, Vector2d>,
    IUnaryNegationOperators<Vector2d, Vector2d>,
    IAdditiveIdentity<Vector2d, Vector2d>,
    IMultiplicativeIdentity<Vector2d, Vector2d>,
    IEqualityOperators<Vector2d, Vector2d, bool>,
    IMinMaxValue<Vector2d>
{
    #region Constants
    
    /// <summary>
    /// The number of components in the vector.
    /// </summary>
    public const int Dimensionality = 2;
    
    /// <summary>
    /// A vector where all elements are <see cref="double.NaN"/>.
    /// <code>
    /// NaN, NaN
    /// </code>
    /// </summary>
    public static readonly Vector2d NaN = new(double.NaN);
    
    /// <summary>
    /// A vector where all elements are <see cref="double.PositiveInfinity"/>.
    /// <code>
    /// PositiveInfinity, PositiveInfinity
    /// </code>
    /// </summary>
    public static readonly Vector2d PositiveInfinity = new(double.PositiveInfinity);
    
    /// <summary>
    /// A vector where all elements are <see cref="double.NegativeInfinity"/>.
    /// <code>
    /// NegativeInfinity, NegativeInfinity
    /// </code>
    /// </summary>
    public static readonly Vector2d NegativeInfinity = new(double.NegativeInfinity);
        
    /// <summary>
    /// A vector where all elements are <see cref="double.MaxValue"/>.
    /// <code>
    /// MaxValue, MaxValue
    /// </code>
    /// </summary>
    public static readonly Vector2d Max = new(double.MaxValue);
    
    /// <summary>
    /// A vector where all elements are <see cref="double.MinValue"/>.
    /// <code>
    /// MinValue, MinValue
    /// </code>
    /// </summary>
    public static readonly Vector2d Min = new(double.MinValue);
    
    /// <summary>
    /// A vector where all elements are zero.
    /// <code>
    /// 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector2d Zero = new(0d);
    
    /// <summary>
    /// A vector where all elements are one.
    /// <code>
    /// 1, 1
    /// </code>
    /// </summary>
    public static readonly Vector2d One = new(1d);
    
    /// <summary>
    /// A vector where only X element is one.
    /// <code>
    /// 1, 0
    /// </code>
    /// </summary>
    public static readonly Vector2d UnitX = new(1d, 0d);
    
    /// <summary>
    /// A vector where only Y element is one.
    /// <code>
    /// 0, 1
    /// </code>
    /// </summary>
    public static readonly Vector2d UnitY = new(0d, 1d);

    #endregion
    
    public static Vector2d AdditiveIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Zero;
    }

    public static Vector2d MultiplicativeIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => One;
    }

    public static Vector2d MinValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Min;
    }

    public static Vector2d MaxValue
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
    
    #endregion

    public Vector2d Absolute
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Abs(this);
    }

    public Vector2d Rounded
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Round(this);
    }

    public Vector2d Floored
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Floor(this);
    }

    public Vector2d Ceiled
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

    public Vector2d Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }
    
    public Vector2d NormalizedFast
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / LengthFast;
    }

    public double Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y;
    }

    public double Production
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y;
    }

    public double Angle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => double.Atan2(Y, X);
    }

    public double this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(index);
    }
    
    #region Constructors
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d(double x, double y)
    {
        X = x;
        Y = y;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d(double scalar) : this(scalar, scalar)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d(Vector2d vector) : this(vector.X, vector.Y)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d(Vector64<double> vector)
    {
        this = Unsafe.As<Vector64<double>, Vector2d>(ref vector);
    }
    
    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out double x, out double y)
    {
        x = X;
        y = Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double Get(int index)
    {
        Tools.ThrowIfOutOfRange(index, 0, Dimensionality);
        return Unsafe.Add(ref Unsafe.AsRef(in X), index);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d WithX(double value) => new(value, Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2d WithY(double value) => new(X, value);

    #region Cast
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Angle AsAngle() => new(Angle);

    /// <summary>
    /// Returns a new array containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double[] AsArray() => [X, Y];
    
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
    /// Converts this vector to a SIMD Vector64 representation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector64<double> AsVector64() => Unsafe.As<Vector2d, Vector64<double>>(ref Unsafe.AsRef(in this));
    
    #endregion

    #region Equality
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector2d other) =>
        X.Equals(other.X) &&
        Y.Equals(other.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AboutEquals(Vector2d other, double tolerance = HyperMath.FloatTolerance) =>
        X.AboutEquals(other.X, tolerance) &&
        Y.AboutEquals(other.Y, tolerance);

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Vector2d other &&
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

    public IEnumerator<double> GetEnumerator()
    {
        yield return X;
        yield return Y;
    }
    
    #endregion

    #region Comparation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector2d other) => LengthSquared.CompareTo(other.LengthSquared);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(double other) => LengthSquared.CompareTo(other * other);

    #endregion

    #region Static Math
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Abs(Vector2d vector) =>
        new(Vector64.Abs(vector.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Round(Vector2d vector) =>
        new(Vector64.Round(vector.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Floor(Vector2d vector) =>
        new(Vector64.Floor(vector.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Ceiling(Vector2d vector) =>
        new(Vector64.Ceiling(vector.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Clamp(Vector2d vector, Vector2d min, Vector2d max) =>
        new(Vector64.Min(Vector64.Max(vector.AsVector64(), min.AsVector64()), max.AsVector64()));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Dot(Vector2d a, Vector2d b) =>
        Vector64.Dot(a.AsVector64(), b.AsVector64());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double DistanceSquared(Vector2d a, Vector2d b) =>
        (a - b).LengthSquared;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Distance(Vector2d a, Vector2d b) =>
        (a - b).Length;
   
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double DistanceFast(Vector2d a, Vector2d b) =>
        (a - b).LengthFast;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d Reflect(Vector2d v, Vector2d n) =>
        v - 2f * Dot(v, n) * n;

    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2d a, Vector2d b) => a.AboutEquals(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2d a, Vector2d b) => !(a == b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator +(Vector2d a, Vector2d b) => new(a.AsVector64() + b.AsVector64());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator +(Vector2d a, Vector64<double> b) => new(a.AsVector64() + b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator +(Vector2d a, double b) => a + Vector64.Create(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator +(double a, Vector2d b) => b + a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator +(Vector2d a) => a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator -(Vector2d a, Vector2d b) => new(a.AsVector64() - b.AsVector64());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator -(Vector2d a, Vector64<double> b) => new(a.AsVector64() - b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator -(Vector2d a, double b) => a - Vector64.Create(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator -(double a, Vector2d b) => b - a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator -(Vector2d a) => new(Vector64.Negate(a.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator *(Vector2d a, Vector2d b) => new(a.AsVector64() * b.AsVector64());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator *(Vector2d a, double b) => new(a.AsVector64() * b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator *(double a, Vector2d b) => b * a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator /(Vector2d a, Vector2d b) => new(a.AsVector64() / b.AsVector64());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2d operator /(Vector2d a, double b) => new(a.AsVector64() / b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2d((double x, double y) a) => new(a.x, a.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (double x, double y)(Vector2d a) => (a.X, a.Y);
    
    #endregion
}