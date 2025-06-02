using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Matrices;

/// <summary>
/// Implementation of a 4x4 matrix for rendering work. (COLUM-MAJOR)
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public readonly partial struct Matrix4x4 : IEquatable<Matrix4x4>, IEnumerable<Vector4>, IEnumerable<float>
{
    /// <summary>
    /// <code>
    /// 0 | 0 | 0 | 0
    /// 0 | 0 | 0 | 0
    /// 0 | 0 | 0 | 0
    /// 0 | 0 | 0 | 0
    /// </code>
    /// </summary>
    public static readonly Matrix4x4 Zero = new(0);
    
    /// <summary>
    /// <code>
    /// 1 | 1 | 1 | 1
    /// 1 | 1 | 1 | 1
    /// 1 | 1 | 1 | 1
    /// 1 | 1 | 1 | 1
    /// </code>
    /// </summary>
    public static readonly Matrix4x4 One = new(1);
    
    /// <summary>
    /// <code>
    /// 1 | 0 | 0 | 0
    /// 0 | 1 | 0 | 0
    /// 0 | 0 | 1 | 0
    /// 0 | 0 | 0 | 1
    /// </code>
    /// </summary>
    public static readonly Matrix4x4 Identity = new(
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );

    public Vector4 Row0 => new(M00, M01, M02, M03);
    public Vector4 Row1 => new(M10, M11, M12, M13);
    public Vector4 Row2 => new(M20, M21, M22, M23);
    public Vector4 Row3 => new(M30, M31, M32, M33);
    public Vector4 Column0 => new(M00, M10, M20, M30);
    public Vector4 Column1 => new(M01, M11, M21, M31);
    public Vector4 Column2 => new(M02, M12, M22, M32);
    public Vector4 Column3 => new(M03, M13, M23, M33);
    
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
    /// Matrix x: 3, y: 0 element.
    /// </summary>
    public readonly float M03;

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
    /// Matrix x: 3, y: 1 element.
    /// </summary>
    public readonly float M13;

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

    /// <summary>
    /// Matrix x: 3, y: 2 element.
    /// </summary>
    public readonly float M23;

    /// <summary>
    /// Matrix x: 0, y: 3 element.
    /// </summary>
    public readonly float M30;

    /// <summary>
    /// Matrix x: 1, y: 3 element.
    /// </summary>
    public readonly float M31;

    /// <summary>
    /// Matrix x: 2, y: 3 element.
    /// </summary>
    public readonly float M32;

    /// <summary>
    /// Matrix x: 3, y: 3 element.
    /// </summary>
    public readonly float M33;

    public float this[int x, int y] =>
        y switch
        {
            0 => x switch
            {
                0 => M00,
                1 => M01,
                2 => M02,
                3 => M03,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
            },
            1 => x switch
            {
                0 => M10,
                1 => M11,
                2 => M12,
                3 => M13,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
            },
            2 => x switch
            {
                0 => M20,
                1 => M21,
                2 => M22,
                3 => M23,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
            },
            3 => x switch
            {
                0 => M30,
                1 => M31,
                2 => M32,
                3 => M33,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(y), y, null)
        };
    
    /// <summary>
    /// Creates new matrix with all rows is "<paramref name="value"/>"
    /// <code>
    /// value | value | value | value 
    /// value | value | value | value
    /// value | value | value | value 
    /// value | value | value | value 
    /// </code>
    /// </summary>
    /// <param name="value">Vector4 to make rows out of</param>
    public Matrix4x4(float value)
    {
        M00 = value;
        M01 = value;
        M02 = value;
        M03 = value;
        M10 = value;
        M11 = value;
        M12 = value;
        M13 = value;
        M20 = value;
        M21 = value;
        M22 = value;
        M23 = value;
        M30 = value;
        M31 = value;
        M32 = value;
        M33 = value;
    }
    
    /// <summary>
    /// Creating matrix
    /// <code>
    /// m00 | m01 | m02 | m03 
    /// m10 | m11 | m12 | m13
    /// m20 | m21 | m22 | m23
    /// m30 | m31 | m32 | m33
    /// </code>
    /// </summary>
    public Matrix4x4(
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33)
    {
        M00 = m00;
        M01 = m01;
        M02 = m02;
        M03 = m03;
        M10 = m10;
        M11 = m11;
        M12 = m12;
        M13 = m13;
        M20 = m20;
        M21 = m21;
        M22 = m22;
        M23 = m23;
        M30 = m30;
        M31 = m31;
        M32 = m32;
        M33 = m33;
    }
    
    /// <summary>
    /// Creates new matrix with all rows is "<paramref name="value"/>"
    /// <code>
    /// value.X | value.Y | value.Z | value.W 
    /// value.X | value.Y | value.Z | value.W 
    /// value.X | value.Y | value.Z | value.W 
    /// value.X | value.Y | value.Z | value.W 
    /// </code>
    /// </summary>
    /// <param name="value">Vector4 to make rows out of</param>
    public Matrix4x4(Vector4 value)
    {
        M00 = value.X;
        M01 = value.Y;
        M02 = value.Z;
        M03 = value.W;
        M10 = value.X;
        M11 = value.Y;
        M12 = value.Z;
        M13 = value.W;
        M20 = value.X;
        M21 = value.Y;
        M22 = value.Z;
        M23 = value.W;
        M30 = value.X;
        M31 = value.Y;
        M32 = value.Z;
        M33 = value.W;
    }

    /// <summary>
    /// Creates new matrix 4x4
    /// <code>
    /// Row0.X | Row0.Y | Row0.Z | Row0.W 
    /// Row1.X | Row1.Y | Row1.Z | Row1.W 
    /// Row2.X | Row2.Y | Row2.Z | Row2.W 
    /// Row2.X | Row2.Y | Row2.Z | Row2.W
    /// </code>
    /// </summary>
    /// <param name="x">Row 0</param>
    /// <param name="y">Row 1</param>
    /// <param name="z">Row 2</param>
    /// <param name="w">Row 3</param>
    public Matrix4x4(Vector4 x, Vector4 y, Vector4 z, Vector4 w)
    {
        M00 = x.X;
        M01 = x.Y;
        M02 = x.Z;
        M03 = x.W;
        M10 = y.X;
        M11 = y.Y;
        M12 = y.Z;
        M13 = y.W;
        M20 = z.X;
        M21 = z.Y;
        M22 = z.Z;
        M23 = z.W;
        M30 = w.X;
        M31 = w.Y;
        M32 = w.Z;
        M33 = w.W;
    }
    
    /// <summary>
    /// Creating matrix
    /// <code>
    ///  m00 | m01 | m02 |  0
    ///  m10 | m11 | m12 |  0
    ///  m20 | m21 | m22 |  0
    ///   0  |  0  |  0  |  0
    /// </code>
    /// </summary>
    public Matrix4x4(Matrix3x3 matrix)
    {
        M00 = matrix.Row0.X;
        M01 = matrix.Row0.Y;
        M02 = matrix.Row0.Z;
        M03 = 0;
        M10 = matrix.Row1.X;
        M11 = matrix.Row1.Y;
        M12 = matrix.Row1.Z;
        M13 = 0;
        M20 = matrix.Row2.X;
        M21 = matrix.Row2.Y;
        M22 = matrix.Row2.Z;
        M23 = 0;
        M30 = 0;
        M31 = 0;
        M32 = 0;
        M33 = 0;
    }

    /// <summary>
    /// Creating matrix
    /// <code>
    ///  m00 | m01 | m02 | m03 
    ///  m10 | m11 | m12 | m13
    ///  m20 | m21 | m22 | m23
    ///  m30 | m31 | m32 | m33
    /// </code>
    /// </summary>
    public Matrix4x4(Matrix4x4 matrix)
    {
        M00 = matrix.M00;
        M01 = matrix.M01;
        M02 = matrix.M02;
        M03 = matrix.M03;
        M10 = matrix.M10;
        M11 = matrix.M11;
        M12 = matrix.M12;
        M13 = matrix.M13;
        M20 = matrix.M20;
        M21 = matrix.M21;
        M22 = matrix.M22;
        M23 = matrix.M23;
        M30 = matrix.M30;
        M31 = matrix.M31;
        M32 = matrix.M32;
        M33 = matrix.M33;
    }

    public Vector2 Transform(Vector2 vector)
    {
        return new Vector2(
            M00 * vector.X + M10 * vector.Y + M30,
            M01 * vector.X + M11 * vector.Y + M31
        );
    }
    
    public Vector3 Transform(Vector3 vector)
    {
        return new Vector3(
            M00 * vector.X + M10 * vector.Y + M20 * vector.Z + M30,
            M01 * vector.X + M11 * vector.Y + M21 * vector.Z + M31,
            M02 * vector.X + M12 * vector.Y + M22 * vector.Z + M32 
        );
    }

    public Rect2 Transform(Rect2 box)
    {
        var v1 = Transform(box.TopRight);
        var v2 = Transform(box.BottomLeft);
        return new Rect2(v2.X, v1.Y, v1.X, v2.Y);
    }
    
    public Matrix4x4 Transpose(Matrix4x4 matrix4X4)
    {
        return new Matrix4x4(
            M00, M10, M20, M30,
            M01, M11, M21, M31,
            M02, M12, M22, M32,
            M03, M13, M23, M33
        );
    }

    public float[] ToArray()
    {
        return
        [
            M00, M01, M02, M03,
            M10, M11, M12, M13,
            M20, M21, M22, M23,
            M30, M31, M32, M33
        ];
    }

    public bool Equals(Matrix4x4 other)
    {
        return M00.AboutEquals(other.M00) &&
               M01.AboutEquals(other.M01) &&
               M02.AboutEquals(other.M02) &&
               M03.AboutEquals(other.M03) &&
               M10.AboutEquals(other.M10) &&
               M11.AboutEquals(other.M11) &&
               M12.AboutEquals(other.M12) &&
               M13.AboutEquals(other.M13) &&
               M20.AboutEquals(other.M20) &&
               M21.AboutEquals(other.M21) &&
               M22.AboutEquals(other.M22) &&
               M23.AboutEquals(other.M23) &&
               M30.AboutEquals(other.M30) &&
               M31.AboutEquals(other.M31) &&
               M32.AboutEquals(other.M32) &&
               M33.AboutEquals(other.M33);
    }

    [MustDisposeResource]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<float>) this).GetEnumerator();
    }

    IEnumerator<Vector4> IEnumerable<Vector4>.GetEnumerator()
    {
        yield return Row0;
        yield return Row1;
        yield return Row2;
        yield return Row3;
    }

    IEnumerator<float> IEnumerable<float>.GetEnumerator()
    {
        yield return M00;
        yield return M01;
        yield return M02;
        yield return M03;
        yield return M10;
        yield return M11;
        yield return M12;
        yield return M13;
        yield return M20;
        yield return M21;
        yield return M22;
        yield return M23;
        yield return M30;
        yield return M31;
        yield return M32;
        yield return M33;
    }

    public override bool Equals(object? obj)
    {
        return obj is Matrix4x4 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row0, Row1, Row2, Row3);
    }

    public override string ToString()
    {
        return $"{M00}, {M01}, {M02}, {M03}\n{M10}, {M11}, {M12}, {M13}\n{M20}, {M21}, {M22}, {M23}\n{M30}, {M31}, {M32}, {M33}";
    }

    public static bool operator ==(Matrix4x4 a, Matrix4x4 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Matrix4x4 a, Matrix4x4 b)
    {
        return !a.Equals(b);
    }
    
    public static Matrix4x4 operator +(Matrix4x4 a, Matrix4x4 b)
    {
        return new Matrix4x4(
            a.M00 + b.M00,
            a.M01 + b.M01,
            a.M02 + b.M02,
            a.M03 + b.M03,
            a.M10 + b.M10,
            a.M11 + b.M11,
            a.M12 + b.M12,
            a.M13 + b.M13,
            a.M20 + b.M20,
            a.M21 + b.M21,
            a.M22 + b.M22,
            a.M23 + b.M23,
            a.M30 + b.M30,
            a.M31 + b.M31,
            a.M32 + b.M32,
            a.M33 + b.M33
        );
    }

    public static Vector4 operator *(Matrix4x4 m, Vector4 v)
    {
        return new Vector4(
            m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z + m.M03 * v.W,
            m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z + m.M13 * v.W,
            m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z + m.M23 * v.W,
            m.M30 * v.X + m.M31 * v.Y + m.M32 * v.Z + m.M33 * v.W
        );
    }

    public static Matrix4x4 operator *(Matrix4x4 m, float s)
    {
        return new Matrix4x4(
            m.M00 * s,
            m.M01 * s,
            m.M02 * s,
            m.M03 * s,
            m.M10 * s,
            m.M11 * s,
            m.M12 * s,
            m.M13 * s,
            m.M20 * s,
            m.M21 * s,
            m.M22 * s,
            m.M23 * s,
            m.M30 * s,
            m.M31 * s,
            m.M32 * s,
            m.M33 * s
        );
    }

    public static Matrix4x4 operator *(Matrix4x4 m, Matrix4x4 b)
    {
        return new Matrix4x4(
            m.M00 * b.M00 + m.M01 * b.M10 + m.M02 * b.M20 + m.M03 * b.M30,
            m.M00 * b.M01 + m.M01 * b.M11 + m.M02 * b.M21 + m.M03 * b.M31,
            m.M00 * b.M02 + m.M01 * b.M12 + m.M02 * b.M22 + m.M03 * b.M32,
            m.M00 * b.M03 + m.M01 * b.M13 + m.M02 * b.M23 + m.M03 * b.M33,
            m.M10 * b.M00 + m.M11 * b.M10 + m.M12 * b.M20 + m.M13 * b.M30,
            m.M10 * b.M01 + m.M11 * b.M11 + m.M12 * b.M21 + m.M13 * b.M31,
            m.M10 * b.M02 + m.M11 * b.M12 + m.M12 * b.M22 + m.M13 * b.M32,
            m.M10 * b.M03 + m.M11 * b.M13 + m.M12 * b.M23 + m.M13 * b.M33,
            m.M20 * b.M00 + m.M21 * b.M10 + m.M22 * b.M20 + m.M23 * b.M30,
            m.M20 * b.M01 + m.M21 * b.M11 + m.M22 * b.M21 + m.M23 * b.M31,
            m.M20 * b.M02 + m.M21 * b.M12 + m.M22 * b.M22 + m.M23 * b.M32,
            m.M20 * b.M03 + m.M21 * b.M13 + m.M22 * b.M23 + m.M23 * b.M33,
            m.M30 * b.M00 + m.M31 * b.M10 + m.M32 * b.M20 + m.M33 * b.M30,
            m.M30 * b.M01 + m.M31 * b.M11 + m.M32 * b.M21 + m.M33 * b.M31,
            m.M30 * b.M02 + m.M31 * b.M12 + m.M32 * b.M22 + m.M33 * b.M32,
            m.M30 * b.M03 + m.M31 * b.M13 + m.M32 * b.M23 + m.M33 * b.M33
        );
    }
    
    public static Matrix4x4 CreateIdentity(Matrix3x3 matrix)
    {
        return new Matrix4x4(
            matrix.M00, matrix.M01, matrix.M02, 0,
            matrix.M10, matrix.M11, matrix.M12, 0,
            matrix.M20, matrix.M21, matrix.M22, 0,
            0, 0, 0, 1
        );
    }
    
    public static Matrix4x4 CreateTransform(Vector3 position, Quaternion quaternion, Vector3 scale)
    {
        return CreateScale(scale) * CreateRotation(quaternion) * CreateTranslation(position);
    }

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  v  |  0  |  0  |  0 
    ///  0  |  v  |  0  |  0
    ///  0  |  0  |  v  |  0
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateScale(float value)
    {
        return CreateScale(value, value, value);
    }

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |  0  |  0  |  0 
    ///  0  |  y  |  0  |  0
    ///  0  |  0  |  1  |  0
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateScale(Vector2 scale)
    {
        return CreateScale(scale.X, scale.Y, 1);
    }

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |  0  |  0  |  0 
    ///  0  |  y  |  0  |  0
    ///  0  |  0  |  z  |  0
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateScale(Vector3 scale)
    {
        return CreateScale(scale.X, scale.Y, scale.Z);
    }

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |  0  |  0  |  0 
    ///  0  |  y  |  0  |  0
    ///  0  |  0  |  z  |  0
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateScale(float x, float y, float z)
    {
        return new Matrix4x4(
            x, 0, 0, 0,
            0, y, 0, 0,
            0, 0, z, 0,
            0, 0, 0, 1
        );
    }
    
    public static Matrix4x4 CreateRotation(Quaternion quaternion)
    {
        var xx = quaternion.X * quaternion.X;
        var yy = quaternion.Y * quaternion.Y;
        var zz = quaternion.Z * quaternion.Z;
        var xy = quaternion.X * quaternion.Y;
        var wz = quaternion.Z * quaternion.W;
        var xz = quaternion.Z * quaternion.X;
        var wy = quaternion.Y * quaternion.W;
        var yz = quaternion.Y * quaternion.Z;
        var wx = quaternion.X * quaternion.W;
        
        return new Matrix4x4(
            1 - 2 * (yy + zz), 2 * (xy + wz), 2 * (xz - wy), 0,
            2 * (xy - wz), 1 - 2 * (zz + xx), 2 * (yz + wx), 0,
            2 * (xz + wy), 2 * (yz - wx), 1 - 2 * (yy + xx), 0,
            0, 0, 0, 1
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotation(Vector3 direction, float angle)
    {
        var cos = MathF.Cos(-angle);
        var sin = MathF.Sin(-angle);
        var t = 1.0f - cos;
        
        direction = direction.Normalized;

        return new Matrix4x4(
            t * direction.X * direction.X + cos,
            t * direction.X * direction.Y - sin * direction.Z,
            t * direction.X * direction.Z + sin * direction.Y,
            0,
            t * direction.X * direction.Y + sin * direction.Z,
            t * direction.Y * direction.Y + cos,
            t * direction.Y * direction.Z - sin * direction.X,
            0,
            t * direction.X * direction.Z - sin * direction.Y,
            t * direction.Y * direction.Z + sin * direction.X,
            t * direction.Z * direction.Z + cos,
            0,
            0,
            0,
            0,
            1
        );
    }

    /// <summary>
    /// Creating rotation axis X matrix
    /// <code>
    ///   1  |   0  |  0  |  0 
    ///   0  |  cos | sin |  0
    ///   0  | -sin | cos |  0
    ///   0  |   0  |  0  |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotationX(float angle)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        return new Matrix4x4(
            1, 0, 0, 0,
            0, cos, sin, 0,
            0, -sin, cos, 0,
            0, 0, 0, 1
        );
    }

    /// <summary>
    /// Creating rotation axis Y matrix
    /// <code>
    ///  cos |  0  | -sin  |  0 
    ///   0  |  1  |   0   |  0
    ///  sin |  0  |  cos  |  0
    ///   0  |  0  |   0   |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotationY(float angle)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        return new Matrix4x4(
            cos, 0, -sin, 0,
            0, 1, 0, 0,
            sin, 0, cos, 0,
            0, 0, 0, 1
        );
    }

    /// <summary>
    /// Creating rotation axis Z matrix
    /// <code>
    ///  cos | sin |  0  |  0 
    /// -sin | cos |  0  |  0
    ///   0  |  0  |  1  |  0
    ///   0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotationZ(float angle)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        return new Matrix4x4(
            cos, sin, 0, 0,
            -sin, cos, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        );
    }

    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  0 
    ///  0  |  1  |  0  |  0
    ///  0  |  0  |  1  |  0
    ///  v  |  v  |  v  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateTranslation(float v)
    {
        return new Matrix4x4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            v, v, v, 1
        );
    }
    
    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  0 
    ///  0  |  1  |  0  |  0
    ///  0  |  0  |  1  |  0
    ///  x  |  y  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateTranslation(float x, float y)
    {
        return new Matrix4x4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            x, y, 0, 1
        );
    }

    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  0 
    ///  0  |  1  |  0  |  0
    ///  0  |  0  |  1  |  0
    ///  x  |  y  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateTranslation(Vector2 vector)
    {
        return new Matrix4x4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            vector.X, vector.Y, 0, 1
        );
    }

    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  0 
    ///  0  |  1  |  0  |  0
    ///  0  |  0  |  1  |  0
    ///  x  |  y  |  z  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateTranslation(Vector3 vector)
    {
        return new Matrix4x4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            vector.X, vector.Y, vector.Z, 1
        );
    }

    /// <summary>
    /// Creating translate matrix. (column-major)
    /// <code>
    ///  1  |  0  |  0  |  0 
    ///  0  |  1  |  0  |  0
    ///  0  |  0  |  1  |  0
    ///  x  |  y  |  z  |  1
    /// </code>
    /// </summary>
    /// <remarks>
    /// In the course of development, I came across an interesting observation.
    /// There are two following conventions for the format of the matrix: row-major and column-major
    /// where we place the vector {x, y, z, 1} or in the last column and column accordingly.
    /// I realize the work with OpenGL and assume the work with it.
    ///
    /// But remember that:
    /// - OpenGL traditionally uses column-major
    /// - DirectX uses row-major
    /// </remarks>
    public static Matrix4x4 CreateTranslation(float x, float y, float z)
    {
        return new Matrix4x4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            x, y, z, 1
        );
    }

    public static Matrix4x4 CreateOrthographic(Rect2 rect2, float zNear, float zFar)
    {
        return CreateOrthographic(rect2.Width, rect2.Height, zNear, zFar);
    }
    
    public static Matrix4x4 CreateOrthographic(Vector2 size, float zNear, float zFar)
    {
        return CreateOrthographic(size.X, size.Y, zNear, zFar);
    }
    
    public static Matrix4x4 CreateOrthographic(float width, float height, float zNear, float zFar)
    {
        return CreateOrthographicOffCenter(-width / 2d, width / 2f, -height / 2d, height / 2f, zNear, zFar);
    }

    public static Matrix4x4 CreateOrthographicOffCenter(double left, float right, double bottom, float top, float zNear, float zFar)
    {
        return CreateOrthographicOffCenter((float) left, right, (float) bottom, top, zNear, zFar);
    }
    
    public static Matrix4x4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
    {
        var v1 = (float) (1d / ((double) right - left));
        var v2 = (float) (1d / ((double) top - bottom));
        var zD = (float) (1d / ((double) zFar - zNear));
        
        return new Matrix4x4(
            2f * v1, 0, 0, 0,
            0, 2f * v2, 0, 0,
            0, 0, -2f * zD, 0,
            (float) -((double) right + left) * v1, (float) -((double) top + bottom) * v2, (float) -((double) zFar + zNear) * zD, 1
        );
    }

    /// <summary>
    /// Creating perspective matrix
    /// <code>
    /// hFov = fov / 2;
    /// hFovX = hFov * aspect;
    ///
    /// 
    /// zDelta = zFar - zNear;
    /// z1 = zFar / zDelta;
    /// z2 = zFar * zNear / zDelta;
    /// 
    ///  cot(hFovX) |     0     |   0  |   0
    ///       0     | cot(hFov) |   0  |   0
    ///       0     |     0     |  z1  |   1
    ///       0     |     0     |  z2  |   0
    /// </code>
    /// </summary>
    public static Matrix4x4 CreatePerspective(float fov, float aspect, float zNear, float zFar)
    {
        var height = 1 / MathF.Tan(fov * 0.5f);
        var width = height / aspect;
        var range = float.IsPositiveInfinity(zFar) ? -1 : zFar / (zNear - zFar);
            
        return new Matrix4x4(
            width, 0, 0, 0,
            0, height, 0, 0,
            0, 0, range, -1,
            0, 0, range * zNear, 0
        );
    }
}
