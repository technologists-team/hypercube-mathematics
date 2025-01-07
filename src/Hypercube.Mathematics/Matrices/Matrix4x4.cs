﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Matrices;

[PublicAPI, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial struct Matrix4x4 : IEquatable<Matrix4x4>
{
    /// <summary>
    /// <code>
    ///   0  |  0  |  0  |  0
    ///   0  |  0  |  0  |  0
    ///   0  |  0  |  0  |  0
    ///   0  |  0  |  0  |  0
    /// </code>
    /// </summary>
    public static Matrix4x4 Zero => new(Vector4.Zero);
    
    /// <summary>
    /// <code>
    ///   1  |  1  |  1  |  1
    ///   1  |  1  |  1  |  1
    ///   1  |  1  |  1  |  1
    ///   1  |  1  |  1  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 One => new(Vector4.One);
    
    /// <summary>
    /// <code>
    ///   1  |  0  |  0  |  0
    ///   0  |  1  |  0  |  0
    ///   0  |  0  |  1  |  0
    ///   0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 Identity => new(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

    public readonly Vector4 Row0;
    public readonly Vector4 Row1;
    public readonly Vector4 Row2;
    public readonly Vector4 Row3;

    public Vector4 Column0 => new(M00, M10, M20, M30);
    public Vector4 Column1 => new(M01, M11, M21, M31);
    public Vector4 Column2 => new(M02, M12, M22, M32);
    public Vector4 Column3 => new(M03, M13, M23, M33);

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
    /// Matrix x: 3, y: 0 element.
    /// </summary>
    public float M03
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row0.W;
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
    /// Matrix x: 3, y: 1 element.
    /// </summary>
    public float M13
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row1.W;
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

    /// <summary>
    /// Matrix x: 3, y: 2 element.
    /// </summary>
    public float M23
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row2.W;
    }

    /// <summary>
    /// Matrix x: 0, y: 3 element.
    /// </summary>
    public float M30
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row3.X;
    }

    /// <summary>
    /// Matrix x: 1, y: 3 element.
    /// </summary>
    public float M31
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row3.Y;
    }

    /// <summary>
    /// Matrix x: 2, y: 3 element.
    /// </summary>
    public float M32
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row3.Z;
    }

    /// <summary>
    /// Matrix x: 3, y: 3 element.
    /// </summary>
    public float M33
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Row3.W;
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
        Row0 = x;
        Row1 = y;
        Row2 = z;
        Row3 = w;
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
        Row0 = value;
        Row1 = value;
        Row2 = value;
        Row3 = value;
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
    public Matrix4x4(
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33
    )
    {
        Row0 = new Vector4(m00, m01, m02, m03);
        Row1 = new Vector4(m10, m11, m12, m13);
        Row2 = new Vector4(m20, m21, m22, m23);
        Row3 = new Vector4(m30, m31, m32, m33);
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
        Row0 = new Vector4(matrix.Row0, 0);
        Row1 = new Vector4(matrix.Row1, 0);
        Row2 = new Vector4(matrix.Row2, 0);
        Row3 = Vector4.Zero;
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
        Row0 = matrix.Row0;
        Row1 = matrix.Row1;
        Row2 = matrix.Row2;
        Row3 = matrix.Row3;
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
        return Row0.Equals(other.Row0) &&
               Row1.Equals(other.Row1) &&
               Row2.Equals(other.Row2) &&
               Row3.Equals(other.Row3);
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
        return $"{Row0}\n{Row1}\n{Row2}\n{Row3}";
    }

    public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
    {
        return new Matrix4x4(
            (a.Row0 * b.Column0).Summation,
            (a.Row0 * b.Column1).Summation,
            (a.Row0 * b.Column2).Summation,
            (a.Row0 * b.Column3).Summation,
            (a.Row1 * b.Column0).Summation,
            (a.Row1 * b.Column1).Summation,
            (a.Row1 * b.Column2).Summation,
            (a.Row1 * b.Column3).Summation,
            (a.Row2 * b.Column0).Summation,
            (a.Row2 * b.Column1).Summation,
            (a.Row2 * b.Column2).Summation,
            (a.Row2 * b.Column3).Summation,
            (a.Row3 * b.Column0).Summation,
            (a.Row3 * b.Column1).Summation,
            (a.Row3 * b.Column2).Summation,
            (a.Row3 * b.Column3).Summation
        );
    }

    public static Vector4 operator *(Matrix4x4 a, Vector4 b)
    {
        return new Vector4(
            (a.Row0 * b).Summation,
            (a.Row1 * b).Summation,
            (a.Row2 * b).Summation,
            (a.Row3 * b).Summation);
    }

    public static bool operator ==(Matrix4x4 a, Matrix4x4 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Matrix4x4 a, Matrix4x4 b)
    {
        return !a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateIdentity(Matrix3x3 matrix3X3)
    {
        return new Matrix4x4(
            new Vector4(matrix3X3.Row0, 0),
            new Vector4(matrix3X3.Row1, 0),
            new Vector4(matrix3X3.Row2, 0),
            Vector4.UnitW);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Transpose(Matrix4x4 matrix4X4)
    {
        return new Matrix4x4(matrix4X4.Column0, matrix4X4.Column1, matrix4X4.Column2, matrix4X4.Column3);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateTransform(Vector3 position, Quaternion quaternion, Vector3 scale)
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

        var rx1 = (1.0f - 2.0f * (yy + zz)) * scale.X;
        var rx2 = 2.0f * (xy + wz) * scale.X;
        var rx3 = 2.0f * (xz - wy) * scale.X;
        var rx4 = rx1 * position.X + rx2 * position.X + rx3 * position.X;
        var ry1 = 2.0f * (xy - wz) * scale.Y;
        var ry2 = (1.0f - 2.0f * (zz + xx)) * scale.Y;
        var ry3 = 2.0f * (yz + wx) * scale.Y;
        var ry4 = ry1 * position.Y + ry2 * position.Y + ry3 * position.Y;
        var rz1 = 2.0f * (xz + wy) * scale.Z;
        var rz2 = 2.0f * (yz - wx) * scale.Z;
        var rz3 = (1.0f - 2.0f * (yy + xx)) * scale.Z;
        var rz4 = rz1 * position.Z + rz2 * position.Z + rz3 * position.Z;
        
        return new Matrix4x4(
            rx1, rx2, rx3, rx4,
            ry1, ry2, ry3, ry4,
            rz1, rz2, rz3, rz4,
            0, 0, 0, 1
        );
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
        return CreateScale(scale.X, scale.Y, 1f);
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

    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  v  |  0  |  0  |  0 
    ///  0  |  v  |  0  |  0
    ///  0  |  0  |  v  |  0
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateScaleY(float value)
    {
        return new Matrix4x4(
            value, 0, 0, 0,
            0, value, 0, 0,
            0, 0, value, 0,
            0, 0, 0, 1
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        var x = new Vector4(
            1.0f - 2.0f * (yy + zz),
            2.0f * (xy + wz),
            2.0f * (xz - wy),
            0
        );
        
        var y = new Vector4(
            2.0f * (xy - wz),
            1.0f - 2.0f * (zz + xx),
            2.0f * (yz + wx),
            0
        );
        
        var z = new Vector4(
            2.0f * (xz + wy),
            2.0f * (yz - wx),
            1.0f - 2.0f * (yy + xx),
            0
        );
        
        return new Matrix4x4(x, y, z, Vector4.UnitW);
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
            Vector4.UnitX,
            new Vector4(0, cos, sin, 0),
            new Vector4(0, -sin, cos, 0),
            Vector4.UnitW
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
            new Vector4(cos, 0, -sin, 0),
            Vector4.UnitY,
            new Vector4(sin, 0, cos, 0),
            Vector4.UnitW
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
            new Vector4(cos, sin, 0, 0),
            new Vector4(-sin, cos, 0, 0),
            Vector4.UnitZ,
            Vector4.UnitW
        );
    }

    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  v 
    ///  0  |  1  |  0  |  v
    ///  0  |  0  |  1  |  v
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateTranslation(float value)
    {
        return CreateTranslation(value, value, value);
    }
    
    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  x 
    ///  0  |  1  |  0  |  y
    ///  0  |  0  |  1  |  0
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateTranslation(Vector2 vector2)
    {
        return CreateTranslation(vector2.X, vector2.Y, 0f);
    }
    
    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  x 
    ///  0  |  1  |  0  |  y
    ///  0  |  0  |  1  |  z
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateTranslation(Vector3 vector3)
    {
        return CreateTranslation(vector3.X, vector3.Y, vector3.Z);
    }
    
    /// <summary>
    /// Creating translate matrix
    /// <code>
    ///  1  |  0  |  0  |  x 
    ///  0  |  1  |  0  |  y
    ///  0  |  0  |  1  |  z
    ///  0  |  0  |  0  |  1
    /// </code>
    /// </summary>
    public static Matrix4x4 CreateTranslation(float x, float y, float z)
    {
        return new Matrix4x4(
            1, 0, 0, x,
            0, 1, 0, y,
            0, 0, 1, z,
            0, 0, 0, 1
        );
    }
    
    public Vector2 Transform(Vector2 vector2)
    {
        return Transform(this, vector2);
    }

    public static Vector2 Transform(Matrix4x4 matrix, Vector2 vector)
    {
        var x1 = matrix.M00 * vector.X + matrix.M01 * vector.Y + matrix.M02 * 0 + matrix.M03 * 1f;
        var y1 = matrix.M10 * vector.X + matrix.M11 * vector.Y + matrix.M12 * 0 + matrix.M13 * 1f;
        
        return new Vector2(x1, y1);
    }

    public Box2 Transform(Box2 quad)
    {
        return Transform(this, quad);
    }
    
    public static Box2 Transform(Matrix4x4 matrix, Box2 quad)
    {
        var tr = Transform(matrix, quad.TopRight);
        var bl = Transform(matrix, quad.BottomLeft);

        return new Box2(bl.X, tr.Y, tr.X, bl.Y);
    }

    public static Matrix4x4 CreateOrthographic(Box2 box2, float zNear, float zFar)
    {
        return CreateOrthographic(box2.Width, box2.Height, zNear, zFar);
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
        var height = 1.0f / MathF.Tan(fov * 0.5f);
        var width = height / aspect;
        var range = float.IsPositiveInfinity(zFar) ? -1.0f : zFar / (zNear - zFar);
            
        return new Matrix4x4(
            width, 0, 0, 0,
            0, height, 0, 0,
            0, 0, range, -1f,
            0, 0, range * zNear, 0
        );
    }
}
