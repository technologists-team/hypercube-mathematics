using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Matrices;

/// <summary>
/// Implementation of a 5x5 matrix for rendering work. (COLUM-MAJOR)
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Matrix5x5 : IEquatable<Matrix5x5>, IEnumerable<float>
{
    #region Static

    public static readonly Matrix5x5 NaN = new(
        float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN, float.NaN, float.NaN
    );

    public static readonly Matrix5x5 Zero = new(
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0
    );

    public static readonly Matrix5x5 One = new(
        1,1,1,1,1,
        1,1,1,1,1,
        1,1,1,1,1,
        1,1,1,1,1,
        1,1,1,1,1
    );

    public static readonly Matrix5x5 Identity = new(
        1,0,0,0,0,
        0,1,0,0,0,
        0,0,1,0,0,
        0,0,0,1,0,
        0,0,0,0,1
    );

    #endregion

    #region Fields (Column-Major semantic, Mxy)

    public readonly float M00, M01, M02, M03, M04;
    public readonly float M10, M11, M12, M13, M14;
    public readonly float M20, M21, M22, M23, M24;
    public readonly float M30, M31, M32, M33, M34;
    public readonly float M40, M41, M42, M43, M44;

    #endregion

    #region Rows / Columns

    public Vector4 Row0 => new(M00, M01, M02, M03);
    public Vector4 Row1 => new(M10, M11, M12, M13);
    public Vector4 Row2 => new(M20, M21, M22, M23);
    public Vector4 Row3 => new(M30, M31, M32, M33);

    #endregion

    #region Ctor

    public Matrix5x5(
        float m00, float m01, float m02, float m03, float m04,
        float m10, float m11, float m12, float m13, float m14,
        float m20, float m21, float m22, float m23, float m24,
        float m30, float m31, float m32, float m33, float m34,
        float m40, float m41, float m42, float m43, float m44)
    {
        M00 = m00; M01 = m01; M02 = m02; M03 = m03; M04 = m04;
        M10 = m10; M11 = m11; M12 = m12; M13 = m13; M14 = m14;
        M20 = m20; M21 = m21; M22 = m22; M23 = m23; M24 = m24;
        M30 = m30; M31 = m31; M32 = m32; M33 = m33; M34 = m34;
        M40 = m40; M41 = m41; M42 = m42; M43 = m43; M44 = m44;
    }

    #endregion

    #region Indexer

    public float this[int x, int y] => y switch
    {
        0 => x switch { 0 => M00, 1 => M01, 2 => M02, 3 => M03, 4 => M04, _ => throw new ArgumentOutOfRangeException(nameof(x)) },
        1 => x switch { 0 => M10, 1 => M11, 2 => M12, 3 => M13, 4 => M14, _ => throw new ArgumentOutOfRangeException(nameof(x)) },
        2 => x switch { 0 => M20, 1 => M21, 2 => M22, 3 => M23, 4 => M24, _ => throw new ArgumentOutOfRangeException(nameof(x)) },
        3 => x switch { 0 => M30, 1 => M31, 2 => M32, 3 => M33, 4 => M34, _ => throw new ArgumentOutOfRangeException(nameof(x)) },
        4 => x switch { 0 => M40, 1 => M41, 2 => M42, 3 => M43, 4 => M44, _ => throw new ArgumentOutOfRangeException(nameof(x)) },
        _ => throw new ArgumentOutOfRangeException(nameof(y))
    };

    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix5x5 operator *(Matrix5x5 a, Matrix5x5 b)
    {
        return new Matrix5x5(
            a.M00*b.M00 + a.M01*b.M10 + a.M02*b.M20 + a.M03*b.M30 + a.M04*b.M40,
            a.M00*b.M01 + a.M01*b.M11 + a.M02*b.M21 + a.M03*b.M31 + a.M04*b.M41,
            a.M00*b.M02 + a.M01*b.M12 + a.M02*b.M22 + a.M03*b.M32 + a.M04*b.M42,
            a.M00*b.M03 + a.M01*b.M13 + a.M02*b.M23 + a.M03*b.M33 + a.M04*b.M43,
            a.M00*b.M04 + a.M01*b.M14 + a.M02*b.M24 + a.M03*b.M34 + a.M04*b.M44,

            a.M10*b.M00 + a.M11*b.M10 + a.M12*b.M20 + a.M13*b.M30 + a.M14*b.M40,
            a.M10*b.M01 + a.M11*b.M11 + a.M12*b.M21 + a.M13*b.M31 + a.M14*b.M41,
            a.M10*b.M02 + a.M11*b.M12 + a.M12*b.M22 + a.M13*b.M32 + a.M14*b.M42,
            a.M10*b.M03 + a.M11*b.M13 + a.M12*b.M23 + a.M13*b.M33 + a.M14*b.M43,
            a.M10*b.M04 + a.M11*b.M14 + a.M12*b.M24 + a.M13*b.M34 + a.M14*b.M44,

            a.M20*b.M00 + a.M21*b.M10 + a.M22*b.M20 + a.M23*b.M30 + a.M24*b.M40,
            a.M20*b.M01 + a.M21*b.M11 + a.M22*b.M21 + a.M23*b.M31 + a.M24*b.M41,
            a.M20*b.M02 + a.M21*b.M12 + a.M22*b.M22 + a.M23*b.M32 + a.M24*b.M42,
            a.M20*b.M03 + a.M21*b.M13 + a.M22*b.M23 + a.M23*b.M33 + a.M24*b.M43,
            a.M20*b.M04 + a.M21*b.M14 + a.M22*b.M24 + a.M23*b.M34 + a.M24*b.M44,

            a.M30*b.M00 + a.M31*b.M10 + a.M32*b.M20 + a.M33*b.M30 + a.M34*b.M40,
            a.M30*b.M01 + a.M31*b.M11 + a.M32*b.M21 + a.M33*b.M31 + a.M34*b.M41,
            a.M30*b.M02 + a.M31*b.M12 + a.M32*b.M22 + a.M33*b.M32 + a.M34*b.M42,
            a.M30*b.M03 + a.M31*b.M13 + a.M32*b.M23 + a.M33*b.M33 + a.M34*b.M43,
            a.M30*b.M04 + a.M31*b.M14 + a.M32*b.M24 + a.M33*b.M34 + a.M34*b.M44,

            a.M40*b.M00 + a.M41*b.M10 + a.M42*b.M20 + a.M43*b.M30 + a.M44*b.M40,
            a.M40*b.M01 + a.M41*b.M11 + a.M42*b.M21 + a.M43*b.M31 + a.M44*b.M41,
            a.M40*b.M02 + a.M41*b.M12 + a.M42*b.M22 + a.M43*b.M32 + a.M44*b.M42,
            a.M40*b.M03 + a.M41*b.M13 + a.M42*b.M23 + a.M43*b.M33 + a.M44*b.M43,
            a.M40*b.M04 + a.M41*b.M14 + a.M42*b.M24 + a.M43*b.M34 + a.M44*b.M44
        );
    }

    #endregion

    #region Transform

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 Transform(Vector4 v)
    {
        var x = M00 * v.X + M01 * v.Y + M02 * v.Z + M03 * v.W + M04;
        var y = M10 * v.X + M11 * v.Y + M12 * v.Z + M13 * v.W + M14;
        var z = M20 * v.X + M21 * v.Y + M22 * v.Z + M23 * v.W + M24;
        var w = M30 * v.X + M31 * v.Y + M32 * v.Z + M33 * v.W + M34;
        var h = M40 * v.X + M41 * v.Y + M42 * v.Z + M43 * v.W + M44;

        if (h.AboutEquals(0f) || h.AboutEquals(1f))
            return new Vector4(x, y, z, w);
        
        var inv = 1f / h;
        return new Vector4(x * inv, y * inv, z * inv, w * inv);
    }

    #endregion

    #region 4D Rotations (SO(4))

    public static Matrix5x5 CreateRotationXY(float a)
    {
        var c = float.Cos(a);
        var s = float.Sin(a);

        return new Matrix5x5(
            c, -s, 0, 0, 0,
            s,  c, 0, 0, 0,
            0,  0, 1, 0, 0,
            0,  0, 0, 1, 0,
            0,  0, 0, 0, 1
        );
    }

    public static Matrix5x5 CreateRotationXW(float a)
    {
        var c = float.Cos(a);
        var s = float.Sin(a);

        return new Matrix5x5(
            c, 0, 0, -s, 0,
            0, 1, 0,  0, 0,
            0, 0, 1,  0, 0,
            s, 0, 0,  c, 0,
            0, 0, 0,  0, 1
        );
    }

    public static Matrix5x5 CreateRotationYZ(float a)
    {
        var c = float.Cos(a);
        var s = float.Sin(a);

        return new Matrix5x5(
            1, 0,  0, 0, 0,
            0, c, -s, 0, 0,
            0, s,  c, 0, 0,
            0, 0,  0, 1, 0,
            0, 0,  0, 0, 1
        );
    }

    public static Matrix5x5 CreateRotationZW(float a)
    {
        var c = float.Cos(a);
        var s = float.Sin(a);

        return new Matrix5x5(
            1, 0, 0,  0, 0,
            0, 1, 0,  0, 0,
            0, 0, c, -s, 0,
            0, 0, s,  c, 0,
            0, 0, 0,  0, 1
        );
    }

    #endregion

    #region Equality / ToString

    public bool Equals(Matrix5x5 other)
    {
        return M00.AboutEquals(other.M00) &&
               M11.AboutEquals(other.M11) &&
               M22.AboutEquals(other.M22) &&
               M33.AboutEquals(other.M33) &&
               M44.AboutEquals(other.M44);
    }

    public override string ToString()
    {
        return $"{M00}, {M01}, {M02}, {M03}, {M04}, ...";
    }

    public override bool Equals(object? obj)
    {
        return obj is Matrix5x5 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(M00, M11, M22, M33, M44);
    }

    #endregion

    #region IEnumerable

    [MustDisposeResource]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<float>) this).GetEnumerator();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<float> IEnumerable<float>.GetEnumerator()
    {
        yield return M00; yield return M01; yield return M02; yield return M03; yield return M04;
        yield return M10; yield return M11; yield return M12; yield return M13; yield return M14;
        yield return M20; yield return M21; yield return M22; yield return M23; yield return M24;
        yield return M30; yield return M31; yield return M32; yield return M33; yield return M34;
        yield return M40; yield return M41; yield return M42; yield return M43; yield return M44;
    }

    public static bool operator ==(Matrix5x5 left, Matrix5x5 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Matrix5x5 left, Matrix5x5 right)
    {
        return !(left == right);
    }

    #endregion
}