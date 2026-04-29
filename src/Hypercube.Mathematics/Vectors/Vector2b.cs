using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

/// <summary>
/// Represents a vector with two boolean values.
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Vector2b : IEquatable<Vector2b>, IEnumerable<bool>, ISpanFormattable
{
    #region Constants
    
    /// <summary>
    /// The number of components in the vector.
    /// </summary>
    public const int Dimensionality = 2;
    
    /// <summary>
    /// A vector where all elements are false.
    /// <code>
    /// false, false
    /// </code>
    /// </summary>
    public static readonly Vector2b Zero = new(false);
    
    /// <summary>
    /// A vector where all elements are true.
    /// <code>
    /// true, true
    /// </code>
    /// </summary>
    public static readonly Vector2b One = new(true);
    
    /// <summary>
    /// A vector where only X element is true.
    /// <code>
    /// true, false
    /// </code>
    /// </summary>
    public static readonly Vector2b UnitX = new(true, false);
    
    /// <summary>
    /// A vector where only Y element is true.
    /// <code>
    /// false, true
    /// </code>
    /// </summary>
    public static readonly Vector2b UnitY = new(false, true);
    
    #endregion
    
    /// <summary>
    /// Vector X component.
    /// </summary>
    public readonly bool X;
    
    /// <summary>
    /// Vector Y component.
    /// </summary>
    public readonly bool Y;

    /// <summary>
    /// Return X &amp; Y.
    /// </summary>
    public bool All
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X & Y;
    }
    
    /// <summary>
    /// Return X | Y.
    /// </summary>
    public bool Any
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X | Y;
    }
    
    /// <summary>
    /// Return !X &amp; !Y.
    /// </summary>
    public bool None
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => !X & !Y;
    }
    
    /// <summary>
    /// Return X ^ Y.
    /// </summary>
    public bool Xor
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X ^ Y;
    }

    /// <summary>
    /// Return X &amp; !Y.
    /// </summary>
    public bool AloneX
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X & !Y;
    }

    /// <summary>
    /// Return !X &amp; Y.
    /// </summary>
    public bool AloneY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => !X & Y;
    }
    
    public bool this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(index);
    }
    
    #region Constructors
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2b(bool x, bool y)
    {
        X = x;
        Y = y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2b(bool scalar) : this(scalar, scalar)
    {
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2b(Vector64<bool> vector)
    {
        this = Unsafe.As<Vector64<bool>, Vector2b>(ref vector);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2b(Vector64<byte> vector)
    {
        this = Unsafe.As<Vector64<byte>, Vector2b>(ref vector);
    }
    
    #endregion
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out bool x, out bool y)
    {
        x = X;
        y = Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Get(int index)
    {
        Tools.ThrowIfOutOfRange(index, 0, Dimensionality - 1);
        return Unsafe.Add(ref Unsafe.AsRef(in X), index);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2b WithX(bool value) => new(value, Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2b WithY(bool value) => new(X, value);

    #region Cast
    
    /// <summary>
    /// Returns a new array containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool[] AsArray() => [X, Y];
    
    /// <summary>
    /// Returns a new read-only span containing the vector elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<bool> AsSpan() => AsUnsafeSpan();

    /// <summary>
    /// Returns a mutable <see cref="Span{byte}"/> pointing directly to the vector memory.
    /// </summary>
    /// <remarks>
    /// <b>WARNING:</b> This bypasses the readonly constraint.
    /// <para>
    /// For safe, read-only access, use <see cref="AsSpan"/> instead.
    /// </para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<bool> AsUnsafeSpan() =>
        MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in X), Dimensionality);
    
    /// <summary>
    /// Converts this vector to a SIMD Vector64 representation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector64<bool> AsVector64() =>
        Unsafe.As<Vector2b, Vector64<bool>>(ref Unsafe.AsRef(in this));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector64<byte> AsVector64Byte()
        => Unsafe.As<Vector2b, Vector64<byte>>(ref Unsafe.AsRef(in this));

    #endregion

    #region Equality
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector2b other) =>
        X == other.X &&
        Y == other.Y;

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) =>
        obj is Vector2b other && Equals(other);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() =>
        HashCode.Combine(X, Y);

    #endregion
    
    #region String Formating
    
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() =>
        ToString(null, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string? format, IFormatProvider? formatProvider) =>
        $"{X}, {Y}".ToString(formatProvider);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
        destination.TryWrite(provider, $"{X}, {Y}", out charsWritten);

    #endregion
    
    #region IEnumerable
    
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <returns>Enumerator of type <see cref="bool"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<bool> GetEnumerator()
    {
        yield return X;
        yield return Y;
    }

    #endregion
    
    #region Static Math

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2b Nand(Vector2b a, Vector2b b)
        => new(!(a.X & b.X), !(a.Y & b.Y));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2b Nor(Vector2b a, Vector2b b)
        => new(!(a.X | b.X), !(a.Y | b.Y));
    
    #endregion
    
    #region Operators
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2b a, Vector2b b) => a.Equals(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2b a, Vector2b b) => !(a == b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2b operator !(Vector2b a)
        => new(!a.X, !a.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2b operator &(Vector2b a, Vector2b b)
        => new(a.X & b.X, a.Y & b.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2b operator |(Vector2b a, Vector2b b)
        => new(a.X | b.X, a.Y | b.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2b operator ^(Vector2b a, Vector2b b)
        => new(a.X ^ b.X, a.Y ^ b.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(Vector2b v)
        => v is { X: true, Y: true };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(Vector2b v)
        => !v.X || !v.Y;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2b((bool x, bool y) a) => new(a.x, a.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (bool x, bool y)(Vector2b a) => (a.X, a.Y);
    
    #endregion
}