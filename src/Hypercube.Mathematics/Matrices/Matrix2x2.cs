using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Matrices.Utilities;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Matrices;

/// <summary>
/// Implementation of a 2x2 matrix for linear transformations. (COLUMN-MAJOR)
/// </summary>
/// <remarks>
/// Column-major matrix. Column vectors. OpenGL convention.
/// Vector is multiplied as: M * v
/// </remarks>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Matrix2x2 : IMatrix, IMatrixSquare, IEquatable<Matrix2x2>, IEnumerable<Vector2>,
    IEnumerable<float>
{
    public const int Length = Dimensionality * Dimensionality;
    public const int Dimensionality = 2;

    #region Constants

    public static readonly Matrix2x2 NaN = new(
        float.NaN, float.NaN,
        float.NaN, float.NaN
    );

    public static readonly Matrix2x2 Zero = new(
        0, 0,
        0, 0
    );

    public static readonly Matrix2x2 One = new(
        1, 1,
        1, 1
    );

    public static readonly Matrix2x2 Identity = new(
        1, 0,
        0, 1
    );

    #endregion

    #region Fields

    // Column 0
    public readonly float M00;
    public readonly float M10;

    // Column 1
    public readonly float M01;
    public readonly float M11;

    #endregion

    #region Rows / Columns

    public Vector2 Row0 => new(M00, M01);
    public Vector2 Row1 => new(M10, M11);

    public Vector2 Column0 => new(M00, M10);
    public Vector2 Column1 => new(M01, M11);

    #endregion

    public Matrix2x2 Transposed => new(M00, M10, M01, M11);

    public float[] Array => Span.ToArray();
    public ReadOnlySpan<float> Span => UnsafeSpan;
    public Span<float> UnsafeSpan => MatrixMath.ToSpan(in Unsafe.AsRef(in M00), Length);

    public float this[int index] => Get(index);
    public float this[int x, int y] => Get(x, y);

    #region Constructors

    public Matrix2x2(float m00, float m01, float m10, float m11)
    {
        M00 = m00;
        M01 = m01;
        M10 = m10;
        M11 = m11;
    }

    public Matrix2x2(Vector2 row0, Vector2 row1)
    {
        M00 = row0.X;
        M01 = row0.Y;
        M10 = row1.X;
        M11 = row1.Y;
    }

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Get(int index) => MatrixMath.SquareGet(index, in M00, Length);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Get(int x, int y) => MatrixMath.SquareGet(x, y, in M00, Dimensionality);

    #region Equality / ToString

    public override bool Equals(object? obj) => obj is Matrix2x2 other && Equals(other);

    public bool Equals(Matrix2x2 other)
    {
        return M00.Equals(other.M00) && M01.Equals(other.M01) &&
               M10.Equals(other.M10) && M11.Equals(other.M11);
    }

    public override int GetHashCode() => HashCode.Combine(M00, M01, M10, M11);

    public override string ToString() => $"{M00}, {M01}, {M10}, {M11}";

    #endregion

    #region Math

    public float Determinant() => M00 * M11 - M01 * M10;

    public Matrix2x2 Inverted()
    {
        var det = Determinant();
        if (float.Abs(det) < float.Epsilon)
            throw new InvalidOperationException("Matrix is singular.");

        var invDet = 1.0f / det;
        return new Matrix2x2(
             M11 * invDet, -M01 * invDet,
            -M10 * invDet,  M00 * invDet
        );
    }

    #endregion

    #region Creation

    #region Scale

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix2x2 CreateScale(float scale) => CreateScale(scale, scale);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix2x2 CreateScale(Vector2 scale) => CreateScale(scale.X, scale.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix2x2 CreateScale(float x, float y)
    {
        return new Matrix2x2(
            x, 0,
            0, y
        );
    }

    #endregion

    #region Rotation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix2x2 CreateRotation(Angle angle) => CreateRotation((float) angle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix2x2 CreateRotation(float angle)
    {
        var cos = float.Cos(angle);
        var sin = float.Sin(angle);
        return new Matrix2x2(
            cos, -sin,
            sin, cos
        );
    }

    #endregion

    #endregion

    #region Operations

    public static Vector2 operator *(Matrix2x2 m, Vector2 v)
    {
        return new Vector2(
            m.M00 * v.X + m.M01 * v.Y,
            m.M10 * v.X + m.M11 * v.Y
        );
    }

    public static Matrix2x2 operator *(Matrix2x2 a, Matrix2x2 b)
    {
        return new Matrix2x2(
            a.M00 * b.M00 + a.M01 * b.M10, a.M00 * b.M01 + a.M01 * b.M11,
            a.M10 * b.M00 + a.M11 * b.M10, a.M10 * b.M01 + a.M11 * b.M11
        );
    }

    #endregion

    #region IEnumerable

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<float>) this).GetEnumerator();

    IEnumerator<Vector2> IEnumerable<Vector2>.GetEnumerator()
    {
        yield return Row0;
        yield return Row1;
    }

    IEnumerator<float> IEnumerable<float>.GetEnumerator()
    {
        yield return M00;
        yield return M01;
        yield return M10;
        yield return M11;
    }

    #endregion
}