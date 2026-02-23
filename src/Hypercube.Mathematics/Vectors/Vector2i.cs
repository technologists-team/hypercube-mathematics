using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Vector2i :
    IEquatable<Vector2i>,
    IComparable<Vector2i>,
    IComparable<int>,
    IEnumerable<int>,
    IAdditionOperators<Vector2i, Vector2i, Vector2i>,
    ISubtractionOperators<Vector2i, Vector2i, Vector2i>,
    IMultiplyOperators<Vector2i, Vector2i, Vector2i>,
    IMultiplyOperators<Vector2i, int, Vector2i>,
    IDivisionOperators<Vector2i, Vector2i, Vector2i>,
    IDivisionOperators<Vector2i, int, Vector2i>,
    IUnaryPlusOperators<Vector2i, Vector2i>,
    IUnaryNegationOperators<Vector2i, Vector2i>,
    IAdditiveIdentity<Vector2i, Vector2i>,
    IMultiplicativeIdentity<Vector2i, Vector2i>,
    IEqualityOperators<Vector2i, Vector2i, bool>,
    IMinMaxValue<Vector2i>
{
    #region Constants
    
    /// <summary>
    /// The number of components in the vector.
    /// </summary>
    public const int Dimensionality = 2;
        
    /// <summary>
    /// A vector where all elements are zero.
    /// <code>
    /// 0, 0
    /// </code>
    /// </summary>
    public static readonly Vector2i Zero = new(0);
    
    /// <summary>
    /// A vector where all elements are one.
    /// <code>
    /// 1, 1
    /// </code>
    /// </summary>
    public static readonly Vector2i One = new(1);
    
    /// <summary>
    /// A vector where all elements are <see cref="int.MaxValue"/>.
    /// <code>
    /// MaxValue, MaxValue
    /// </code>
    /// </summary>
    public static readonly Vector2i Max = new(int.MaxValue);
    
    /// <summary>
    /// A vector where all elements are <see cref="int.MinValue"/>.
    /// <code>
    /// MinValue, MinValue
    /// </code>
    /// </summary>
    public static readonly Vector2i Min = new(int.MinValue);

    
    /// <summary>
    /// A vector where only X element is one.
    /// <code>
    /// 1, 0
    /// </code>
    /// </summary>
    public static readonly Vector2i UnitX = new(1, 0);
    
    /// <summary>
    /// A vector where only Y element is one.
    /// <code>
    /// 0, 1
    /// </code>
    /// </summary>
    public static readonly Vector2i UnitY = new(0, 1);

    #endregion
    
    public static Vector2i AdditiveIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Zero;
    }

    public static Vector2i MultiplicativeIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => One;
    }

    public static Vector2i MinValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Min;
    }

    public static Vector2i MaxValue
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
    
    #endregion

    public Vector2i Absolute
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
        get => X + Y;
    }

    public int Production
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y;
    }
    
    public int this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(index);
    }
    
    #region Constructors
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i(int x, int y)
    {
        X = x;
        Y = y;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i(int scalar) : this(scalar, scalar)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i(Vector2i vector) : this(vector.X, vector.Y)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i(Vector64<int> vector)
    {
        this = Unsafe.As<Vector64<int>, Vector2i>(ref vector);
    }
    
    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Get(int index)
    {
        Tools.ThrowIfOutOfRange(index, 0, Dimensionality);
        return Unsafe.Add(ref Unsafe.AsRef(in X), index);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i WithX(int value) => new(value, Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2i WithY(int value) => new(X, value);

    #region Cast

    /// <summary>
    /// Returns a new array containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int[] AsArray() => [X, Y];
    
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
    /// Converts this vector to a SIMD Vector64 representation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector64<int> AsVector64() => Unsafe.As<Vector2i, Vector64<int>>(ref Unsafe.AsRef(in this));
    
    #endregion

    #region Equality
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector2i other) =>
        X.Equals(other.X) &&
        Y.Equals(other.Y);
    
    public override bool Equals(object? obj) =>
        obj is Vector2i other &&
        Equals(other);
    
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() =>
        HashCode.Combine(X, Y);

    #endregion

    #region String Formating
    
    /// <inheritdoc/>
    public override string ToString() => $"{X}, {Y}";
    
    #endregion

    #region IEnumerable
    
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<int> GetEnumerator()
    {
        yield return X;
        yield return Y;
    }
    
    #endregion

    #region Comparation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector2i other) =>
        LengthSquared.CompareTo(other.LengthSquared);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(int other) =>
        LengthSquared.CompareTo(other * other);

    #endregion

    #region Static Math
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i Abs(Vector2i vector) =>
        new(Vector64.Abs(vector.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i Clamp(Vector2i vector, Vector2i min, Vector2i max) =>
        new(Vector64.Min(Vector64.Max(vector.AsVector64(), min.AsVector64()), max.AsVector64()));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Dot(Vector2i a, Vector2i b) =>
        Vector64.Dot(a.AsVector64(), b.AsVector64());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector2i a, Vector2i b) =>
        (a - b).LengthSquared;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector2i a, Vector2i b) =>
        (a - b).Length;
   
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceFast(Vector2i a, Vector2i b) =>
        (a - b).LengthFast;
    
    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2i a, Vector2i b) => a.Equals(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2i a, Vector2i b) => !(a == b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator +(Vector2i a, Vector2i b) => new(a.AsVector64() + b.AsVector64());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator +(Vector2i a, Vector64<int> b) => new(a.AsVector64() + b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator +(Vector2i a, int b) => a + Vector64.Create(b);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator +(int a, Vector2i b) => b + a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator +(Vector2i a) => a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator -(Vector2i a, Vector2i b) => new(a.AsVector64() - b.AsVector64());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator -(Vector2i a, Vector64<int> b) => new(a.AsVector64() - b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator -(Vector2i a, int b) => a - Vector64.Create(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator -(int a, Vector2i b) => b - a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator -(Vector2i a) => new(Vector64.Negate(a.AsVector64()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator *(Vector2i a, Vector2i b) => new(a.AsVector64() * b.AsVector64());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator *(Vector2i a, int b) => new(a.AsVector64() * b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator *(int a, Vector2i b) => b * a;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator /(Vector2i a, Vector2i b) => new(a.AsVector64() / b.AsVector64());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator /(Vector2i a, int b) => new(a.AsVector64() / b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2i a, float b) => (Vector2) a / b;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2i((int x, int y) a) => new(a.x, a.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (int x, int y)(Vector2i a) => (a.X, a.Y);
    
    #endregion
    
    public static implicit operator Vector2(Vector2i v) => new(v.X, v.Y);
}