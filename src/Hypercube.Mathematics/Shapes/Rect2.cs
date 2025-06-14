﻿using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Shapes;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public readonly struct Rect2
{
    public static readonly Rect2 NaN = new(Vector2.NaN, Vector2.NaN);
    public static readonly Rect2 Zero = new(Vector2.Zero, Vector2.Zero);
    public static readonly Rect2 UV = new(0.0f, 1.0f, 1.0f, 0.0f);
    public static readonly Rect2 Centered = new(-0.5f, -0.5f, 0.5f, 0.5f);
    
    public float Left => Point0.X;
    public float Top => Point0.Y;
    public float Right => Point1.X;
    public float Bottom => Point1.Y;

    public Vector2 TopLeft => new(Left, Top);
    public Vector2 BottomLeft => new(Left, Bottom);
    public Vector2 TopRight => new(Right, Top);
    public Vector2 BottomRight => new(Right, Bottom);
    
    public float Width => MathF.Abs(Right - Left);
    public float Height => MathF.Abs(Bottom - Top);
    public Vector2 Size => new(Width, Height);
    public Vector2 Center => (Point0 + Point1) / 2;

    public readonly Vector2 Point0;
    public readonly Vector2 Point1;

    public Vector2 Diagonal
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

    public Vector2[] Vertices
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get =>
        [
            TopLeft, TopRight, BottomRight, BottomLeft
        ];
    }
    
    public Rect2(Vector2 point0, Vector2 point1)
    {
        Point0 = point0;
        Point1 = point1;
    }

    public Rect2(float left, float top, float right, float bottom)
    {
        Point0 = new Vector2(left, top);
        Point1 = new Vector2(right, bottom);
    }

    public Rect2(float value)
    {
        Point0 = new Vector2(value);
        Point1 = new Vector2(value);
    }
    
    public override string ToString()
    {
        return $"[{Point0}, {Point1}]";
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect2 operator +(Rect2 a, Rect2 b)
    {
        return new Rect2(a.Point0 + b.Point0, a.Point1 + b.Point1);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect2 operator +(Rect2 a, Vector2 b)
    {
        return new Rect2(a.Point0 + b.X, a.Point1 + b.Y);
    }
}