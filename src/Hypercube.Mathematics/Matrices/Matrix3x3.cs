using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Matrices;

[PublicAPI, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial struct Matrix3x3 : IEquatable<Matrix3x3>, IEnumerable<Vector3>
{
    private const int IndexRow0 = 0;
    private const int IndexRow1 = 1;
    private const int IndexRow2 = 2;

    private const int IndexColumn0 = 0;
    private const int IndexColumn1 = 1;
    private const int IndexColumn2 = 2;
    
    public static Matrix3x3 Zero => new(Vector3.Zero);
    public static Matrix3x3 One => new(Vector3.One);
    public static Matrix3x3 Identity => new(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);
    
    public readonly Vector3 Row0;
    public readonly Vector3 Row1;
    public readonly Vector3 Row2;

    public Vector3 Column0 => new(M00, M10, M20);
    public Vector3 Column1 => new(M01, M11, M21);
    public Vector3 Column2 => new(M02, M12, M22);
    
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
    /// Matrix x: 2, y: 0 element.
    /// </summary>
    public float M02
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row0.Z;
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
    /// Matrix x: 2, y: 1 element.
    /// </summary>
    public float M12
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row1.Z;
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
    
    /// <summary>
    /// Matrix x: 2, y: 2 element.
    /// </summary>
    public float M22
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row2.Z;
    }

    public float this[int x, int y] =>
        y switch
        {
            IndexRow0 => x switch
            {
                IndexColumn0 => M00,
                IndexColumn1 => M01,
                IndexColumn2 => M02,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
            },
            IndexRow1 => x switch
            {
                IndexColumn0 => M10,
                IndexColumn1 => M11,
                IndexColumn2 => M12,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
            },
            IndexRow2 => x switch
            {
                IndexColumn0 => M20,
                IndexColumn1 => M21,
                IndexColumn2 => M22,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(y), y, null)
        };

    /// <summary>
    /// Creates 3x3 matrix
    /// <code>
    ///  x.X | x.Y | x.Z
    ///  y.X | y.Y | y.Z
    ///  z.X | z.Y | z.Z
    /// </code>
    /// </summary>
    public Matrix3x3(Vector3 x, Vector3 y, Vector3 z)
    {
        Row0 = x;
        Row1 = y;
        Row2 = z;
    }
    
    /// <summary>
    /// Creates 3x3 matrix
    /// <code>
    ///  value.X | value.Y | value.Z
    ///  value.X | value.Y | value.Z
    ///  value.X | value.Y | value.Z
    /// </code>
    /// </summary>
    public Matrix3x3(Vector3 value)
    {
        Row0 = value;
        Row1 = value;
        Row2 = value;
    }
    
    /// <summary>
    /// Creates 3x3 matrix
    /// <code>
    ///  m00 | m01 | m02
    ///  m10 | m11 | m12
    ///  m20 | m21 | m22
    /// </code>
    /// </summary>
    public Matrix3x3(Matrix3x3 matrix)
    {
        Row0 = matrix.Row0;
        Row1 = matrix.Row1;
        Row2 = matrix.Row2;
    }
    
    /// <summary>
    /// Creates 3x3 matrix
    /// <code>
    ///  m00 | m01 | m02
    ///  m10 | m11 | m12
    ///  m20 | m21 | m22
    /// </code>
    /// </summary>
    public Matrix3x3(
        float m00, float m01, float m02,
        float m10, float m11, float m12,
        float m20, float m21, float m22)
    {
        Row0 = new Vector3(m00, m01, m02);
        Row1 = new Vector3(m10, m11, m12);
        Row2 = new Vector3(m20, m21, m22);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 Transpose(Matrix3x3 matrix3X3)
    {
        return new Matrix3x3(matrix3X3.Column0, matrix3X3.Column1, matrix3X3.Column2);
    }
    
    /// <summary>
    /// Creating translation matrix
    /// <code>
    ///  1  |  0  |  x 
    ///  0  |  1  |  y
    ///  0  |  0  |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateTranslation(Vector2 position)
    {
        return CreateTranslation(position.X, position.Y);
    }
    
    /// <summary>
    /// Creating translation matrix
    /// <code>
    ///  1  |  0  |  x 
    ///  0  |  1  |  y
    ///  0  |  0  |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateTranslation(float x, float y)
    {
        return new Matrix3x3(
            1, 0, x,
            0, 1, y,
            0, 0, 1
        );
    }
    
    /// <summary>
    /// Creating rotation matrix
    /// <code>
    /// cos | -sin |  0
    /// sin |  cos |  0
    ///  0  |   0  |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateRotation(Angle angle)
    {
        return CreateRotation(angle.Theta);
    }

    /// <summary>
    /// Creating rotation matrix
    /// <code>
    /// cos | -sin |  0
    /// sin |  cos |  0
    ///  0  |   0  |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateRotation(double angle)
    {
        var cos = (float)Math.Cos(angle);
        var sin = (float)Math.Sin(angle);
        
        return new Matrix3x3(
            cos, -sin, 0,
            sin, cos, 0,
            0, 0, 1
        );
    }
    
    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |  0  |  0 
    ///  0  |  y  |  0
    ///  0  |  0  |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateScale(Vector2 scale)
    {
        return CreateScale(scale.X, scale.Y);
    }

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |  0  |  0 
    ///  0  |  y  |  0
    ///  0  |  0  |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateScale(float x, float y)
    {
        return new Matrix3x3(
            x, 0, 0,
            0, y, 0,
            0, 0, 1
        );
    }

    /// <summary>
    /// Creating transform matrix
    /// <code>
    /// cos | -sin |  x
    /// sin |  cos |  y
    ///  0  |   0  |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateTransform(Vector2 position, Angle angle)
    {
        return CreateTransform(position, angle, Vector2.One);
    }
    
    /// <summary>
    /// Creating scaled transform matrix
    /// <code>
    /// cos * scale.X | -sin * scale.Y |  x
    /// sin * scale.X |  cos * scale.Y |  y
    ///       0       |       0        |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateTransform(Vector2 position, Angle angle, Vector2 scale)
    {
        var sin = (float)Math.Sin(angle.Theta);
        var cos = (float)Math.Cos(angle.Theta);
        
        return new Matrix3x3(
            cos * scale.X, -sin * scale.Y, position.X,
            sin * scale.X,  cos * scale.Y, position.Y,
            0, 0, 1
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
    {
        return new Matrix3x3(
            (a.Row0 * b.Column0).Summation, (a.Row0 * b.Column1).Summation, (a.Row0 * b.Column2).Summation,
            (a.Row1 * b.Column0).Summation, (a.Row1 * b.Column1).Summation, (a.Row1 * b.Column2).Summation,
            (a.Row2 * b.Column0).Summation, (a.Row2 * b.Column1).Summation, (a.Row2 * b.Column2).Summation
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Matrix3x3 a, Vector3 b)
    {
        return new Vector3(
            (a.Row0 * b).Summation,
            (a.Row1 * b).Summation,
            (a.Row2 * b).Summation
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Matrix3x3 other)
    {
        return Row0.Equals(other.Row0) &&
               Row1.Equals(other.Row1) &&
               Row2.Equals(other.Row2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<Vector3> GetEnumerator()
    {
        yield return Row0;
        yield return Row1;
        yield return Row2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Matrix3x3 other && Equals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Row0, Row1, Row2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Matrix3x3 a, Matrix3x3 b)
    {
        return a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Matrix3x3 a, Matrix3x3 b)
    {
        return !a.Equals(b);
    }
}