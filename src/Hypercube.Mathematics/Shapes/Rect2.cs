using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Shapes;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public readonly struct Rect2 : IEquatable<Rect2>, IEnumerable<Vector2>, ISpanFormattable
{
    public static readonly Rect2 NaN = new(float.NaN, float.NaN, float.NaN, float.NaN);
    public static readonly Rect2 Zero = new(0, 0, 0, 0);
    public static readonly Rect2 UV = new(0, 1, 1, 0);
    public static readonly Rect2 Centered = new(-0.5f, -0.5f, 0.5f, 0.5f);
    
    public readonly float Left;
    public readonly float Top;
    public readonly float Right;
    public readonly float Bottom;
    
    /// <remarks>
    /// Point 0.
    /// </remarks>
    public Vector2 TopLeft
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Left, Top);
    }

    public Vector2 BottomLeft
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Left, Bottom);
    }

    public Vector2 TopRight
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Right, Top);
    }

    /// <remarks>
    /// Point 1.
    /// </remarks>
    public Vector2 BottomRight
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Right, Bottom);
    }

    public float Width
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Right - Left;
    }

    public float Height
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Bottom - Top;
    }

    public Vector2 Size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Width, Height);
    }

    public Vector2 Center
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Left + Width / 2, Top + Height / 2);
    }

    public Vector2 Diagonal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => BottomRight - TopLeft;
    }

    public Vector2[] Vertices
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => [TopLeft, TopRight, BottomRight, BottomLeft];
    }
    
    public float Perimeter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 2 * (Width + Height);
    }

    public float Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Width * Height;
    }

    public float AspectRatio
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Width / Height;
    }

    public bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => !float.IsNaN(Left) && !float.IsNaN(Right) && !float.IsNaN(Top) && !float.IsNaN(Bottom);
    }

    public Rect2(float left, float top, float right, float bottom)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(left, right, nameof(left));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(bottom, top, nameof(bottom));
        
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
    
    public Rect2(Vector2 topLeft, Vector2 rightBottom) : this(topLeft.X, topLeft.Y, rightBottom.X, rightBottom.Y)
    {
    }

    public Rect2(Rect2 rect) : this(rect.Left, rect.Top, rect.Right, rect.Bottom)
    {
    }
    
    public void Deconstruct(out float left, out float top, out float right, out float bottom)
    {
        left = Left;
        top = Top;
        right = Right;
        bottom = Bottom;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect2 WithLeft(float left)
    {
        return new Rect2(left, Top, Right, Bottom);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect2 WithRight(float right)
    {
        return new Rect2(Left, Top, right, Bottom);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect2 WithTop(float top)
    {
        return new Rect2(Left, top, Right, Bottom);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect2 WithBottom(float bottom)
    {
        return new Rect2(Left, Top, Right, bottom);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect2 Inflate(Vector2 dimensions)
    {
        return new Rect2(Left - dimensions.X, Top - dimensions.Y, Right + dimensions.X, Bottom + dimensions.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect2 Translate(Vector2 offset)
    {
        return new Rect2(Left + offset.X, Top + offset.Y, Right + offset.X, Bottom + offset.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 NearestPoint(Vector2 point)
    {
        return new Vector2(float.Clamp(point.X, Left, Right),
            float.Clamp(point.Y, Top, Bottom));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(Vector2 point)
    {
        return point.X >= Left &&
               point.X <= Right &&
               point.Y >= Top &&
               point.Y <= Bottom;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(Rect2 other)
    {
        return other.Left >= Left &&
               other.Right <= Right &&
               other.Top >= Top
               && other.Bottom <= Bottom;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Intersects(Rect2 other)
    {
        return other.Left <= Right &&
               other.Right >= Left &&
               other.Top <= Bottom &&
               other.Bottom >= Top;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Overlaps(Rect2 other)
    {
        return other.Left < Right &&
               other.Right > Left &&
               other.Top < Bottom &&
               other.Bottom > Top;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"[({Left}, {Top}), ({Right}, {Bottom})]";
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return $"[({Left}, {Top}), ({Right}, {Bottom})]".ToString(formatProvider);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return destination.TryWrite(provider, $"[({Left}, {Top}), ({Right}, {Bottom})]", out charsWritten);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Rect2 other)
    {
        return Left.AboutEquals(other.Left) &&
               Top.AboutEquals(other.Top) &&
               Right.AboutEquals(other.Right) &&
               Bottom.AboutEquals(other.Bottom);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Rect2 other, float tolerance)
    {
        return Left.AboutEquals(other.Left, tolerance) &&
               Top.AboutEquals(other.Top, tolerance) &&
               Right.AboutEquals(other.Right, tolerance) &&
               Bottom.AboutEquals(other.Bottom, tolerance);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<Vector2> GetEnumerator()
    {
        yield return TopLeft;
        yield return TopRight;
        yield return BottomRight;
        yield return BottomLeft;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Rect2 other && Equals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Left, Top, Right, Bottom);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect2 operator +(Rect2 a, Rect2 b)
    {
        return new Rect2(a.Left + b.Left, a.Top + b.Top, a.Right + b.Right, a.Bottom + b.Bottom);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect2 operator -(Rect2 a)
    {
        return new Rect2(-a.Left, -a.Top, -a.Right, -a.Bottom);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect2 operator -(Rect2 a, Rect2 b)
    {
        return new Rect2(a.Left - b.Left, a.Top - b.Top, a.Right - b.Right, a.Bottom - b.Bottom);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Rect2 a, Rect2 b)
    {
        return a.Equals(b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Rect2 a, Rect2 b)
    {
        return !a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect2 FromSize(Vector2 position, Vector2 size)
    {
        return new Rect2(position.X, position.Y + size.Y, position.X + size.X, position.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect2 FromCenter(Vector2 center, Vector2 size)
    {
        var halfSize = size / 2f;
        return new Rect2(
            center.X - halfSize.X,
            center.Y - halfSize.Y,
            center.X + halfSize.X,
            center.Y + halfSize.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect2 FromUnnormalized(float x1, float y1, float x2, float y2)
    {
        return new Rect2(
            float.Min(x1, x2),
            float.Max(y1, y2),
            float.Max(x1, x2),
            float.Min(y1, y2));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect2 FromUnnormalized(Vector2 p1, Vector2 p2)
    {
        return new Rect2(
            float.Min(p1.X, p2.X),
            float.Max(p1.Y, p2.Y),
            float.Max(p1.X, p2.X),
            float.Min(p1.Y, p2.Y));
    }
}
