using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Dimensions;

/// <summary>
/// Represents a 2D hybrid affine transformation composed of two independent linear dimensions: <br />
/// <c>f(x) = x * HDimX.Scalar + HDimX.Offset</c><br />
/// <c>f(y) = x * HDimX.Scalar + HDimY.Offset</c><br />
/// </summary>
/// <remarks>
/// This structure models a pair of affine functions applied to a 2D vector:
/// a combination of proportional (scalar) and constant (offset) components, for both axis.<br />
/// It can be interpreted as:
/// <list type="bullet">
/// <item><description>A per-axis linear transformation in 2D space</description></item>
/// <item><description>A compact representation of 2D affine scaling and shifting</description></item>
/// <item><description>A composable transformation unit for vector spaces</description></item>
/// </list>
/// </remarks>
/// <example>
/// Example interpretation:
/// <code>
/// HDim2 d = new HDim2(0.5f, 10f, 2f, -5f);
/// Vector2 result = d.Resolve(new Vector2(100f, 20f));
/// // X: 0.5 * 100 + 10 = 60
/// // Y: 2 * 20 - 5 = 35
/// // Result: (60, 35)
/// </code>
/// </example>
/// <seealso cref="HDim"/>
[PublicAPI]
public readonly struct HDim2 : IEquatable<HDim2>
{
    public static readonly HDim2 Zero = new(HDim.Zero, HDim.Zero);
    public static readonly HDim2 ScalarHalf = new(0.5f, 0, 0.5f, 0.5f);
    public static readonly HDim2 ScalarOne = new(1, 0, 1, 0);

    public readonly HDim X;
    public readonly HDim Y;


    public HDim2(HDim x, HDim y)
    {
        X = x;
        Y = y;
    }

    public HDim2(float scalarX, float offsetX, float scalarY, float offsetY)
    {
        X = new HDim(scalarX, offsetX);
        Y = new HDim(scalarY, offsetY);
    }

    public Vector2 Resolve(Vector2 space) => new (X.Resolve(space.X), Y.Resolve(space.Y));

    public bool Equals(HDim2 other) =>
        X.Equals(other.X) &&
        Y.Equals(other.Y);
    
    public bool AboutEquals(HDim2 other, float tolerance = HyperMath.FloatTolerance) =>
        X.AboutEquals(other.X, tolerance) &&
        Y.AboutEquals(other.Y, tolerance);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is HDim2 other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y);
    
    /// <inheritdoc />
    public override string ToString() => $"(X: {X}, Y: {Y})";

    public static bool operator ==(HDim2 left, HDim2 right) => left.AboutEquals(right);

    public static bool operator !=(HDim2 left, HDim2 right) => !(left == right);
}