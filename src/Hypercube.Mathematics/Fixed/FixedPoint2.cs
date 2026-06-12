using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Fixed;

/// <summary>
/// Represents a fixed-point number with 2 decimal places.
/// </summary>
[DebuggerDisplay("{ToString()}")]
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
public readonly struct FixedPoint2 : 
    IEquatable<FixedPoint2>, 
    IComparable<FixedPoint2>,
    ISpanFormattable,
    IAdditionOperators<FixedPoint2, FixedPoint2, FixedPoint2>,
    ISubtractionOperators<FixedPoint2, FixedPoint2, FixedPoint2>,
    IMultiplyOperators<FixedPoint2, FixedPoint2, FixedPoint2>,
    IDivisionOperators<FixedPoint2, FixedPoint2, FixedPoint2>,
    IUnaryPlusOperators<FixedPoint2, FixedPoint2>,
    IUnaryNegationOperators<FixedPoint2, FixedPoint2>,
    IAdditiveIdentity<FixedPoint2, FixedPoint2>,
    IMultiplicativeIdentity<FixedPoint2, FixedPoint2>,
    IComparisonOperators<FixedPoint2, FixedPoint2, bool>,
    IMinMaxValue<FixedPoint2>
{
    public const int DecimalPlaces = 2;
    public const long Scale = 100;

    public static readonly FixedPoint2 Zero = new(0);
    public static readonly FixedPoint2 One = new(Scale);
    public static readonly FixedPoint2 Half = new(Scale / 2);
    public static readonly FixedPoint2 Epsilon = FromRaw(1);
    public static readonly FixedPoint2 MinValue = new(long.MinValue);
    public static readonly FixedPoint2 MaxValue = new(long.MaxValue);

    public static FixedPoint2 AdditiveIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Zero;
    }

    public static FixedPoint2 MultiplicativeIdentity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => One;
    }

    static FixedPoint2 IMinMaxValue<FixedPoint2>.MinValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MinValue;
    }

    static FixedPoint2 IMinMaxValue<FixedPoint2>.MaxValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MaxValue;
    }
    
    public FixedPoint2 Absolute
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Abs(this);
    }

    public FixedPoint2 Rounded
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Round(this);
    }

    public FixedPoint2 Floored
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Floor(this);
    }

    public FixedPoint2 Ceiled
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

    private FixedPoint2(long raw)
    {
        Raw = raw;
    }
    
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(FixedPoint2 other) => Raw == other.Raw;

    /// <summary>
    /// Checks if two fixed-point numbers are approximately equal within a specified tolerance.
    /// </summary>
    /// <param name="other">The other number to compare.</param>
    /// <param name="tolerance">The maximum difference in raw units (default is 1, which equals 0.01).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AboutEquals(FixedPoint2 other, long tolerance = 1)
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

    public override bool Equals(object? obj) => obj is FixedPoint2 other && Equals(other);

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
    public int CompareTo(FixedPoint2 other) => Raw.CompareTo(other.Raw);

    #endregion

    #region Math

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 Abs(FixedPoint2 value) => new(Math.Abs(value.Raw));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 Min(FixedPoint2 a, FixedPoint2 b) => a.Raw < b.Raw ? a : b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 Max(FixedPoint2 a, FixedPoint2 b) => a.Raw > b.Raw ? a : b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 Clamp(FixedPoint2 value, FixedPoint2 min, FixedPoint2 max)
    {
        if (min.Raw > max.Raw)
            throw new ArgumentOutOfRangeException(nameof(min), "Clamp minimum cannot be greater than maximum.");
        
        return new FixedPoint2(Math.Clamp(value.Raw, min.Raw, max.Raw));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 Floor(FixedPoint2 value)
    {
        var raw = value.Raw;
        if (raw == long.MinValue)
            return value;
        
        var frac = raw % Scale;
        if (frac < 0)
            frac += Scale;
        
        return new FixedPoint2(raw - frac);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 Ceiling(FixedPoint2 value)
    {
        var raw = value.Raw;
        var frac = raw % Scale;
        
        if (frac > 0)
            frac -= Scale;
        
        return new FixedPoint2(raw - frac);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 Round(FixedPoint2 value)
    {
        var raw = value.Raw;
        if (raw == long.MinValue)
            return value;
        
        var abs = long.Abs(raw);
        var rounded = (abs + Scale / 2) / Scale * Scale;
        return new FixedPoint2(long.Sign(raw) * rounded);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(FixedPoint2 value) => long.Sign(value.Raw);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 Lerp(FixedPoint2 a, FixedPoint2 b, float t) => a + (b - a) * FromDouble(t);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 Lerp(FixedPoint2 a, FixedPoint2 b, FixedPoint2 t) => a + (b - a) * t;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 FromRaw(long raw) => new(raw);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 FromDouble(double value) => FromDecimal((decimal) value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 FromDecimal(decimal value) => new((long) decimal.Round(value * Scale, MidpointRounding.AwayFromZero));

    #endregion

    #region Parsing

    public static FixedPoint2 Parse(string value) 
        => FromDecimal(decimal.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture));

    public static bool TryParse(string value, out FixedPoint2 result)
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
    public static bool operator ==(FixedPoint2 a, FixedPoint2 b) => a.Raw == b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(FixedPoint2 a, FixedPoint2 b) => a.Raw != b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(FixedPoint2 a, FixedPoint2 b) => a.Raw < b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(FixedPoint2 a, FixedPoint2 b) => a.Raw > b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(FixedPoint2 a, FixedPoint2 b) => a.Raw <= b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(FixedPoint2 a, FixedPoint2 b) => a.Raw >= b.Raw;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 operator +(FixedPoint2 a, FixedPoint2 b)
    {
        checked { return new FixedPoint2(a.Raw + b.Raw); }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 operator +(FixedPoint2 a) => a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 operator -(FixedPoint2 a, FixedPoint2 b)
    {
        checked { return new FixedPoint2(a.Raw - b.Raw); }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 operator -(FixedPoint2 a) => new(-a.Raw);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 operator *(FixedPoint2 a, FixedPoint2 b)
        => new((long)decimal.Round((decimal)a.Raw * b.Raw / Scale, MidpointRounding.AwayFromZero));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 operator /(FixedPoint2 a, FixedPoint2 b)
        => b.Raw == 0
            ? throw new DivideByZeroException()
            : new FixedPoint2((long)decimal.Round((decimal)a.Raw * Scale / b.Raw, MidpointRounding.AwayFromZero));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedPoint2 operator %(FixedPoint2 a, FixedPoint2 b)
        => b.Raw == 0
            ? throw new DivideByZeroException()
            : new FixedPoint2(a.Raw % b.Raw);

    #endregion

    #region Implicit / Explicit

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator FixedPoint2(int value) => new(value * Scale);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator FixedPoint2(long value)
    {
        checked { return new FixedPoint2(value * Scale); }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator double(FixedPoint2 fix) => (double) fix.Raw / Scale;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator float(FixedPoint2 fix) => (float) fix.Raw / Scale;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator decimal(FixedPoint2 fix) => (decimal) fix.Raw / Scale;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int(FixedPoint2 fix) => (int) (fix.Raw / Scale);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator long(FixedPoint2 fix) => fix.Raw / Scale;

    #endregion

    #endregion
}
