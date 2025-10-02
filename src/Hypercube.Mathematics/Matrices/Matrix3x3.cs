using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Matrices;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial struct Matrix3x3 : IEquatable<Matrix3x3>, IEnumerable<Vector3>, IEnumerable<float>
{
    /// <value>
    /// <code>
    /// NaN, NaN, NaN
    /// NaN, NaN, NaN
    /// NaN, NaN, NaN
    /// </code>
    /// </value>
    public static readonly Matrix3x3 NaN = new(
        float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN
    );
    
    /// <value>
    /// <code>
    /// 0, 0, 0
    /// 0, 0, 0
    /// 0, 0, 0
    /// </code>
    /// </value>
    public static readonly Matrix3x3 Zero = new(
        0, 0, 0,
        0, 0, 0,
        0, 0, 0
    );
    
    /// <value>
    /// <code>
    /// 1, 1, 1
    /// 1, 1, 1
    /// 1, 1, 1
    /// </code>
    /// </value>
    public static readonly Matrix3x3 One = new(
        1, 1, 1,
        1, 1, 1,
        1, 1, 1
    );
    
    /// <value>
    /// <code>
    /// 1, 0, 0
    /// 0, 1, 0
    /// 0, 0, 1
    /// </code>
    /// </value>
    public static readonly Matrix3x3 Identity = new(
        1, 0, 0,
        0, 1, 0,
        0, 0, 1
    );
    
    public Vector3 Row0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M00, M01, M02);
    }

    public Vector3 Row1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M10, M11, M12);
    }

    public Vector3 Row2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M20, M21, M22);
    }

    public Vector3 Column0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M00, M10, M20);
    }

    public Vector3 Column1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M01, M11, M21);
    }

    public Vector3 Column2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M02, M12, M22);
    }

    /// <summary>
    /// Matrix x: 0, y: 0 element.
    /// </summary>
    public readonly float M00;

    /// <summary>
    /// Matrix x: 1, y: 0 element.
    /// </summary>
    public readonly float M01;

    /// <summary>
    /// Matrix x: 2, y: 0 element.
    /// </summary>
    public readonly float M02;

    /// <summary>
    /// Matrix x: 0, y: 1 element.
    /// </summary>
    public readonly float M10;

    /// <summary>
    /// Matrix x: 1, y: 1 element.
    /// </summary>
    public readonly float M11;

    /// <summary>
    /// Matrix x: 2, y: 1 element.
    /// </summary>
    public readonly float M12;

    /// <summary>
    /// Matrix x: 0, y: 2 element.
    /// </summary>
    public readonly float M20;

    /// <summary>
    /// Matrix x: 1, y: 2 element.
    /// </summary>
    public readonly float M21;

    /// <summary>
    /// Matrix x: 2, y: 2 element.
    /// </summary>
    public readonly float M22;

    public Matrix3x3 Transposed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(
            M00, M10, M20,
            M01, M11, M21,
            M02, M12, M22
        );
    }

    public float[] Array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => [
            M00, M10, M20,
            M01, M11, M21,
            M02, M12, M22
        ];
    }
    
    public float this[int x, int y] =>
        y switch
        {
            0 => x switch
            {
                0 => M00,
                1 => M01,
                2 => M02,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
            },
            1 => x switch
            {
                0 => M10,
                1 => M11,
                2 => M12,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
            },
            2 => x switch
            {
                0 => M20,
                1 => M21,
                2 => M22,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(y), y, null)
        };
    
    /// <summary>
    /// Creates 3x3 matrix.
    /// <code>
    ///  m00, m01, m02
    ///  m10, m11, m12
    ///  m20, m21, m22
    /// </code>
    /// </summary>
    public Matrix3x3(
        float m00, float m01, float m02,
        float m10, float m11, float m12,
        float m20, float m21, float m22)
    {
        M00 = m00;
        M01 = m01;
        M02 = m02;
        M10 = m10;
        M11 = m11;
        M12 = m12;
        M20 = m20;
        M21 = m21;
        M22 = m22;
    }
    
    /// <summary>
    /// Creates 3x3 matrix
    /// <code>
    ///  x.X | x.Y | x.Z
    ///  y.X | y.Y | y.Z
    ///  z.X | z.Y | z.Z
    /// </code>
    /// </summary>
    public Matrix3x3(Vector3 row0, Vector3 row1, Vector3 row2)
    {
        M00 = row0.X;
        M01 = row0.Y;
        M02 = row0.Z;
        M10 = row1.X;
        M11 = row1.Y;
        M12 = row1.Z;
        M20 = row2.X;
        M21 = row2.Y;
        M22 = row2.Z;
    }
    
    /// <summary>
    /// Creates 3x3 matrix.
    /// <code>
    ///  m00, m01, m02
    ///  m10, m11, m12
    ///  m20, m21, m22
    /// </code>
    /// </summary>
    public Matrix3x3(Matrix3x3 matrix)
    {
        M00 = matrix.M00;
        M01 = matrix.M01;
        M02 = matrix.M02;
        M10 = matrix.M10;
        M11 = matrix.M11;
        M12 = matrix.M12;
        M20 = matrix.M20;
        M21 = matrix.M21;
        M22 = matrix.M22;
    }
    
    public bool Equals(Matrix3x3 other)
    {
        return M00.AboutEquals(other.M00) && M01.AboutEquals(other.M01) && M02.AboutEquals(other.M02) &&
               M10.AboutEquals(other.M10) && M11.AboutEquals(other.M11) && M12.AboutEquals(other.M12) &&
               M20.AboutEquals(other.M20) && M21.AboutEquals(other.M21) && M22.AboutEquals(other.M22);
    }
    
    [MustDisposeResource]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<float>) this).GetEnumerator();
    }
    
    IEnumerator<Vector3> IEnumerable<Vector3>.GetEnumerator()
    {
        yield return Row0;
        yield return Row1;
        yield return Row2;
    }

    IEnumerator<float> IEnumerable<float>.GetEnumerator()
    {
        yield return M00;
        yield return M01;
        yield return M02;
        yield return M10;
        yield return M11;
        yield return M12;
        yield return M20;
        yield return M21;
        yield return M22;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(
        out float m00, out float m01, out float m02,
        out float m10, out float m11, out float m12,
        out float m20, out float m21, out float m22)
    {
        m00 = M00;
        m01 = M01;
        m02 = M02;
        m10 = M10;
        m11 = M11;
        m12 = M12;
        m20 = M20;
        m21 = M21;
        m22 = M22;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out Vector3 row0, out Vector3 row1, out Vector3 row2)
    {
        row0 = Row0;
        row1 = Row1;
        row2 = Row2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Matrix3x3 Transform(float scale)
    {
        return Multiply(this, scale);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Transform(Vector3 vector)
    {
        return Multiply(this, vector);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Matrix3x2 Transform(Matrix3x2 matrix)
    {
        return Multiply(this, matrix);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Matrix3x3 Transform(Matrix3x3 matrix)
    {
        return Multiply(this, matrix);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Matrix3x3 other && Equals(other);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(HashCode.Combine(M00, M01, M02, M10, M11), M12, M20, M21, M22);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"{M00}, {M01}, {M02}, {M10}, {M11}, {M12}, {M20}, {M21}, {M22}";
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator +(Matrix3x3 a, Matrix3x3 b)
    {
        return new Matrix3x3(
            a.M00 + b.M00,
            a.M01 + b.M01,
            a.M02 + b.M02,
            a.M10 + b.M10,
            a.M11 + b.M11,
            a.M12 + b.M12,
            a.M20 + b.M20,
            a.M21 + b.M21,
            a.M22 + b.M22
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator -(Matrix3x3 a)
    {
        return new Matrix3x3(
            -a.M00,
            -a.M01,
            -a.M02,
            -a.M10,
            -a.M11,
            -a.M12,
            -a.M20,
            -a.M21,
            -a.M22
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator -(Matrix3x3 a, Matrix3x3 b)
    {
        return new Matrix3x3(
            a.M00 - b.M00,
            a.M01 - b.M01,
            a.M02 - b.M02,
            a.M10 - b.M10,
            a.M11 - b.M11,
            a.M12 - b.M12,
            a.M20 - b.M20,
            a.M21 - b.M21,
            a.M22 - b.M22
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator *(Matrix3x3 m, float s)
    {
        return Multiply(m, s);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]  
    public static Vector3 operator *(Matrix3x3 m, Vector3 v)
    {
        return Multiply(m, v);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x2 operator *(Matrix3x3 l, Matrix3x2 r)
    {
        return Multiply(l, r);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator *(Matrix3x3 l, Matrix3x3 r)
    {
        return Multiply(l, r);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 Multiply(Matrix3x3 m, float s)
    {
        return new Matrix3x3(
            m.M00 * s,
            m.M01 * s,
            m.M02 * s,
            m.M10 * s,
            m.M11 * s,
            m.M12 * s,
            m.M20 * s,
            m.M21 * s,
            m.M22 * s
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Multiply(Matrix3x3 m, Vector3 v)
    {
        return new Vector3(
            m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z,
            m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z,
            m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x2 Multiply(Matrix3x3 l, Matrix3x2 r)
    {    
        return new Matrix3x2(
            l.M00 * r.M00 + l.M01 * r.M10 + l.M02 * r.M20,
            l.M00 * r.M01 + l.M01 * r.M11 + l.M02 * r.M21,
            l.M10 * r.M00 + l.M11 * r.M10 + l.M12 * r.M20,
            l.M10 * r.M01 + l.M11 * r.M11 + l.M12 * r.M21,
            l.M20 * r.M00 + l.M21 * r.M10 + l.M22 * r.M20,
            l.M20 * r.M01 + l.M21 * r.M11 + l.M22 * r.M21
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 Multiply(Matrix3x3 l, Matrix3x3 r)
    {
        return new Matrix3x3(
            l.M00 * r.M00 + l.M01 * r.M10 + l.M02 * r.M20,
            l.M00 * r.M01 + l.M01 * r.M11 + l.M02 * r.M21,
            l.M00 * r.M02 + l.M01 * r.M12 + l.M02 * r.M22,
            l.M10 * r.M00 + l.M11 * r.M10 + l.M12 * r.M20,
            l.M10 * r.M01 + l.M11 * r.M11 + l.M12 * r.M21,
            l.M10 * r.M02 + l.M11 * r.M12 + l.M12 * r.M22,
            l.M20 * r.M00 + l.M21 * r.M10 + l.M22 * r.M20,
            l.M20 * r.M01 + l.M21 * r.M11 + l.M22 * r.M21,
            l.M20 * r.M02 + l.M21 * r.M12 + l.M22 * r.M22
        );
    }
    
    /// <summary>
    /// Creating transform matrix.
    /// <code>
    /// cos * s, -sin * s,  x
    /// sin * s,  cos * s,  y
    ///    0   ,     0   ,  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateTransform(Vector2 position, Angle angle, Vector2 scale)
    {
        var sin = (float) double.Sin(angle.Theta);
        var cos = (float) double.Cos(angle.Theta);
        return new Matrix3x3(
            cos * scale.X, -sin * scale.Y, position.X,
            sin * scale.X,  cos * scale.Y, position.Y,
            0, 0, 1
        );
    }
    
    /// <summary>
    /// Creating transform matrix.
    /// <code>
    /// cos, -sin,  x
    /// sin,  cos,  y
    ///  0 ,   0 ,  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateTransform(Vector2 position, Angle angle)
    {
        var sin = (float) double.Sin(angle.Theta);
        var cos = (float) double.Cos(angle.Theta);
        return new Matrix3x3(
            cos, -sin, position.X,
            sin,  cos, position.Y,
            0, 0, 1
        );
    }
    
    /// <summary>
    /// Creating scale matrix.
    /// <code>
    /// v, 0, 0 
    /// 0, v, 0
    /// 0, 0, 1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateScale(float value)
    {
        return new Matrix3x3(
            value, 0, 0,
            0, value, 0,
            0, 0, 1
        );
    }
    
    /// <summary>
    /// Creating scale matrix.
    /// <code>
    /// x, 0, 0 
    /// 0, y, 0
    /// 0, 0, 1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateScale(Vector2 scale)
    {
        return new Matrix3x3(
            scale.X, 0, 0,
            0, scale.Y, 0,
            0, 0, 1
        );
    }

    /// <summary>
    /// Creating scale matrix.
    /// <code>
    /// x, 0, 0 
    /// 0, y, 0
    /// 0, 0, 1
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
    /// Creating rotation matrix.
    /// <code>
    /// cos, -sin,  0
    /// sin,  cos,  0
    ///  0 ,   0 ,  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateRotation(Angle angle)
    {
        return CreateRotation(angle.Theta);
    }

    /// <summary>
    /// Creating rotation matrix.
    /// <code>
    /// cos, -sin,  0
    /// sin,  cos,  0
    ///  0 ,   0 ,  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateRotation(double angle)
    {
        var cos = (float) double.Cos(angle);
        var sin = (float) double.Sin(angle);
        return new Matrix3x3(
            cos, -sin, 0,
            sin, cos, 0,
            0, 0, 1
        );
    }

    /// <summary>
    /// Creating translation matrix.
    /// <code>
    /// 1, 0, x 
    /// 0, 1, y
    /// 0, 0, 1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateTranslation(Vector2 position)
    {
        return CreateTranslation(position.X, position.Y);
    }

    /// <summary>
    /// Creating translation matrix.
    /// <code>
    /// 1, 0, x 
    /// 0, 1, y
    /// 0, 0, 1
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
}