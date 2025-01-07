using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Matrices;

[PublicAPI]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial struct Matrix3x2
{
    public static Matrix3x2 Zero => new(Vector2.Zero);
    public static Matrix3x2 One => new(Vector2.One);
    public static Matrix3x2 Identity => new(Vector2.UnitX, Vector2.UnitY, Vector2.Zero);

    public readonly Vector2 Row0;
    public readonly Vector2 Row1;
    public readonly Vector2 Row2;

    public Vector3 Column0 => new(M00, M10, M20);
    public Vector3 Column1 => new(M01, M11, M21);

    /// <summary>
    /// Matrix x: 0, y: 0 element.
    /// </summary>
    public float M00
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row0.X;
    }

    /// <summary>
    /// Matrix x: 1, y: 0 element.
    /// </summary>
    public float M01
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row0.Y;
    }

    /// <summary>
    /// Matrix x: 0, y: 1 element.
    /// </summary>
    public float M10
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row1.X;
    }

    /// <summary>
    /// Matrix x: 1, y: 1 element.
    /// </summary>
    public float M11
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row1.Y;
    }

    /// <summary>
    /// Matrix x: 0, y: 2 element.
    /// </summary>
    public float M20
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row2.X;
    }

    /// <summary>
    /// Matrix x: 1, y: 2 element.
    /// </summary>
    public float M21
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row2.Y;
    }

    public Matrix3x2(float m00, float m01, float m10, float m11, float m20, float m21)
    {
        Row0 = new Vector2(m00, m01);
        Row1 = new Vector2(m10, m11);
        Row2 = new Vector2(m20, m21);
    }

    public Matrix3x2(Vector2 vector)
    {
        Row0 = vector;
        Row1 = vector;
        Row2 = vector;
    }

    public Matrix3x2(Vector2 row0, Vector2 row1, Vector2 row2)
    {
        Row0 = row0;
        Row1 = row1;
        Row2 = row2;
    }

    /// <summary>
    /// Creating rotation matrix
    /// <code>
    /// cos | -sin
    /// sin |  cos
    ///  0  |   0 
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x2 CreateRotation(Angle angle)
    {
        return CreateRotation(angle.Theta);
    }

    /// <summary>
    /// Creating rotation matrix
    /// <code>
    /// cos | -sin
    /// sin |  cos
    ///  0  |   0 
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x2 CreateRotation(double angle)
    {
        var cos = (float)Math.Cos(angle);
        var sin = (float)Math.Sin(angle);
        return new Matrix3x2(cos, sin, -sin, cos, 0, 0);
    }
    
    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |   0
    ///  0  |   y
    ///  0  |   0 
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x2 CreateScale(Vector2 scale)
    {
        return CreateScale(scale.X, scale.Y);
    }

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |   0
    ///  0  |   y
    ///  0  |   0 
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x2 CreateScale(float x, float y)
    {
        return new Matrix3x2(x, 0, y, 0, 0, 0);
    }
}