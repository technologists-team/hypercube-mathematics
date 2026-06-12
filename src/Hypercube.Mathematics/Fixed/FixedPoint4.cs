using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Fixed;

/// <summary>
/// Represents a fixed-point number with 4 decimal places.
/// </summary>
[DebuggerDisplay("{ToString()}")]
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
public readonly struct FixedPoint4 : 
    IEquatable<FixedPoint4>, 
    IComparable<FixedPoint4>,
    ISpanFormattable,
    IAdditionOperators<FixedPoint4, FixedPoint4, FixedPoint4>,
    ISubtractionOperators<FixedPoint4, FixedPoint4, FixedPoint4>,
    IMultiplyOperators<FixedPoint4, FixedPoint4, FixedPoint4>,
    IDivisionOperators<FixedPoint4, FixedPoint4, FixedPoint4>,
    IUnaryPlusOperators<FixedPoint4, FixedPoint4>,
    IUnaryNegationOperators<FixedPoint4, FixedPoint4>,
    IAdditiveIdentity<FixedPoint4, FixedPoint4>,
    IMultiplicativeIdentity<FixedPoint4, FixedPoint4>,
    IComparisonOperators<FixedPoint4, FixedPoint4, bool>,
    IMinMaxValue<FixedPoint4>
{
    public const int DecimalPlaces = 4;
    public const long Scale = 10_000;

    public static readonly FixedPoint4 Zero = new(0);
    public static readonly FixedPoint4 One = new(Scale);
    public static readonly FixedPoint4 Half = new(Scale / 2);
    public static readonly FixedPoint4 Epsilon = FromRaw(1);
    public static readonly FixedPoint4 MinValue = new(long.MinValue);
    public static readonly FixedPoint4 MaxValue = new(long.MaxValue);

    public static FixedPoint4 AdditiveIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Zero;
    }

    public static FixedPoint4 MultiplicativeIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => One;
    }

    static FixedPoint4 IMinMaxValue<FixedPoint4>.MinValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MinValue;
    }

    static FixedPoint4 IMinMaxValue<FixedPoint4>.MaxValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MaxValue;
    }
    
    public FixedPoint4 Absolute
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Abs(this);
    }

    public FixedPoint4 Rounded
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Round(this);
    }

    public FixedPoint4 Floored
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Floor(this);
    }

    public FixedPoint4 Ceiled
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Ceiling(this);
    }

    public int Signed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => long.Sign(Raw);
    }
    
    public long Raw
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }
    
    public float Float
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (float) Raw / Scale;
    }
    
    public double Double
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (double) Raw / Scale;
    }
    
    public decimal Decimal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (decimal) Raw / Scale;
    }
    
    public int Int
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (int) (Raw / Scale);
    }
    
    public long Long
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Raw / Scale;
    }

    private FixedPoint4(long raw)
    {
        Raw = raw;
    }
    
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(FixedPoint4 other) => Raw == other.Raw;

    /// <summary>
    /// Checks if two fixed-point numbers are approximately equal within a specified tolerance.
    /// </summary>
    /// <param name="other">The other number to compare.</param>
    /// <param name="tolerance">The maximum difference in raw units (default is 1, which equals 0.01).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AboutEquals(FixedPoint4 other, long tolerance = 1)
    {
        var a = Raw;
        var b = other.Raw;
        
        if (a == b)
            return true;
        
        if (tolerance < 0)
            return false;
        
        if ((a ^ b) >= 0)
            return (a > b ? a - b : b - a) <= tolerance;
        
        if (a > 0)
            return a <= tolerance && (a - b) <= tolerance;
        
        return b <= tolerance && (b - a) <= tolerance;
    }

    public override bool Equals(object? obj) => obj is FixedPoint4 other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => Raw.GetHashCode();

    #endregion

    #region String Formatting

    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);

    public string ToString(string? format, IFormatProvider? formatProvider) 
        => ((decimal)Raw / Scale).ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => ((decimal)Raw / Scale).TryFormat(destination, out charsWritten, format, provider);

    #endregion

    #region Comparison

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(FixedPoint4 other) => Raw.CompareTo(other.Raw);

    #endregion

    #region Math

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 Abs(FixedPoint4 value) => new(Math.Abs(value.Raw));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 Min(FixedPoint4 a, FixedPoint4 b) => a.Raw < b.Raw ? a : b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 Max(FixedPoint4 a, FixedPoint4 b) => a.Raw > b.Raw ? a : b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 Clamp(FixedPoint4 value, FixedPoint4 min, FixedPoint4 max)
    {
        if (min.Raw > max.Raw)
            throw new ArgumentOutOfRangeException(nameof(min), "Clamp minimum cannot be greater than maximum.");
        
        return new FixedPoint4(Math.Clamp(value.Raw, min.Raw, max.Raw));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 Floor(FixedPoint4 value)
    {
        var raw = value.Raw;
        if (raw == long.MinValue)
            return value;
        
        var frac = raw % Scale;
        if (frac < 0)
            frac += Scale;
        
        return new FixedPoint4(raw - frac);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 Ceiling(FixedPoint4 value)
    {
        var raw = value.Raw;
        var frac = raw % Scale;
        
        if (frac > 0)
            frac -= Scale;
        
        return new FixedPoint4(raw - frac);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 Round(FixedPoint4 value)
    {
        var raw = value.Raw;
        if (raw == long.MinValue)
            return value;
        
        var abs = long.Abs(raw);
        var rounded = (abs + Scale / 2) / Scale * Scale;
        return new FixedPoint4(long.Sign(raw) * rounded);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(FixedPoint4 value) => long.Sign(value.Raw);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 Lerp(FixedPoint4 a, FixedPoint4 b, float t) => a + (b - a) * FromDouble(t);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 Lerp(FixedPoint4 a, FixedPoint4 b, FixedPoint4 t) => a + (b - a) * t;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 FromRaw(long raw) => new(raw);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 FromDouble(double value) => FromDecimal((decimal) value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 FromDecimal(decimal value) => new((long) decimal.Round(value * Scale, MidpointRounding.AwayFromZero));

    #endregion

    #region Parsing

    public static FixedPoint4 Parse(string value) 
        => FromDecimal(decimal.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture));

    public static bool TryParse(string value, out FixedPoint4 result)
    {
        if (decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
        {
            result = FromDecimal(parsed);
            return true;
        }

        result = Zero;
        return false;
    }

    #endregion

    #region Operators

    #region Calculation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(FixedPoint4 a, FixedPoint4 b) => a.Raw == b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(FixedPoint4 a, FixedPoint4 b) => a.Raw != b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(FixedPoint4 a, FixedPoint4 b) => a.Raw < b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(FixedPoint4 a, FixedPoint4 b) => a.Raw > b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(FixedPoint4 a, FixedPoint4 b) => a.Raw <= b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(FixedPoint4 a, FixedPoint4 b) => a.Raw >= b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 operator +(FixedPoint4 a, FixedPoint4 b)
    {
        checked { return new FixedPoint4(a.Raw + b.Raw); }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 operator +(FixedPoint4 a) => a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 operator -(FixedPoint4 a, FixedPoint4 b)
    {
        checked { return new FixedPoint4(a.Raw - b.Raw); }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 operator -(FixedPoint4 a) => new(-a.Raw);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 operator *(FixedPoint4 a, FixedPoint4 b)
        => new((long)decimal.Round((decimal)a.Raw * b.Raw / Scale, MidpointRounding.AwayFromZero));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 operator /(FixedPoint4 a, FixedPoint4 b)
        => b.Raw == 0
            ? throw new DivideByZeroException()
            : new FixedPoint4((long)decimal.Round((decimal)a.Raw * Scale / b.Raw, MidpointRounding.AwayFromZero));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint4 operator %(FixedPoint4 a, FixedPoint4 b)
        => b.Raw == 0
            ? throw new DivideByZeroException()
            : new FixedPoint4(a.Raw % b.Raw);

    #endregion

    #region Implicit / Explicit

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator FixedPoint4(int value) => new(value * Scale);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator FixedPoint4(long value)
    {
        checked { return new FixedPoint4(value * Scale); }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator double(FixedPoint4 fix) => (double) fix.Raw / Scale;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator float(FixedPoint4 fix) => (float) fix.Raw / Scale;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator decimal(FixedPoint4 fix) => (decimal) fix.Raw / Scale;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int(FixedPoint4 fix) => (int) (fix.Raw / Scale);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator long(FixedPoint4 fix) => fix.Raw / Scale;

    #endregion

    #endregion
}
