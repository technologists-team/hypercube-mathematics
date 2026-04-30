using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using Hypercube.Mathematics.Matrices.Utilities;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Matrices;

/// <summary>
/// Implementation of a 4x4 matrix for rendering work. (ROW-MAJOR)
/// </summary>
/// <remarks>
/// Row-major matrix (M[row, col]).
/// Stored in row-major memory layout.
/// Vector multiplication uses: v' = v * M
/// </remarks>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Matrix4x4 : IMatrix, IMatrixSquare, IEquatable<Matrix4x4>, IEnumerable<Vector4>, IEnumerable<float>, IFormattable
{
    /// <summary>
    /// The total number of elements in the matrix (4x4=16).
    /// </summary>
    public const int Length = Dimensionality * Dimensionality;
    
    /// <summary>
    /// The number of rows and columns in the matrix.
    /// </summary>
    public const int Dimensionality = 4;

    #region Constants
    
    /// <summary>
    /// A matrix where all elements are <see cref="float.NaN"/>.
    /// <code>
    /// NaN, NaN, NaN, NaN,
    /// NaN, NaN, NaN, NaN,
    /// NaN, NaN, NaN, NaN,
    /// NaN, NaN, NaN, NaN
    /// </code>
    /// </summary>
    public static readonly Matrix4x4 NaN = new(
        float.NaN, float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN, float.NaN
    );
    
    /// <summary>
    /// A matrix where all elements are zero.
    /// <code>
    /// 0, 0, 0, 0,
    /// 0, 0, 0, 0,
    /// 0, 0, 0, 0,
    /// 0, 0, 0, 0
    /// </code>
    /// </summary>
    public static readonly Matrix4x4 Zero = new(
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0
    );
    
    /// <summary>
    /// A matrix where all elements are one.
    /// <code>
    /// 1, 1, 1, 1,
    /// 1, 1, 1, 1,
    /// 1, 1, 1, 1,
    /// 1, 1, 1, 1
    /// </code>
    /// </summary>
    public static readonly Matrix4x4 One = new(
        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1
    );
    
    /// <summary>
    /// The identity matrix.
    /// <code>
    /// 1, 0, 0, 0,
    /// 0, 1, 0, 0,
    /// 0, 0, 1, 0,
    /// 0, 0, 0, 1
    /// </code>
    /// </summary>
    public static readonly Matrix4x4 Identity = new(
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );
    
    #endregion
    
    #region Fields

    /// <summary>
    /// Matrix row 0, column 0 element.
    /// </summary>
    public readonly float M00;

    /// <summary>
    /// Matrix row 0, column 1 element.
    /// </summary>
    public readonly float M01;

    /// <summary>
    /// Matrix row 0, column 2 element.
    /// </summary>
    public readonly float M02;

    /// <summary>
    /// Matrix row 0, column 3 element.
    /// </summary>
    public readonly float M03;

    /// <summary>
    /// Matrix row 1, column 0 element.
    /// </summary>
    public readonly float M10;

    /// <summary>
    /// Matrix row 1, column 1 element.
    /// </summary>
    public readonly float M11;

    /// <summary>
    /// Matrix row 1, column 2 element.
    /// </summary>
    public readonly float M12;

    /// <summary>
    /// Matrix row 1, column 3 element.
    /// </summary>
    public readonly float M13;

    /// <summary>
    /// Matrix row 2, column 0 element.
    /// </summary>
    public readonly float M20;

    /// <summary>
    /// Matrix row 2, column 1 element.
    /// </summary>
    public readonly float M21;

    /// <summary>
    /// Matrix row 2, column 2 element.
    /// </summary>
    public readonly float M22;

    /// <summary>
    /// Matrix row 2, column 3 element.
    /// </summary>
    public readonly float M23;

    /// <summary>
    /// Matrix row 3, column 0 element.
    /// </summary>
    public readonly float M30;

    /// <summary>
    /// Matrix row 3, column 1 element.
    /// </summary>
    public readonly float M31;

    /// <summary>
    /// Matrix row 3, column 2 element.
    /// </summary>
    public readonly float M32;

    /// <summary>
    /// Matrix row 3, column 3 element.
    /// </summary>
    public readonly float M33;
    
    #endregion
    
    #region Rows / Columns
    
    /// <summary>
    /// Gets the first row (index 0) of the matrix.
    /// </summary>
    public Vector4 Row0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M00, M01, M02, M03);
    }

    /// <summary>
    /// Gets the second row (index 1) of the matrix.
    /// </summary>
    public Vector4 Row1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M10, M11, M12, M13);
    }

    /// <summary>
    /// Gets the third row (index 2) of the matrix.
    /// </summary>
    public Vector4 Row2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M20, M21, M22, M23);
    }

    /// <summary>
    /// Gets the fourth row (index 3) of the matrix.
    /// </summary>
    public Vector4 Row3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M30, M31, M32, M33);
    }
    
    /// <summary>
    /// Gets the first column (index 0) of the matrix.
    /// </summary>
    public Vector4 Column0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M00, M10, M20, M30);
    }

    /// <summary>
    /// Gets the second column (index 1) of the matrix.
    /// </summary>
    public Vector4 Column1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M01, M11, M21, M31);
    }

    /// <summary>
    /// Gets the third column (index 2) of the matrix.
    /// </summary>
    public Vector4 Column2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M02, M12, M22, M32);
    }

    /// <summary>
    /// Gets the fourth column (index 3) of the matrix.
    /// </summary>
    public Vector4 Column3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M03, M13, M23, M33);
    }
    
    #endregion
    
    /// <summary>
    /// Returns a new matrix that is the transpose of this matrix.
    /// </summary>
    public Matrix4x4 Transposed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(
            M00, M10, M20, M30,
            M01, M11, M21, M31,
            M02, M12, M22, M32,
            M03, M13, M23, M33
        );
    }
    
    /// <summary>
    /// Returns a new array containing the matrix elements in row-major order.
    /// </summary>
    public float[] Array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Span.ToArray();
    }
    
    /// <summary>
    /// Returns a read-only span that wraps the matrix elements in row-major order.
    /// </summary>
    public ReadOnlySpan<float> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => UnsafeSpan;
    }

    /// <summary>
    /// Returns a mutable <see cref="Span{float}"/> pointing directly to the matrix memory.
    /// </summary>
    /// <remarks>
    /// <b>WARNING:</b> This bypasses the readonly constraint.
    /// <para>
    /// For safe, read-only access, use <see cref="Span"/> instead.
    /// </para>
    /// </remarks>
    public Span<float> UnsafeSpan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MatrixMath.ToSpan(in Unsafe.AsRef(in M00), Length);
    }
    
    /// <summary>
    /// Gets the element at the specified linear index (row-major).
    /// </summary>
    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(index);
    }

    /// <summary>
    /// Gets the element at the specified row and column.
    /// </summary>
    public float this[int x, int y]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(x, y);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix4x4"/> struct.
    /// <code>
    /// m00, m01, m02, m03, 
    /// m10, m11, m12, m13,
    /// m20, m21, m22, m23,
    /// m30, m31, m32, m33
    /// </code>
    /// </summary>
    public Matrix4x4(
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33)
    {
        M00 = m00; M01 = m01; M02 = m02; M03 = m03;
        M10 = m10; M11 = m11; M12 = m12; M13 = m13;
        M20 = m20; M21 = m21; M22 = m22; M23 = m23;
        M30 = m30; M31 = m31; M32 = m32; M33 = m33;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix4x4"/> struct from row vectors.
    /// </summary>
    public Matrix4x4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
    {
        M00 = row0.X; M01 = row0.Y; M02 = row0.Z; M03 = row0.W;
        M10 = row1.X; M11 = row1.Y; M12 = row1.Z; M13 = row1.W;
        M20 = row2.X; M21 = row2.Y; M22 = row2.Z; M23 = row2.W;
        M30 = row3.X; M31 = row3.Y; M32 = row3.Z; M33 = row3.W;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix4x4"/> struct from a <see cref="Matrix3x3"/>.
    /// </summary>
    public Matrix4x4(Matrix3x3 matrix)
    {
        M00 = matrix.M00; M01 = matrix.M01; M02 = matrix.M02; M03 = 0;
        M10 = matrix.M10; M11 = matrix.M11; M12 = matrix.M12; M13 = 0;
        M20 = matrix.M20; M21 = matrix.M21; M22 = matrix.M22; M23 = 0;
        M30 = 0; M31 = 0; M32 = 0; M33 = 1;
    }
    
    /// <summary>
    /// Initializes a new copy of a <see cref="Matrix4x4"/>.
    /// </summary>
    public Matrix4x4(Matrix4x4 matrix)
    {
        M00 = matrix.M00; M01 = matrix.M01; M02 = matrix.M02; M03 = matrix.M03;
        M10 = matrix.M10; M11 = matrix.M11; M12 = matrix.M12; M13 = matrix.M13;
        M20 = matrix.M20; M21 = matrix.M21; M22 = matrix.M22; M23 = matrix.M23;
        M30 = matrix.M30; M31 = matrix.M31; M32 = matrix.M32; M33 = matrix.M33;
    }
    
    /// <summary>
    /// Gets the element by linear index.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Get(int index) => MatrixMath.SquareGet(index, in M00, Length);
    
    /// <summary>
    /// Gets the element by row and column index.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Get(int x, int y) => MatrixMath.SquareGet(x, y, in M00, Dimensionality);

    /// <summary>
    /// Compares this matrix with another for exact equality.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Matrix4x4 other)
    {
        return M00.Equals(other.M00) && M01.Equals(other.M01) &&
               M02.Equals(other.M02) && M03.Equals(other.M03) &&
               // Row 1
               M10.Equals(other.M10) && M11.Equals(other.M11) &&
               M12.Equals(other.M12) && M13.Equals(other.M13) &&
               // Row 2
               M20.Equals(other.M20) && M21.Equals(other.M21) &&
               M22.Equals(other.M22) && M23.Equals(other.M23) &&
               // Row 3
               M30.Equals(other.M30) && M31.Equals(other.M31) &&
               M32.Equals(other.M32) && M33.Equals(other.M33);
    }
    
    /// <summary>
    /// Compares this matrix with another for equality within a given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AboutEquals(Matrix4x4 other, float tolerance = HyperMath.FloatTolerance)
    {
        return M00.AboutEquals(other.M00, tolerance) && M01.AboutEquals(other.M01, tolerance) &&
               M02.AboutEquals(other.M02, tolerance) && M03.AboutEquals(other.M03, tolerance) &&
               // Row 1
               M10.AboutEquals(other.M10, tolerance) && M11.AboutEquals(other.M11, tolerance) &&
               M12.AboutEquals(other.M12, tolerance) && M13.AboutEquals(other.M13, tolerance) &&
               // Row 2
               M20.AboutEquals(other.M20, tolerance) && M21.AboutEquals(other.M21, tolerance) &&
               M22.AboutEquals(other.M22, tolerance) && M23.AboutEquals(other.M23, tolerance) &&
               // Row 3
               M30.AboutEquals(other.M30, tolerance) && M31.AboutEquals(other.M31, tolerance) &&
               M32.AboutEquals(other.M32, tolerance) && M33.AboutEquals(other.M33, tolerance);
    }

    #region IEnumerable

    [MustDisposeResource, MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<float>) this).GetEnumerator();

    [MustDisposeResource, MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<Vector4> IEnumerable<Vector4>.GetEnumerator()
    {
        yield return Row0;
        yield return Row1;
        yield return Row2;
        yield return Row3;
    }
    
    IEnumerator<float> IEnumerable<float>.GetEnumerator()
    {
        yield return M00; yield return M01; yield return M02; yield return M03;
        yield return M10; yield return M11; yield return M12; yield return M13;
        yield return M20; yield return M21; yield return M22; yield return M23;
        yield return M30; yield return M31; yield return M32; yield return M33;
    }
    
    #endregion
    
    #region Deconstruct
    
    /// <summary>
    /// Deconstructs the matrix into its individual elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(
        out float m00, out float m01, out float m02, out float m03,
        out float m10, out float m11, out float m12, out float m13,
        out float m20, out float m21, out float m22, out float m23,
        out float m30, out float m31, out float m32, out float m33)
    {
        m00 = M00; m01 = M01; m02 = M02; m03 = M03;
        m10 = M10; m11 = M11; m12 = M12; m13 = M13;
        m20 = M20; m21 = M21; m22 = M22; m23 = M23;
        m30 = M30; m31 = M31; m32 = M32; m33 = M33;
    }
    
    /// <summary>
    /// Deconstructs the matrix into row vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out Vector4 row0, out Vector4 row1, out Vector4 row2, out Vector4 row3)
    {
        row0 = Row0;
        row1 = Row1;
        row2 = Row2;
        row3 = Row3;
    }
    
    #endregion
    
    #region Equality / ToString
    
    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Matrix4x4 other && Equals(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(Row0, Row1, Row2, Row3);

    /// <summary>
    /// Returns a string representation of the matrix using the default "f" (flat) format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => ToString(MatrixFormatter.FormatFlat.ToString(), CultureInfo.InvariantCulture);

    /// <summary>
    /// Formats the value of the current instance using the specified format.
    /// </summary>
    /// <param name="format">The format specifier (b, c, i, f).</param>
    /// <param name="formatProvider">The format provider.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string? format, IFormatProvider? formatProvider = null) =>
        MatrixFormatter.Format(format, formatProvider, nameof(Matrix4x4), Dimensionality, Span);
    
    #endregion

    #region Advanced Math

    /// <summary>
    /// Calculates the determinant of the 4x4 matrix.
    /// </summary>
    public float Determinant()
    {
        // Laplace's rule for decomposition by row.
        return M00 * (M11 * (M22 * M33 - M23 * M32) - M12 * (M21 * M33 - M23 * M31) + M13 * (M21 * M32 - M22 * M31)) -
               M01 * (M10 * (M22 * M33 - M23 * M32) - M12 * (M20 * M33 - M23 * M30) + M13 * (M20 * M32 - M22 * M30)) +
               M02 * (M10 * (M21 * M33 - M23 * M31) - M11 * (M20 * M33 - M23 * M30) + M13 * (M20 * M31 - M21 * M30)) -
               M03 * (M10 * (M21 * M32 - M22 * M31) - M11 * (M20 * M32 - M22 * M30) + M12 * (M20 * M31 - M21 * M30));
    }

    /// <summary>
    /// Returns an inverted copy of this matrix.
    /// Throws <see cref="InvalidOperationException"/> if the matrix is singular (determinant is 0).
    /// </summary>
    public Matrix4x4 Inverted()
    {
        var det = Determinant();
        if (float.Abs(det) < float.Epsilon)
            throw new InvalidOperationException("Matrix is singular and cannot be inverted.");

        var invDet = 1.0f / det;

        // Calculation using a matrix of algebraic complements (Adjugate matrix)
        return new Matrix4x4(
            // Row 0
            invDet * (M11 * (M22 * M33 - M23 * M32) + M12 * (M23 * M31 - M21 * M33) + M13 * (M21 * M32 - M22 * M31)),
            invDet * (M01 * (M23 * M32 - M22 * M33) + M02 * (M21 * M33 - M23 * M31) + M03 * (M22 * M31 - M21 * M32)),
            invDet * (M01 * (M12 * M33 - M13 * M32) + M02 * (M13 * M31 - M11 * M33) + M03 * (M11 * M32 - M12 * M31)),
            invDet * (M01 * (M13 * M22 - M12 * M23) + M02 * (M11 * M23 - M13 * M21) + M03 * (M12 * M21 - M11 * M22)),
            // Row 1
            invDet * (M10 * (M23 * M32 - M22 * M33) + M12 * (M20 * M33 - M23 * M30) + M13 * (M22 * M30 - M20 * M32)),
            invDet * (M00 * (M22 * M33 - M23 * M32) + M02 * (M23 * M30 - M20 * M33) + M03 * (M20 * M32 - M22 * M30)),
            invDet * (M00 * (M13 * M32 - M12 * M33) + M02 * (M10 * M33 - M13 * M30) + M03 * (M12 * M30 - M10 * M32)),
            invDet * (M00 * (M12 * M23 - M13 * M22) + M02 * (M13 * M20 - M10 * M23) + M03 * (M10 * M22 - M12 * M20)),
            // Row 2
            invDet * (M10 * (M21 * M33 - M23 * M31) + M11 * (M23 * M30 - M20 * M33) + M13 * (M20 * M31 - M21 * M30)),
            invDet * (M00 * (M23 * M31 - M21 * M33) + M01 * (M20 * M33 - M23 * M30) + M03 * (M21 * M30 - M20 * M31)),
            invDet * (M00 * (M11 * M33 - M13 * M31) + M01 * (M13 * M30 - M10 * M33) + M03 * (M10 * M31 - M11 * M30)),
            invDet * (M00 * (M13 * M21 - M11 * M23) + M01 * (M10 * M23 - M13 * M20) + M03 * (M11 * M20 - M10 * M21)),
            // Row 3
            invDet * (M10 * (M22 * M31 - M21 * M32) + M11 * (M20 * M32 - M22 * M30) + M12 * (M21 * M30 - M20 * M31)),
            invDet * (M00 * (M21 * M32 - M22 * M31) + M01 * (M22 * M30 - M20 * M32) + M02 * (M20 * M31 - M21 * M30)),
            invDet * (M00 * (M12 * M31 - M11 * M32) + M01 * (M10 * M32 - M12 * M30) + M02 * (M11 * M30 - M10 * M31)),
            invDet * (M00 * (M11 * M22 - M12 * M21) + M01 * (M12 * M20 - M10 * M22) + M02 * (M10 * M21 - M11 * M20))
        );
    }

    #endregion
    
    #region Operations
    
    #region Operations Equality

    /// <summary>
    /// Compares two matrices for equality using a tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Matrix4x4 a, Matrix4x4 b) => a.AboutEquals(b);

    /// <summary>
    /// Compares two matrices for inequality using a tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Matrix4x4 a, Matrix4x4 b) => !(a == b);

    #endregion

    #region Operations Addition 

    /// <summary>
    /// Adds a scalar to each element of the matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 operator +(Matrix4x4 a, float b)
    {
        return new Matrix4x4(
            a.M00 + b, a.M01 + b, a.M02 + b, a.M03 + b,
            a.M10 + b, a.M11 + b, a.M12 + b, a.M13 + b,
            a.M20 + b, a.M21 + b, a.M22 + b, a.M23 + b,
            a.M30 + b, a.M31 + b, a.M32 + b, a.M33 + b
        );
    }
    
    /// <summary>
    /// Adds a scalar to each element of the matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 operator +(float a, Matrix4x4 b) => b + a;

    /// <summary>
    /// Adds a vector to each row of the matrix.
    /// </summary>
    public static Matrix4x4 operator +(Matrix4x4 m, Vector4 v)
    {
        return new Matrix4x4(
            m.M00 + v.X, m.M01 + v.Y, m.M02 + v.Z, m.M03 + v.W,
            m.M10 + v.X, m.M11 + v.Y, m.M12 + v.Z, m.M13 + v.W,
            m.M20 + v.X, m.M21 + v.Y, m.M22 + v.Z, m.M23 + v.W,
            m.M30 + v.X, m.M31 + v.Y, m.M32 + v.Z, m.M33 + v.W
        );
    }

    /// <summary>
    /// Performs component-wise addition of two matrices.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 operator +(Matrix4x4 a, Matrix4x4 b)
    {
        return new Matrix4x4(
            a.M00 + b.M00, a.M01 + b.M01, a.M02 + b.M02, a.M03 + b.M03,
            a.M10 + b.M10, a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13,
            a.M20 + b.M20, a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23,
            a.M30 + b.M30, a.M31 + b.M31, a.M32 + b.M32, a.M33 + b.M33
        );
    }

    #endregion
    
    #region Operations Subtraction
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 operator -(Matrix4x4 a)
    {
        return new Matrix4x4(
            -a.M00, -a.M01, -a.M02, -a.M03,
            -a.M10, -a.M11, -a.M12, -a.M13,
            -a.M20, -a.M21, -a.M22, -a.M23,
            -a.M30, -a.M31, -a.M32, -a.M33
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 operator -(Matrix4x4 a, Matrix4x4 b)
    {
        return new Matrix4x4(
            a.M00 - b.M00, a.M01 - b.M01, a.M02 - b.M02, a.M03 - b.M03,
            a.M10 - b.M10, a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13,
            a.M20 - b.M20, a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23,
            a.M30 - b.M30, a.M31 - b.M31, a.M32 - b.M32, a.M33 - b.M33
        );
    }
    
    #endregion
    
    #region Operations Multiplication

    #region Utility
    
    /// <summary>
    /// Transforms a rectangle by the matrix.
    /// row-major: v' = v * M
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect2 operator *(Rect2 r, Matrix4x4 m)
    {
        var v1 = new Vector2(r.Right, r.Top) * m;
        var v2 = new Vector2(r.Left, r.Bottom) * m;
        return new Rect2(v2.X, v1.Y, v1.X, v2.Y);
    }
    
    /// <summary>
    /// Transforms a 4-point rectangle by the matrix.
    /// row-major: v' = v * M
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect4 operator *(Rect4 r, Matrix4x4 m) =>
        new(
            r.Point0 * m,
            r.Point1 * m,
            r.Point2 * m,
            r.Point3 * m
        );

    #endregion

    /// <summary>
    /// Multiplies each element of the matrix by a scalar.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 operator *(Matrix4x4 a, float s) =>
        new(
            a.M00 * s, a.M01 * s, a.M02 * s, a.M03 * s,
            a.M10 * s, a.M11 * s, a.M12 * s, a.M13 * s,
            a.M20 * s, a.M21 * s, a.M22 * s, a.M23 * s,
            a.M30 * s, a.M31 * s, a.M32 * s, a.M33 * s
        );

    /// <summary>
    /// Transforms a Vector2 by the matrix (assuming z = 0, w = 1).
    /// row-major: v' = v * M
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 v, Matrix4x4 m) =>
        new(
            v.X * m.M00 + v.Y * m.M10 + m.M30,
            v.X * m.M01 + v.Y * m.M11 + m.M31
        );

    /// <summary>
    /// Transforms a Vector3 by the matrix (assuming w = 1).
    /// row-major: v' = v * M
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 v, Matrix4x4 m) =>
        new(
            v.X * m.M00 + v.Y * m.M10 + v.Z * m.M20 + m.M30,
            v.X * m.M01 + v.Y * m.M11 + v.Z * m.M21 + m.M31,
            v.X * m.M02 + v.Y * m.M12 + v.Z * m.M22 + m.M32
        );

    /// <summary>
    /// Transforms a <see cref="Vector4"/> by the matrix.
    /// row-major: v' = v * M
    /// </summary>
    public static Vector4 operator *(Vector4 v, Matrix4x4 m) =>
        new(
            v.X * m.M00 + v.Y * m.M10 + v.Z * m.M20 + v.W * m.M30,
            v.X * m.M01 + v.Y * m.M11 + v.Z * m.M21 + v.W * m.M31,
            v.X * m.M02 + v.Y * m.M12 + v.Z * m.M22 + v.W * m.M32,
            v.X * m.M03 + v.Y * m.M13 + v.Z * m.M23 + v.W * m.M33
        );

    /// <summary>
    /// Multiplies two matrices (A * B).
    /// </summary>
    public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b) =>
        new(
            // Row 0
            a.M00 * b.M00 + a.M01 * b.M10 + a.M02 * b.M20 + a.M03 * b.M30,
            a.M00 * b.M01 + a.M01 * b.M11 + a.M02 * b.M21 + a.M03 * b.M31,
            a.M00 * b.M02 + a.M01 * b.M12 + a.M02 * b.M22 + a.M03 * b.M32,
            a.M00 * b.M03 + a.M01 * b.M13 + a.M02 * b.M23 + a.M03 * b.M33,
            // Row 1
            a.M10 * b.M00 + a.M11 * b.M10 + a.M12 * b.M20 + a.M13 * b.M30,
            a.M10 * b.M01 + a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31,
            a.M10 * b.M02 + a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32,
            a.M10 * b.M03 + a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33,
            // Row 2
            a.M20 * b.M00 + a.M21 * b.M10 + a.M22 * b.M20 + a.M23 * b.M30,
            a.M20 * b.M01 + a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31,
            a.M20 * b.M02 + a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32,
            a.M20 * b.M03 + a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33,
            // Row 3
            a.M30 * b.M00 + a.M31 * b.M10 + a.M32 * b.M20 + a.M33 * b.M30,
            a.M30 * b.M01 + a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31,
            a.M30 * b.M02 + a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32,
            a.M30 * b.M03 + a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33
        );

    #endregion
    
    #endregion
    
    #region Creation

    #region Identity

    /// <summary>
    /// Creates a 4x4 identity matrix from a 3x3 matrix basis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateIdentity(Matrix3x3 matrix) =>
        new(
            matrix.M00, matrix.M01, matrix.M02, 0,
            matrix.M10, matrix.M11, matrix.M12, 0,
            matrix.M20, matrix.M21, matrix.M22, 0,
            0, 0, 0, 1
        );

    #endregion
    
    #region Transform

    /// <summary>
    /// Creates a transformation matrix using S * R * T order (row-major).
    /// </summary>
    public static Matrix4x4 CreateTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        => CreateTransformSRT(position, rotation, scale);

    /// <summary>
    /// Creates a transformation matrix using S * R * T order (row-major).
    /// </summary>
    public static Matrix4x4 CreateTransformSRT(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        // Auxiliary variables for quaternions
        var xx = rotation.X * rotation.X;
        var yy = rotation.Y * rotation.Y;
        var zz = rotation.Z * rotation.Z;
        var xy = rotation.X * rotation.Y;
        var wz = rotation.Z * rotation.W;
        var xz = rotation.Z * rotation.X;
        var wy = rotation.Y * rotation.W;
        var yz = rotation.Y * rotation.Z;
        var wx = rotation.X * rotation.W;

        // Scaling the rotation matrix
        var m00 = (1 - 2 * (yy + zz)) * scale.X;
        var m01 = 2 * (xy + wz) * scale.X;
        var m02 = 2 * (xz - wy) * scale.X;
        var m10 = 2 * (xy - wz) * scale.Y;
        var m11 = (1 - 2 * (zz + xx)) * scale.Y;
        var m12 = 2 * (yz + wx) * scale.Y;
        var m20 = 2 * (xz + wy) * scale.Z;
        var m21 = 2 * (yz - wx) * scale.Z;
        var m22 = (1 - 2 * (yy + xx)) * scale.Z;

        // Create SRT matrix
        return new Matrix4x4(
            m00, m01, m02, 0,
            m10, m11, m12, 0,
            m20, m21, m22, 0,
            position.X, position.Y, position.Z, 1
        );
    }

    /// <summary>
    /// Creates a transformation matrix using T * R * S order.
    /// </summary>
    public static Matrix4x4 CreateTransformTRS(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        // Auxiliary variables for quaternions
        var xx = rotation.X * rotation.X;
        var yy = rotation.Y * rotation.Y;
        var zz = rotation.Z * rotation.Z;
        var xy = rotation.X * rotation.Y;
        var wz = rotation.Z * rotation.W;
        var xz = rotation.Z * rotation.X;
        var wy = rotation.Y * rotation.W;
        var yz = rotation.Y * rotation.Z;
        var wx = rotation.X * rotation.W;

        // Scaling the rotation matrix
        var m00 = (1 - 2 * (yy + zz)) * scale.X;
        var m01 = 2 * (xy + wz) * scale.Y;
        var m02 = 2 * (xz - wy) * scale.Z;
    
        var m10 = 2 * (xy - wz) * scale.X;
        var m11 = (1 - 2 * (zz + xx)) * scale.Y;
        var m12 = 2 * (yz + wx) * scale.Z;
    
        var m20 = 2 * (xz + wy) * scale.X;
        var m21 = 2 * (yz - wx) * scale.Y;
        var m22 = (1 - 2 * (yy + xx)) * scale.Z;

        // Create a TRS matrix
        return new Matrix4x4(
            m00, m01, m02, 0,
            m10, m11, m12, 0,
            m20, m21, m22, 0,
            position.X, position.Y, position.Z, 1
        );
    }

    #endregion
    
    #region Scale
    
    /// <summary>
    /// Creates a uniform scaling matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateScale(float scale) => CreateScale(scale, scale, scale);

    /// <summary>
    /// Creates a scaling matrix from a Vector2 (Z remains 1).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateScale(Vector2 scale) => CreateScale(scale.X, scale.Y, 1);

    /// <summary>
    /// Creates a scaling matrix from a Vector3.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateScale(Vector3 scale) => CreateScale(scale.X, scale.Y, scale.Z);

    /// <summary>
    /// Creates a scaling matrix from X, Y, Z components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateScale(float x, float y, float z) =>
        new(
            x, 0, 0, 0,
            0, y, 0, 0,
            0, 0, z, 0,
            0, 0, 0, 1
        );

    #endregion
    
    #region Rotation
    
    /// <summary>
    /// Creates a rotation matrix from a quaternion.
    /// </summary>
    public static Matrix4x4 CreateRotation(Quaternion rotation)
    {
        // Auxiliary variables for quaternions
        var xx = rotation.X * rotation.X;
        var yy = rotation.Y * rotation.Y;
        var zz = rotation.Z * rotation.Z;
        var xy = rotation.X * rotation.Y;
        var wz = rotation.Z * rotation.W;
        var xz = rotation.Z * rotation.X;
        var wy = rotation.Y * rotation.W;
        var yz = rotation.Y * rotation.Z;
        var wx = rotation.X * rotation.W;
        
        return new Matrix4x4(
            1 - 2 * (yy + zz), 2 * (xy + wz),     2 * (xz - wy), 0,
            2 * (xy - wz),     1 - 2 * (zz + xx), 2 * (yz + wx), 0,
            2 * (xz + wy),     2 * (yz - wx), 1 - 2 * (yy + xx), 0,
            0,                 0,                             0, 1
        );
    }

    /// <summary>
    /// Creates a rotation matrix around an axis by an Angle.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotation(Vector3 direction, Angle angle) => CreateRotation(direction, (float) angle);

    /// <summary>
    /// Creates a rotation matrix around an axis by radians.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotation(Vector3 direction, float angle)
    {
        var cos = float.Cos(angle);
        var sin = float.Sin(angle);
        var t = 1.0f - cos;
        
        direction = direction.Normalized;

        return new Matrix4x4(
            // Row 0
            t * direction.X * direction.X + cos,
            t * direction.X * direction.Y - sin * direction.Z,
            t * direction.X * direction.Z + sin * direction.Y,
            0,
            // Row 1
            t * direction.X * direction.Y + sin * direction.Z,
            t * direction.Y * direction.Y + cos,
            t * direction.Y * direction.Z - sin * direction.X,
            0,
            // Row 2
            t * direction.X * direction.Z - sin * direction.Y,
            t * direction.Y * direction.Z + sin * direction.X,
            t * direction.Z * direction.Z + cos,
            0,
            // Row 3
            0,
            0,
            0,
            1
        );
    }

    /// <summary>
    /// Creates a rotation matrix around the X axis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotationX(Angle angle) => CreateRotationX((float) angle);

    /// <summary>
    /// Creates a rotation matrix around the X axis by radians.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotationX(float angle)
    {
        var c = float.Cos(angle);
        var s = float.Sin(angle);
        return new Matrix4x4(
            1,  0, 0, 0,
            0,  c, s, 0,
            0, -s, c, 0,
            0,  0, 0, 1
        );
    }
    
    /// <summary>
    /// Creates a rotation matrix around the Y axis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotationY(Angle angle) => CreateRotationY((float) angle);

    /// <summary>
    /// Creates a rotation matrix around the Y axis by radians.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotationY(float angle)
    {
        var c = float.Cos(angle);
        var s = float.Sin(angle);
        return new Matrix4x4(
             c, 0, s, 0,
             0, 1, 0, 0,
            -s, 0, c, 0,
             0, 0, 0, 1
        );
    }

    /// <summary>
    /// Creates a rotation matrix around the Z axis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotationZ(Angle angle) => CreateRotationZ((float) angle);

    /// <summary>
    /// Creates a rotation matrix around the Z axis by radians.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateRotationZ(float angle)
    {
        var c = float.Cos(angle);
        var s = float.Sin(angle);
        return new Matrix4x4(
            c, -s, 0, 0,
            s,  c, 0, 0,
            0,  0, 1, 0,
            0,  0, 0, 1
        );
    }
    
    #endregion
    
    #region Translation

    /// <summary>
    /// Creates a translation matrix from a scalar (all components).
    /// </summary>
    public static Matrix4x4 CreateTranslation(float scalar) => CreateTranslation(scalar, scalar, scalar);

    /// <summary>
    /// Creates a translation matrix from a Vector2.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateTranslation(Vector2 vector) => CreateTranslation(vector.X, vector.Y, 0);

    /// <summary>
    /// Creates a translation matrix from X and Y components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateTranslation(float x, float y) => CreateTranslation(x, y, 0);

    /// <summary>
    /// Creates a translation matrix from a Vector3.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateTranslation(Vector3 vector) => CreateTranslation(vector.X, vector.Y, vector.Z);

    /// <summary>
    /// Creates a translation matrix from X, Y, Z components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateTranslation(float x, float y, float z) =>
        new(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            x, y, z, 1
        );

    #endregion
    
    #region Projection

    /// <summary>
    /// Creates an orthographic projection matrix from a Rect2.
    /// </summary>
    public static Matrix4x4 CreateOrthographic(Rect2 rect2, float zNear, float zFar) =>
        CreateOrthographic(rect2.Width, rect2.Height, zNear, zFar);
    
    /// <summary>
    /// Creates an orthographic projection matrix from size.
    /// </summary>
    public static Matrix4x4 CreateOrthographic(Vector2 size, float zNear, float zFar) =>
        CreateOrthographic(size.X, size.Y, zNear, zFar);
    
    /// <summary>
    /// Creates a centered orthographic projection matrix.
    /// </summary>
    public static Matrix4x4 CreateOrthographic(float width, float height, float zNear, float zFar) =>
        CreateOrthographicOffCenter(-width / 2f, width / 2f, -height / 2f, height / 2f, zNear, zFar);

    /// <summary>
    /// Creates an off-center orthographic projection matrix (OpenGL convention).
    /// </summary>
    public static Matrix4x4 CreateOrthographicOffCenter(
        float left,
        float right,
        float bottom,
        float top,
        float zNear,
        float zFar)
    {
        var invRL = 1f / (right - left);
        var invTB = 1f / (top - bottom);
        var invFN = 1f / (zFar - zNear);

        
        // Scaling the rotation matrix
        var m00 = 2f *  invRL;
        var m11 = 2f *  invTB;
        var m22 = 2f * -invFN;
        
        var m30 = -(right + left) * invRL;
        var m31 = -(top + bottom) * invTB;
        var m32 = -(zFar + zNear) * invFN;
        
        return new Matrix4x4(
            m00,   0,   0, 0,
              0, m11,   0, 0,
              0,   0, m22, 0,
            m30, m31, m32, 1
        );
    }
    
    /// <summary>
    /// Creates a perspective projection matrix (OpenGL convention).
    /// </summary>
    public static Matrix4x4 CreatePerspective(
        float fov,
        float aspect,
        float zNear,
        float zFar)
    {
        var tanHalfFov = float.Tan(fov * 0.5f);

        var h = 1f / tanHalfFov;
        var w = h / aspect;

        var range = zFar / (zNear - zFar);

        return new Matrix4x4(
            w,  0,  0,              0,
            0,  h,  0,              0,
            0,  0,  range,         -1,
            0,  0,  range * zNear,  0
        );
    }
    
    #endregion
    
    #endregion
}
