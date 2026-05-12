using Hypercube.Mathematics.Extensions;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Dimensions;

/// <summary>
/// Represents a linear hybrid value of the form: <br />
/// <c>f(x) = x * Scalar + Offset</c>
/// </summary>
/// <remarks>
/// This structure models a simple affine transformation:
/// a combination of proportional (scalar) and constant (offset) components. <br />
/// It can be interpreted as:
/// <list type="bullet">
/// <item> <description>A linear function of a single variable</description></item>
/// <item> <description>A compact representation of affine scaling</description></item>
/// <item><description> A composable numeric transformation unit</description> </item>
/// </list>
/// </remarks>
/// <example>
/// Example interpretation:
/// <code>
/// HDim d = new HDim(0.5f, 10f);
/// float result = d.Resolve(100f); // 60
/// // 0.5 * 100 + 10 = 60
/// </code>
/// </example>
/// <seealso cref="HDim2"/>
/// <seealso cref="HDimRect"/>
[PublicAPI]
public readonly struct HDim : IEquatable<HDim>
{
    public static readonly HDim Zero = new(0, 0);
    
    public readonly float Scalar;
    public readonly float Offset;

    public HDim(float scalar, float offset)
    {
        Scalar = scalar;
        Offset = offset;
    }
    
    public float Resolve(float space) => space * Scalar + Offset;
    
    public bool IsZero => Scalar == 0f && Offset == 0f;

    public bool Equals(HDim other) =>
        Scalar.Equals(other.Scalar) &&
        Offset.Equals(other.Offset);

    public bool AboutEquals(HDim other, float tolerance = HyperMath.FloatTolerance) =>
        Scalar.AboutEquals(other.Scalar, tolerance) &&
        Offset.AboutEquals(other.Offset, tolerance);
    
    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is HDim other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Scalar, Offset);
    
    /// <inheritdoc />
    public override string ToString()
        => $"s{Scalar}; o{Offset}";

    public static bool operator ==(HDim left, HDim right) => left.AboutEquals(right);

    public static bool operator !=(HDim left, HDim right) => !(left == right);
    
    public static HDim operator +(HDim a, HDim b)
        => new(a.Scalar + b.Scalar, a.Offset + b.Offset);

    public static HDim operator +(HDim a)
        => new(a.Scalar, a.Offset);

    public static HDim operator -(HDim a, HDim b)
        => new(a.Scalar - b.Scalar, a.Offset - b.Offset);

    public static HDim operator -(HDim a)
        => new(-a.Scalar, -a.Offset);

    public static HDim operator *(HDim a, float value)
        => new(a.Scalar * value, a.Offset * value);

    public static HDim operator /(HDim a, float value)
        => new(a.Scalar / value, a.Offset / value);
}