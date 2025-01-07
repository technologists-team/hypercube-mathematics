using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Shapes;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public readonly struct Box2i
{
    public static readonly Box2i Zero = new(Vector2i.Zero, Vector2i.Zero);
    
    public int Left => Point0.X;
    public int Top => Point0.Y;
    public int Right => Point1.X;
    public int Bottom => Point1.Y;

    public Vector2i TopLeft => new(Left, Top);
    public Vector2i BottomLeft => new(Left, Bottom);
    public Vector2i TopRight => new(Right, Top);
    public Vector2i BottomRight => new(Right, Bottom);
    
    public int Width => Math.Abs(Right - Left);
    public int Height => Math.Abs(Bottom - Top);
    
    public Vector2i Size => new(Width, Height);
    public Vector2i Center => (Point0 + Point1) / 2;

    public readonly Vector2i Point0;
    public readonly Vector2i Point1;

    public Vector2i Diagonal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TopLeft - BottomRight;
    }
    
    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Width * Width + Height * Height;
    }
    
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MathF.Sqrt(LengthSquared);
    }

    public Vector2i[] Vertices
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get =>
        [
            TopLeft, TopRight, BottomRight, BottomLeft
        ];
    }
    
    public Box2i(Vector2i point0, Vector2i point1)
    {
        Point0 = point0;
        Point1 = point1;
    }

    public Box2i(int left, int top, int right, int bottom)
    {
        Point0 = new Vector2i(left, top);
        Point1 = new Vector2i(right, bottom);
    }

    public Box2i(int value)
    {
        Point0 = new Vector2i(value);
        Point1 = new Vector2i(value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Box2i operator +(Box2i a, Box2i b)
    {
        return new Box2i(a.Point0 + b.Point0, a.Point1 + b.Point1);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Box2i operator +(Box2i a, Vector2i b)
    {
        return new Box2i(a.Point0 + b.X, a.Point1 + b.Y);
    }
}