using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using Hypercube.Mathematics.Matrices.Utilities;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Matrices;

/// <summary>
/// Implementation of a 5x5 matrix for rendering work. (COLUM-MAJOR)
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Matrix5x5 : IMatrix, IMatrixSquare, IEquatable<Matrix5x5>, IEnumerable<Vector5>, IEnumerable<float>, IFormattable
{
    /// <summary>
    /// The total number of elements in the matrix (4x4=16).
    /// </summary>
    public const int Length = Dimensionality * Dimensionality;
    
    /// <summary>
    /// The number of rows and columns in the matrix.
    /// </summary>
    public const int Dimensionality = 5;

    #region Constants
    
    /// <summary>
    /// A matrix where all elements are <see cref="float.NaN"/>.
    /// <code>
    /// NaN, NaN, NaN, NaN, NaN
    /// NaN, NaN, NaN, NaN, NaN
    /// NaN, NaN, NaN, NaN, NaN
    /// NaN, NaN, NaN, NaN, NaN
    /// NaN, NaN, NaN, NaN, NaN
    /// </code>
    /// </summary>
    public static readonly Matrix5x5 NaN = new(
        float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN, float.NaN, float.NaN
    );

    /// <summary>
    /// A matrix where all elements are zero.
    /// <code>
    /// 0, 0, 0, 0, 0
    /// 0, 0, 0, 0, 0
    /// 0, 0, 0, 0, 0
    /// 0, 0, 0, 0, 0
    /// 0, 0, 0, 0, 0
    /// </code>
    /// </summary>
    public static readonly Matrix5x5 Zero = new(
        0, 0, 0, 0, 0,
        0, 0, 0, 0, 0,
        0, 0, 0, 0, 0,
        0, 0, 0, 0, 0,
        0, 0, 0, 0 ,0
    );

    /// <summary>
    /// A matrix where all elements are one.
    /// <code>
    /// 1, 1, 1, 1, 1
    /// 1, 1, 1, 1, 1
    /// 1, 1, 1, 1, 1
    /// 1, 1, 1, 1, 1
    /// 1, 1, 1, 1, 1
    /// </code>
    /// </summary>
    public static readonly Matrix5x5 One = new(
        1, 1, 1, 1, 1,
        1, 1, 1, 1, 1,
        1, 1, 1, 1, 1,
        1, 1, 1, 1, 1,
        1, 1, 1, 1, 1
    );

    /// <summary>
    /// The identity matrix.
    /// <code>
    /// 1, 0, 0, 0, 0
    /// 0, 1, 0, 0, 0
    /// 0, 0, 1, 0, 0
    /// 0, 0, 0, 1, 0
    /// 0, 0, 0, 0, 1
    /// </code>
    /// </summary>
    public static readonly Matrix5x5 Identity = new(
        1, 0, 0, 0, 0,
        0, 1, 0, 0, 0,
        0, 0, 1, 0, 0,
        0, 0, 0, 1, 0,
        0, 0, 0, 0, 1
    );

    #endregion
    
    #region Fields

    /// <summary>
    /// Matrix x: 0, y: 0 element.
    /// </summary>
    public readonly float M00;
    
    /// <summary>
    /// Matrix x: 0, y: 1 element.
    /// </summary>
    public readonly float M10;
    
    /// <summary>
    /// Matrix x: 0, y: 2 element.
    /// </summary>
    public readonly float M20;
    
    /// <summary>
    /// Matrix x: 0, y: 3 element.
    /// </summary>
    public readonly float M30;
    
    /// <summary>
    /// Matrix x: 0, y: 4 element.
    /// </summary>
    public readonly float M40;
    
    /// <summary>
    /// Matrix x: 1, y: 0 element.
    /// </summary>
    public readonly float M01;
    
    /// <summary>
    /// Matrix x: 1, y: 1 element.
    /// </summary>
    public readonly float M11;
    
    /// <summary>
    /// Matrix x: 1, y: 2 element.
    /// </summary>
    public readonly float M21;
    
    /// <summary>
    /// Matrix x: 1, y: 3 element.
    /// </summary>
    public readonly float M31;
    
    /// <summary>
    /// Matrix x: 1, y: 4 element.
    /// </summary>
    public readonly float M41;
    
    /// <summary>
    /// Matrix x: 2, y: 0 element.
    /// </summary>
    public readonly float M02;

    /// <summary>
    /// Matrix x: 2, y: 1 element.
    /// </summary>
    public readonly float M12;
    
    /// <summary>
    /// Matrix x: 2, y: 2 element.
    /// </summary>
    public readonly float M22;
    
    /// <summary>
    /// Matrix x: 2, y: 3 element.
    /// </summary>
    public readonly float M32;
    
    /// <summary>
    /// Matrix x: 2, y: 4 element.
    /// </summary>
    public readonly float M42;
    
    /// <summary>
    /// Matrix x: 3, y: 0 element.
    /// </summary>
    public readonly float M03;
    
    /// <summary>
    /// Matrix x: 3, y: 1 element.
    /// </summary>
    public readonly float M13;

    /// <summary>
    /// Matrix x: 3, y: 2 element.
    /// </summary>
    public readonly float M23;
    
    /// <summary>
    /// Matrix x: 3, y: 3 element.
    /// </summary>
    public readonly float M33;
        
    /// <summary>
    /// Matrix x: 3, y: 4 element.
    /// </summary>
    public readonly float M43;
    
    /// <summary>
    /// Matrix x: 4, y: 0 element.
    /// </summary>
    public readonly float M04;

    /// <summary>
    /// Matrix x: 4, y: 1 element.
    /// </summary>
    public readonly float M14;
    
    /// <summary>
    /// Matrix x: 4, y: 2 element.
    /// </summary>
    public readonly float M24;
    
    /// <summary>
    /// Matrix x: 4, y: 3 element.
    /// </summary>
    public readonly float M34;
    
    /// <summary>
    /// Matrix x: 4, y: 4 element.
    /// </summary>
    public readonly float M44;

    #endregion

    #region Rows / Columns

    /// <summary>
    /// Gets the row at index 0 of the matrix.
    /// </summary>
    public Vector5 Row0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M00, M01, M02, M03, M04);
    }

    /// <summary>
    /// Gets the row at index 1 of the matrix.
    /// </summary>
    public Vector5 Row1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M10, M11, M12, M13, M14);
    }

    /// <summary>
    /// Gets the row at index 2 of the matrix.
    /// </summary>
    public Vector5 Row2
    {  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M20, M21, M22, M23, M24);
    }

    /// <summary>
    /// Gets the row at index 3 of the matrix.
    /// </summary>
    public Vector5 Row3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M30, M31, M32, M33, M34);
    }

    /// <summary>
    /// Gets the row at index 4 of the matrix.
    /// </summary>
    public Vector5 Row4
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M40, M41, M42, M43, M44);
    }

    /// <summary>
    /// Gets the column at index 0 of the matrix.
    /// </summary>
    public Vector5 Column0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M00, M10, M20, M30, M40);
    }

    /// <summary>
    /// Gets the column at index 1 of the matrix.
    /// </summary>
    public Vector5 Column1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M01, M11, M21, M31, M41);
    }

    /// <summary>
    /// Gets the column at index 2 of the matrix.
    /// </summary>
    public Vector5 Column2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M02, M12, M22, M32, M42);
    }

    /// <summary>
    /// Gets the column at index 3 of the matrix.
    /// </summary>
    public Vector5 Column3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M03, M13, M23, M33, M43);
    }

    /// <summary>
    /// Gets the column at index 4 of the matrix.
    /// </summary>
    public Vector5 Column4
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M04, M14, M24, M34, M44);
    }

    #endregion
    
    /// <summary>
    /// Returns a new matrix that is the transpose of this matrix.
    /// </summary>
    public Matrix5x5 Transposed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(
            M00, M10, M20, M30, M40,
            M01, M11, M21, M31, M41,
            M02, M12, M22, M32, M42,
            M03, M13, M23, M33, M43,
            M04, M14, M24, M34, M44
        );
    }
    
    /// <summary>
    /// Returns a new array containing the matrix elements in column-major order.
    /// </summary>
    public float[] Array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Span.ToArray();
    }
    
    /// <summary>
    /// Returns a read-only span that wraps the matrix elements in column-major order.
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
    /// Gets the element at the specified linear index (column-major).
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
    /// Initializes a new instance of the <see cref="Matrix5x5"/> struct.
    /// <code>
    /// m00, m01, m02, m03, m04 
    /// m10, m11, m12, m13, m14
    /// m20, m21, m22, m23, m24
    /// m30, m31, m32, m33, m34
    /// m40, m41, m42, m43, m44
    /// </code>
    /// </summary>
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
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix5x5"/> struct from row vectors.
    /// </summary>
    public Matrix5x5(
        Vector5 row0,
        Vector5 row1,
        Vector5 row2,
        Vector5 row3,
        Vector5 row4)
    {
        M00 = row0.X; M01 = row0.Y; M02 = row0.Z; M03 = row0.W; M04 = row0.V;
        M10 = row1.X; M11 = row1.Y; M12 = row1.Z; M13 = row1.W; M14 = row1.V;
        M20 = row2.X; M21 = row2.Y; M22 = row2.Z; M23 = row2.W; M24 = row2.V;
        M30 = row3.X; M31 = row3.Y; M32 = row3.Z; M33 = row3.W; M34 = row3.V;
        M40 = row4.X; M41 = row4.Y; M42 = row4.Z; M43 = row4.W; M44 = row4.V;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix5x5"/> struct from a <see cref="Matrix4x4"/>.
    /// </summary>
    public Matrix5x5(Matrix4x4 matrix)
    {
        M00 = matrix.M00; M01 = matrix.M01; M02 = matrix.M02; M03 = matrix.M03; M04 = 0;
        M10 = matrix.M10; M11 = matrix.M11; M12 = matrix.M12; M13 = matrix.M13; M14 = 0;
        M20 = matrix.M20; M21 = matrix.M21; M22 = matrix.M22; M23 = matrix.M23; M24 = 0;
        M30 = matrix.M30; M31 = matrix.M31; M32 = matrix.M32; M33 = matrix.M33; M34 = 0;
        M40 = 0; M41 = 0; M42 = 0; M43 = 0; M44 = 0;
    }
    
    /// <summary>
    /// Initializes a new copy of a <see cref="Matrix5x5"/>.
    /// </summary>
    public Matrix5x5(Matrix5x5 matrix)
    {
        M00 = matrix.M00; M01 = matrix.M01; M02 = matrix.M02; M03 = matrix.M03; M04 = matrix.M04;
        M10 = matrix.M10; M11 = matrix.M11; M12 = matrix.M12; M13 = matrix.M13; M14 = matrix.M14;
        M20 = matrix.M20; M21 = matrix.M21; M22 = matrix.M22; M23 = matrix.M23; M24 = matrix.M24;
        M30 = matrix.M30; M31 = matrix.M31; M32 = matrix.M32; M33 = matrix.M33; M34 = matrix.M34;
        M40 = matrix.M40; M41 = matrix.M41; M42 = matrix.M42; M43 = matrix.M43; M44 = matrix.M44;
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
    public bool Equals(Matrix5x5 other)
    {
        return M00.Equals(other.M00) && M01.Equals(other.M01) &&
               M02.Equals(other.M02) && M03.Equals(other.M03) &&
               M04.Equals(other.M04) &&
               // Row 1
               M10.Equals(other.M10) && M11.Equals(other.M11) &&
               M12.Equals(other.M12) && M13.Equals(other.M13) &&
               M14.Equals(other.M14) &&
               // Row 2
               M20.Equals(other.M20) && M21.Equals(other.M21) &&
               M22.Equals(other.M22) && M23.Equals(other.M23) &&
               M24.Equals(other.M24) &&
               // Row 3
               M30.Equals(other.M30) && M31.Equals(other.M31) &&
               M32.Equals(other.M32) && M32.Equals(other.M32) &&
               M34.Equals(other.M34) &&
               // Row 4
               M40.Equals(other.M40) && M41.Equals(other.M41) &&
               M42.Equals(other.M42) && M42.Equals(other.M42) && 
               M44.Equals(other.M44);
    }
    
    /// <summary>
    /// Compares this matrix with another for equality within a given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AboutEquals(Matrix5x5 other, float tolerance = HyperMath.FloatTolerance)
    {
        return M00.AboutEquals(other.M00, tolerance) && M01.AboutEquals(other.M01, tolerance) &&
               M02.AboutEquals(other.M02, tolerance) && M03.AboutEquals(other.M03, tolerance) &&
               M04.AboutEquals(other.M04, tolerance) &&
               // Row 1
               M10.AboutEquals(other.M10, tolerance) && M11.AboutEquals(other.M11, tolerance) &&
               M12.AboutEquals(other.M12, tolerance) && M13.AboutEquals(other.M13, tolerance) &&
               M14.AboutEquals(other.M14, tolerance) &&
               // Row 2
               M20.AboutEquals(other.M20, tolerance) && M21.AboutEquals(other.M21, tolerance) &&
               M22.AboutEquals(other.M22, tolerance) && M23.AboutEquals(other.M23, tolerance) &&
               M34.AboutEquals(other.M34, tolerance) &&
               // Row 3
               M30.AboutEquals(other.M30, tolerance) && M31.AboutEquals(other.M31, tolerance) &&
               M32.AboutEquals(other.M32, tolerance) && M33.AboutEquals(other.M33, tolerance) &&
               M44.AboutEquals(other.M44, tolerance) &&
               // Row 4
               M40.AboutEquals(other.M40, tolerance) && M41.AboutEquals(other.M41, tolerance) &&
               M42.AboutEquals(other.M42, tolerance) && M42.AboutEquals(other.M42, tolerance) && 
               M44.AboutEquals(other.M44, tolerance);
    }
    
    #region IEnumerable

    [MustDisposeResource, MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<float>) this).GetEnumerator();

    [MustDisposeResource, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<Vector5> GetEnumerator()
    {
        yield return Row0;
        yield return Row1;
        yield return Row2;
        yield return Row3;
        yield return Row4;
    }
    
    IEnumerator<float> IEnumerable<float>.GetEnumerator()
    {
        yield return M00; yield return M01; yield return M02; yield return M03; yield return M04;
        yield return M10; yield return M11; yield return M12; yield return M13; yield return M14;
        yield return M20; yield return M21; yield return M22; yield return M23; yield return M24;
        yield return M30; yield return M31; yield return M32; yield return M33; yield return M34;
        yield return M40; yield return M41; yield return M42; yield return M43; yield return M44;
    }
    
    #endregion
        
    #region Deconstruct
    
    /// <summary>
    /// Deconstructs the matrix into its individual elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(
        out float m00, out float m01, out float m02, out float m03, out float m04,
        out float m10, out float m11, out float m12, out float m13, out float m14,
        out float m20, out float m21, out float m22, out float m23, out float m24,
        out float m30, out float m31, out float m32, out float m33, out float m34,
        out float m40, out float m41, out float m42, out float m43, out float m44)
    {
        m00 = M00; m01 = M01; m02 = M02; m03 = M03; m04 = M04;
        m10 = M10; m11 = M11; m12 = M12; m13 = M13; m14 = M14;
        m20 = M20; m21 = M21; m22 = M22; m23 = M23; m24 = M24;
        m30 = M30; m31 = M31; m32 = M32; m33 = M33; m34 = M34;
        m40 = M40; m41 = M41; m42 = M42; m43 = M43; m44 = M44;
    }
    
    /// <summary>
    /// Deconstructs the matrix into row vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out Vector5 row0, out Vector5 row1, out Vector5 row2, out Vector5 row3, out Vector5 row4)
    {
        row0 = Row0;
        row1 = Row1;
        row2 = Row2;
        row3 = Row3;
        row4 = Row4;
    }
    
    #endregion
    
    #region Equality / ToString
    
    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Matrix5x5 other && Equals(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(Row0, Row1, Row2, Row3, Row4);

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
        MatrixFormatter.Format(format, formatProvider, nameof(Matrix5x5), Dimensionality, Span);

    #endregion
    
    // TODO: Advanced Math
    
    #region Operations
    
    #region Operators Equality
    
    /// <summary>
    /// Compares two matrices for equality using a tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Matrix5x5 left, Matrix5x5 right) => left.AboutEquals(right);

    /// <summary>
    /// Compares two matrices for inequality using a tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Matrix5x5 left, Matrix5x5 right) => !(left == right);

    #endregion
    
    #region Operations Multiplication
    
    /// <summary>
    /// Multiplies two matrices (A * B).
    /// </summary>
    public static Matrix5x5 operator *(Matrix5x5 a, Matrix5x5 b)
    {
        return new Matrix5x5(
            // Row 0
            a.M00 * b.M00 + a.M01 * b.M10 + a.M02 * b.M20 + a.M03 * b.M30 + a.M04 * b.M40,
            a.M00 * b.M01 + a.M01 * b.M11 + a.M02 * b.M21 + a.M03 * b.M31 + a.M04 * b.M41,
            a.M00 * b.M02 + a.M01 * b.M12 + a.M02 * b.M22 + a.M03 * b.M32 + a.M04 * b.M42,
            a.M00 * b.M03 + a.M01 * b.M13 + a.M02 * b.M23 + a.M03 * b.M33 + a.M04 * b.M43,
            a.M00 * b.M04 + a.M01 * b.M14 + a.M02 * b.M24 + a.M03 * b.M34 + a.M04 * b.M44,
            // Row 1
            a.M10 * b.M00 + a.M11 * b.M10 + a.M12 * b.M20 + a.M13 * b.M30 + a.M14 * b.M40,
            a.M10 * b.M01 + a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
            a.M10 * b.M02 + a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
            a.M10 * b.M03 + a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
            a.M10 * b.M04 + a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,
            // Row 2
            a.M20 * b.M00 + a.M21 * b.M10 + a.M22 * b.M20 + a.M23 * b.M30 + a.M24 * b.M40,
            a.M20 * b.M01 + a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
            a.M20 * b.M02 + a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
            a.M20 * b.M03 + a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
            a.M20 * b.M04 + a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,
            // Row 3
            a.M30 * b.M00 + a.M31 * b.M10 + a.M32 * b.M20 + a.M33 * b.M30 + a.M34 * b.M40,
            a.M30 * b.M01 + a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
            a.M30 * b.M02 + a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
            a.M30 * b.M03 + a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
            a.M30 * b.M04 + a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,
            // Row 4
            a.M40 * b.M00 + a.M41 * b.M10 + a.M42 * b.M20 + a.M43 * b.M30 + a.M44 * b.M40,
            a.M40 * b.M01 + a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
            a.M40 * b.M02 + a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
            a.M40 * b.M03 + a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
            a.M40 * b.M04 + a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44
        );
    }

    #endregion
    
    #endregion

    #region Creation
    
    #region Identity
    
    /// <summary>
    /// Creates a <see cref="Matrix5x5"/> identity from a <see cref="Matrix2x2"/> basis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix5x5 CreateIdentity(Matrix2x2 matrix)
    {
        return new Matrix5x5(
            matrix.M00, matrix.M01, 0, 0, 0,
            matrix.M10, matrix.M11, 0, 0, 0,
                     0,          0, 1, 0, 0,
                     0,          0, 0, 1, 0, 
                     0,          0, 0, 0, 1
        );
    }
    
    /// <summary>
    /// Creates a <see cref="Matrix5x5"/> identity from a <see cref="Matrix3x3"/> basis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix5x5 CreateIdentity(Matrix3x3 matrix)
    {
        return new Matrix5x5(
            matrix.M00, matrix.M01, matrix.M02, 0, 0,
            matrix.M10, matrix.M11, matrix.M12, 0, 0,
            matrix.M20, matrix.M21, matrix.M22, 0, 0,
                     0,          0,          0, 1, 0, 
                     0,          0,          0, 0, 1
        );
    }
    
    /// <summary>
    /// Creates a <see cref="Matrix5x5"/> identity from a <see cref="Matrix4x4"/> basis.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix5x5 CreateIdentity(Matrix4x4 matrix)
    {
        return new Matrix5x5(
            matrix.M00, matrix.M01, matrix.M02, matrix.M03, 0,
            matrix.M10, matrix.M11, matrix.M12, matrix.M13, 0,
            matrix.M20, matrix.M21, matrix.M22, matrix.M23, 0,
            matrix.M30, matrix.M31, matrix.M32, matrix.M33, 0, 
                     0,          0,          0,          0, 1
        );
    }

    #endregion
    
    #endregion
}