using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using JetBrains.Annotations;
using Vector2 = Hypercube.Mathematics.Vectors.Vector2;
using Vector3 = Hypercube.Mathematics.Vectors.Vector3;

namespace Hypercube.Mathematics.Matrices;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly partial struct Matrix3x2 : IEquatable<Matrix3x2>, IEnumerable<Vector2>, IEnumerable<float>
{
    /// <value>
    /// <code>
    ///  NaN, NaN
    ///  NaN, NaN
    ///  NaN, NaN
    /// </code>
    /// </value>
    public static readonly Matrix3x2 NaN = new(float.NaN, float.NaN,float.NaN, float.NaN, float.NaN, float.NaN);
    
    /// <value>
    /// <code>
    ///  0, 0
    ///  0, 0
    ///  0, 0
    /// </code>
    /// </value>
    public static readonly Matrix3x2 Zero = new(0, 0, 0, 0, 0, 0);
    
    /// <value>
    /// <code>
    ///  1, 1
    ///  1, 1
    ///  1, 1   
    /// </code>
    /// </value>
    public static readonly Matrix3x2 One = new(1, 1, 1, 1, 1, 1);
    
    /// <value>
    /// <code>
    ///  1, 0
    ///  0, 1
    ///  0, 0   
    /// </code>
    /// </value>
    public static readonly Matrix3x2 Identity = new(1, 0, 0, 1, 0, 0);

    public Vector2 Row0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M00, M01);
    }

    public Vector2 Row1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M10, M11);
    }

    public Vector2 Row2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M20, M21);
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

    /// <summary>
    /// Matrix x: 0, y: 0 element.
    /// </summary>
    public readonly float M00;
    
    /// <summary>
    /// Matrix x: 0, y: 1 element.
    /// </summary>
    public readonly float M01;
    
    /// <summary>
    /// Matrix x: 1, y: 0 element.
    /// </summary>
    public readonly float M10;
    
    /// <summary>
    /// Matrix x: 1, y: 1 element.
    /// </summary>
    public readonly float M11;
    
    /// <summary>
    /// Matrix x: 2, y: 0 element.
    /// </summary>
    public readonly float M20;
    
    /// <summary>
    /// Matrix x: 2, y: 1 element.
    /// </summary>
    public readonly float M21;
    
    public Matrix3x2(float m00, float m01, float m10, float m11, float m20, float m21)
    {
        M00 = m00;
        M01 = m01;
        M10 = m10;
        M11 = m11;
        M20 = m20;
        M21 = m21;
    }
    
    public Matrix3x2(Vector2 row0, Vector2 row1, Vector2 row2)
    {
        M00 = row0.X;
        M01 = row0.Y;
        M10 = row1.X;
        M11 = row1.Y;
        M20 = row2.X;
        M21 = row2.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out float m00, out float m01, out float m10, out float m11, out float m20, out float m21)
    {
        m00 = M00;
        m01 = M01;
        m10 = M10;
        m11 = M11;
        m20 = M20;
        m21 = M21;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out Vector2 row0, out Vector2 row1, out Vector2 row2)
    {
        row0 = Row0;
        row1 = Row1;
        row2 = Row2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Matrix3x2 other)
    {
        return M00.AboutEquals(other.M00) && M01.AboutEquals(other.M01) &&
               M10.AboutEquals(other.M10) && M11.AboutEquals(other.M11) &&
               M20.AboutEquals(other.M20) && M21.AboutEquals(other.M21);
    }

    [MustDisposeResource]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<float>) this).GetEnumerator();
    }
    
    IEnumerator<Vector2> IEnumerable<Vector2>.GetEnumerator()
    {
        yield return Row0;
        yield return Row1;
        yield return Row2;
    }

    IEnumerator<float> IEnumerable<float>.GetEnumerator()
    {
        yield return M00;
        yield return M01;
        yield return M10;
        yield return M11;
        yield return M20;
        yield return M21;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Matrix3x2 m && Equals(m);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(M00, M01, M10, M11, M20, M21);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"{M00}, {M01}, {M10}, {M11}, {M20}, {M21}";
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Matrix3x2 a, Matrix3x2 b)
    {
        return a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Matrix3x2 a, Matrix3x2 b)
    {
        return !a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x2 operator *(Matrix3x2 a, Matrix3x2 b)
    {
        return new Matrix3x2(
            a.M00 * b.M00 + a.M01 * b.M10,
            a.M00 * b.M01 + a.M01 * b.M11,
            a.M10 * b.M00 + a.M11 * b.M10,
            a.M10 * b.M01 + a.M11 * b.M11,
            a.M20 * b.M00 + a.M21 * b.M10 + b.M20,
            a.M20 * b.M01 + a.M21 * b.M11 + b.M21
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Matrix3x2 a, Vector2 b)
    {
        return new Vector2(
            b.X * a.M00 + b.Y * a.M10 + a.M20,
            b.X * a.M01 + b.Y * a.M11 + a.M21
        );
    }
    
    /// <summary>
    /// Creating translation matrix
    /// <code>
    ///  1  |  0
    ///  0  |  1
    ///  x  |  y 
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x2 CreateTranslation(Vector2 translation)
    {
        return CreateTranslation(translation.X, translation.Y);
    }

    /// <summary>
    /// Creating translation matrix
    /// <code>
    ///  1  |  0
    ///  0  |  1
    ///  x  |  y 
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x2 CreateTranslation(float x, float y)
    {
        return new Matrix3x2(1, 0, 0, 1, x, y);
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
        var cos = (float) double.Cos(angle);
        var sin = (float) double.Sin(angle);
        return new Matrix3x2(cos, sin, -sin, cos, 0, 0);
    }
    
    /// <summary>
    /// Creating scale matrix
    /// <code>
    ///  x  |  0
    ///  0  |  y
    ///  0  |  0 
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
    ///  x  |  0
    ///  0  |  y
    ///  0  |  0 
    /// </code>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x2 CreateScale(float x, float y)
    {
        return new Matrix3x2(x, 0, 0, y, 0, 0);
    }
}