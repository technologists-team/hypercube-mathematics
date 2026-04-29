using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

/// <summary>
/// Represents a vector with three single-precision floating-point values.
/// Optimized with SIMD.
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Vector3i :
    IEquatable<Vector3i>,
    IComparable<Vector3i>,
    IComparable<int>,
    IEnumerable<int>,
    ISpanFormattable,
    IAdditionOperators<Vector3i, Vector3i, Vector3i>,
    ISubtractionOperators<Vector3i, Vector3i, Vector3i>,
    IMultiplyOperators<Vector3i, Vector3i, Vector3i>,
    IMultiplyOperators<Vector3i, int, Vector3i>,
    IDivisionOperators<Vector3i, Vector3i, Vector3i>,
    IDivisionOperators<Vector3i, int, Vector3i>,
    IUnaryPlusOperators<Vector3i, Vector3i>,
    IUnaryNegationOperators<Vector3i, Vector3i>,
    IAdditiveIdentity<Vector3i, Vector3i>,
    IMultiplicativeIdentity<Vector3i, Vector3i>,
    IEqualityOperators<Vector3i, Vector3i, bool>,
    IMinMaxValue<Vector3i>
{
    #region Constants
    
    /// <summary>
    /// The number of components in the vector.
    /// </summary>
    public const int Dimensionality = 3;
    
    /// <summary>
    /// A vector where all elements are <see cref="int.MaxValue"/>.
    /// <code>
    /// MaxValue, MaxValue
    /// </code>
    /// </summary>
    public static readonly Vector3i Max = new(int.MaxValue);
    
    /// <summary>
    /// A vector where all elements are <see cref="int.MinValue"/>.
    /// <code>
    /// MinValue, MinValue
    /// </code>
    /// </summary>
    public static readonly Vector3i Min = new(int.MinValue);
    
    /// <summary>
    /// A vector where all elements are zero.
    /// <code>
    /// 0, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector3i Zero = new(0);
    
    /// <summary>
    /// A vector where all elements are one.
    /// <code>
    /// 1, 1, 1
    /// </code>
    /// </summary>
    public static readonly Vector3i One = new(1);
    
    /// <summary>
    /// A vector where only X element is one.
    /// <code>
    /// 1, 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector3i UnitX = new(1, 0, 0);
    
    /// <summary>
    /// A vector where only Y element is one.
    /// <code>
    /// 0, 1, 0
    /// </code>
    /// </summary>
    public static readonly Vector3i UnitY = new(0, 1, 0);
    
    /// <summary>
    /// A vector where only Z element is one.
    /// <code>
    /// 0, 0, 1
    /// </code>
    /// </summary>
    public static readonly Vector3i UnitZ = new(0, 0, 1);
    
    #endregion
    
    public static Vector3i AdditiveIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Zero;
    }

    public static Vector3i MultiplicativeIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => One;
    }

    public static Vector3i MinValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Min;
    }

    public static Vector3i MaxValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Max;
    }

    #region Fields
    
    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public readonly int X;

    /// <summary>
    /// The Y component of the vector.
    /// </summary>
    public readonly int Y;
    
    /// <summary>
    /// The Z component of the vector.
    /// </summary>
    public readonly int Z;
    
    #endregion
    
    public Vector3i Absolute
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Abs(this);
    }
    
    public float AspectRatio
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (float) X / Y;
    }
    
    public int LengthSquared
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
    
    public int Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y + Z;
    }

    public int Production
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y * Z;
    }

    public int this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(index);
    }

    #region Constructors

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i(int scalar) : this(scalar, scalar, scalar)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i(Vector3i vector) : this(vector.X, vector.Y, vector.Z)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i(Vector128<int> vector)
    {
        this = Unsafe.As<Vector128<int>, Vector3i>(ref vector);
    }

    #endregion
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out int x, out int y, out int z)
    {
        x = X;
        y = Y;
        z = Z;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Get(int index)
    {
        Tools.ThrowIfOutOfRange(index, 0, Dimensionality - 1);
        return Unsafe.Add(ref Unsafe.AsRef(in X), index);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i WithX(int value) => new(value, Y, Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i WithY(int value) => new(X, value, Z);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3i WithZ(int value) => new(X, Y, value);
    
    #region Cast

    /// <summary>
    /// Returns a new array containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int[] AsArray() => [X, Y, Z];
    
    /// <summary>
    /// Returns a new read-only span containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<int> AsSpan() => AsUnsafeSpan();

    /// <summary>
    /// Returns a mutable <see cref="Span{int}"/> pointing directly to the vector memory.
    /// </summary>
    /// <remarks>
    /// <b>WARNING:</b> This bypasses the readonly constraint.
    /// <para>
    /// For safe, read-only access, use <see cref="AsSpan"/> instead.
    /// </para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<int> AsUnsafeSpan() => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in X), Dimensionality);
    
    /// <summary>
    /// Converts this vector to a SIMD Vector128 representation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector128<int> AsVector128() => Unsafe.As<Vector3i, Vector128<int>>(ref Unsafe.AsRef(in this));
    
    #endregion
    
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector3i other) =>
        X.Equals(other.X) &&
        Y.Equals(other.Y) &&
        Z.Equals(other.Z);
    
    public override bool Equals(object? obj) =>
        obj is Vector3i other && Equals(other);

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

    public IEnumerator<int> GetEnumerator()
    {
        yield return X;
        yield return Y;
        yield return Z;
    }

    #endregion

    #region Comparison

    public int CompareTo(Vector3i other) =>
        LengthSquared.CompareTo(other.LengthSquared);

    public int CompareTo(int other) =>
        LengthSquared.CompareTo(other * other);

    #endregion

    #region Static Math

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i Abs(Vector3i vector) =>
        new(Vector128.Abs(vector.AsVector128()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i Clamp(Vector3i vector, Vector3i min, Vector3i max) =>
        new(Vector128.Min(Vector128.Max(vector.AsVector128(), min.AsVector128()), max.AsVector128()));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Dot(Vector3i a, Vector3i b) =>
        Vector128.Dot(a.AsVector128(), b.AsVector128());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DistanceSquared(Vector3i a, Vector3i b) =>
        (a - b).LengthSquared;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector3i a, Vector3i b) =>
        (a - b).Length;
   
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceFast(Vector3i a, Vector3i b) =>
        (a - b).LengthFast;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i Cross(Vector3i a, Vector3i b)
    {
        // TODO: SMID?
        return new Vector3i(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);
    }

    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector3i a, Vector3i b) => a.Equals(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector3i a, Vector3i b) => !(a == b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator +(Vector3i a, Vector3i b) => new(a.AsVector128() + b.AsVector128());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator +(Vector3i a, Vector128<int> b) => new(a.AsVector128() + b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator +(Vector3i a, int b) => a + Vector128.Create(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator +(int a, Vector3i b) => b + a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator +(Vector3i a) => a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator -(Vector3i a, Vector3i b) => new(a.AsVector128() - b.AsVector128());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator -(Vector3i a, Vector128<int> b) => new(a.AsVector128() - b);
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator -(Vector3i a, int b) => a - Vector128.Create(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator -(int a, Vector3i b) => b - a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator -(Vector3i a) => new(Vector128.Negate(a.AsVector128()));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator *(Vector3i a, Vector3i b) => new(a.AsVector128() * b.AsVector128());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator *(Vector3i a, int b) => new(a.AsVector128() * b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator *(int a, Vector3i b) => b * a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator /(Vector3i a, Vector3i b) => new(a.AsVector128() / b.AsVector128());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3i operator /(Vector3i a, int b) => new(a.AsVector128() / b);

    #endregion
}
