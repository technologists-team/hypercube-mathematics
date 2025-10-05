using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

/// <summary>
/// Represents a vector with two boolean values.
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly partial struct Vector2b : IEquatable<Vector2b>, IEnumerable<bool>, ISpanFormattable
{
    /// <value>
    /// Vector (0, 0).
    /// </value>
    public static readonly Vector2b Zero = new(false);
    
    /// <value>
    /// Vector (1, 1).
    /// </value>
    public static readonly Vector2b One = new(true);
    
    /// <value>
    /// Vector (1, 0).
    /// </value>
    public static readonly Vector2b UnitX = new(true, false);
    
    /// <value>
    /// Vector (0, 1).
    /// </value>
    public static readonly Vector2b UnitY = new(false, true);
    
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
    
    /// <summary>
    /// Gets the component of the vector by index.
    /// </summary>
    /// <param name="index">
    /// The component index: 0 for <see cref="X"/>, 1 for <see cref="Y"/>.
    /// </param>
    /// <returns>The bool value of the component at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="index"/> is not 0 or 1.
    /// </exception>
    public bool this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return index switch
            {
                0 => X,
                1 => Y,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };
        }
    }
    
    /// <summary>
    /// Initializes a new vector with specified X and Y components.
    /// </summary>
    /// <param name="x">Value for the X component.</param>
    /// <param name="y">Value for the Y component.</param>
    public Vector2b(bool x, bool y)
    {
        X = x;
        Y = y;
    }
    
    /// <summary>
    /// Initializes a new vector with both components set to the same value.
    /// </summary>
    /// <param name="value">Value for both X and Y components.</param>
    public Vector2b(bool value)
    {
        X = value;
        Y = value;
    }
    
    /// <summary>
    /// Returns a new vector with the X component replaced.
    /// </summary>
    /// <param name="value">New X value.</param>
    /// <returns>A new <see cref="Vector2b"/> with updated X.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2b WithX(bool value)
    {
        return new Vector2b(value, Y);
    }
    
    /// <summary>
    /// Returns a new vector with the Y component replaced.
    /// </summary>
    /// <param name="value">New Y value.</param>
    /// <returns>A new <see cref="Vector2b"/> with updated Y.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2b WithY(bool value)
    {
        return new Vector2b(X, value);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates over the components (X, Y) of the vector.
    /// </summary>
    /// <returns>Enumerator of type <see cref="bool"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<bool> GetEnumerator()
    {
        yield return X;
        yield return Y;
    }

    /// <summary>
    /// Determines whether the specified <see cref="Vector2b"/> is equal to the current vector.
    /// </summary>
    /// <param name="other">The vector to compare with the current vector.</param>
    /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector2b other)
    {
        return X == other.X &&
               Y == other.Y;
    }
    
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Vector2b other && Equals(other);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"{X}, {Y}";
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return $"{X}, {Y}".ToString(formatProvider);
    }
    
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return destination.TryWrite(provider, $"{X}, {Y}", out charsWritten);
    }
    
    /// <summary>
    /// Determines whether two vectors are equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2b left, Vector2b right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two vectors are not equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2b left, Vector2b right)
    {
        return !left.Equals(right);
    }
}