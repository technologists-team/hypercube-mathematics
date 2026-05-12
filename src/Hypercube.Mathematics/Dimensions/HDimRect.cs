using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Dimensions;

/// <summary>
/// Represents padding or margin in four directions (bottom, left, top, right).
/// </summary>
/// <seealso cref="HDim"/>
[PublicAPI]
public readonly struct HDimRect
{
    public readonly HDim Bottom;
    public readonly HDim Left;
    public readonly HDim Top;
    public readonly HDim Right;

    /// <summary>
    /// Zero padding (all sides are zero).
    /// </summary>
    public static readonly HDimRect Zero = new(HDim.Zero, HDim.Zero, HDim.Zero, HDim.Zero);

    /// <summary>
    /// Uniform padding on all sides.
    /// </summary>
    public static HDimRect All(HDim value) => new(value, value, value, value);

    /// <summary>
    /// Horizontal and vertical symmetric padding.
    /// </summary>
    public static HDimRect Symmetric(HDim horizontal, HDim vertical)
        => new(vertical, horizontal, vertical, horizontal);

    /// <summary>
    /// Creates padding with independent values.
    /// </summary>
    public static HDimRect FromRect(HDim left, HDim top, HDim right, HDim bottom)
        => new(bottom, left, top, right);

    public HDimRect(HDim bottom, HDim left, HDim top, HDim right)
    {
        Bottom = bottom;
        Left = left;
        Top = top;
        Right = right;
    }

    /// <summary>
    /// Returns total horizontal padding (left + right).
    /// </summary>
    public HDim Horizontal => Left + Right;

    /// <summary>
    /// Returns total vertical padding (top + bottom).
    /// </summary>
    public HDim Vertical => Top + Bottom;

    /// <summary>
    /// Checks whether all sides are zero.
    /// </summary>
    public bool IsZero => Bottom.IsZero && Left.IsZero && Top.IsZero && Right.IsZero;

    /// <summary>
    /// Resolves padding into pixel space using given size context.
    /// </summary>
    public Rect2 Resolve(Vector2 size) => new(
        Left.Resolve(size.X),
        Top.Resolve(size.Y),
        Right.Resolve(size.X),
        Bottom.Resolve(size.Y)
    );

    /// <inheritdoc />
    public override string ToString() => $"(L: {Left}, T: {Top}, R: {Right}, B: {Bottom})";
}
