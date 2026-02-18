using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using Hypercube.Mathematics.Matrices.Utilities;
using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Matrices;

/// <summary>
/// Implementation of a 3x3 matrix for 2D transforms and 3D rotations. (COLUMN-MAJOR)
/// </summary>
/// <remarks>
/// Column-major matrix. Column vectors. OpenGL convention.
/// Vector is multiplied as: M * v
/// </remarks>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly struct Matrix3x3 : IMatrix, IMatrixSquare, IEquatable<Matrix3x3>, IEnumerable<Vector3>, IEnumerable<float>, IFormattable
{
    /// <summary>
    /// The total number of elements in the matrix (4x4=16).
    /// </summary>
    public const int Length = Dimensionality * Dimensionality;
    
    /// <summary>
    /// The number of rows and columns in the matrix.
    /// </summary>
    public const int Dimensionality = 3;

    #region Constants
    
    /// <summary>
    /// A matrix where all elements are <see cref="float.NaN"/>.
    /// <code>
    /// NaN, NaN, NaN
    /// NaN, NaN, NaN
    /// NaN, NaN, NaN
    /// </code>
    /// </summary>
    public static readonly Matrix3x3 NaN = new(
        float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN,
        float.NaN, float.NaN, float.NaN
    );
    
    /// <summary>
    /// A matrix where all elements are zero.
    /// <code>
    /// 0, 0, 0
    /// 0, 0, 0
    /// 0, 0, 0
    /// </code>
    /// </summary>
    public static readonly Matrix3x3 Zero = new(
        0, 0, 0,
        0, 0, 0,
        0, 0, 0
    );
    
    /// <summary>
    /// A matrix where all elements are one.
    /// <code>
    /// 1, 1, 1
    /// 1, 1, 1
    /// 1, 1, 1
    /// </code>
    /// </summary>
    public static readonly Matrix3x3 One = new(
        1, 1, 1,
        1, 1, 1,
        1, 1, 1
    );
    
    /// <summary>
    /// The identity matrix.
    /// <code>
    /// 1, 0, 0
    /// 0, 1, 0
    /// 0, 0, 1
    /// </code>
    /// </summary>
    public static readonly Matrix3x3 Identity = new(
        1, 0, 0,
        0, 1, 0,
        0, 0, 1
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
    
    #endregion
    
    #region Rows / Columns
    
    /// <summary>
    /// Gets the first row (index 0) of the matrix.
    /// </summary>
    public Vector3 Row0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M00, M01, M02);
    }

    /// <summary>
    /// Gets the second row (index 1) of the matrix.
    /// </summary>
    public Vector3 Row1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M10, M11, M12);
    }

    /// <summary>
    /// Gets the third row (index 2) of the matrix.
    /// </summary>
    public Vector3 Row2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M20, M21, M22);
    }
    
    /// <summary>
    /// Gets the first column (index 0) of the matrix.
    /// </summary>
    public Vector3 Column0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M00, M10, M20);
    }

    /// <summary>
    /// Gets the second column (index 1) of the matrix.
    /// </summary>
    public Vector3 Column1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M01, M11, M21);
    }

    /// <summary>
    /// Gets the third column (index 2) of the matrix.
    /// </summary>
    public Vector3 Column2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(M02, M12, M22);
    }
        
    #endregion
    
    /// <summary>
    /// Returns a new matrix that is the transpose of this matrix.
    /// </summary>
    public Matrix3x3 Transposed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(
            M00, M10, M20,
            M01, M11, M21,
            M02, M12, M22
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
    
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix3x3"/> struct.
    /// <code>
    /// m00, m01, m02
    /// m10, m11, m12
    /// m20, m21, m22
    /// </code>
    /// </summary>
    public Matrix3x3(
        float m00, float m01, float m02,
        float m10, float m11, float m12,
        float m20, float m21, float m22)
    {
        M00 = m00; M01 = m01; M02 = m02;
        M10 = m10; M11 = m11; M12 = m12;
        M20 = m20; M21 = m21; M22 = m22;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix3x3"/> struct from row vectors.
    /// </summary>
    public Matrix3x3(Vector3 row0, Vector3 row1, Vector3 row2)
    {
        M00 = row0.X; M01 = row0.Y; M02 = row0.Z;
        M10 = row1.X; M11 = row1.Y; M12 = row1.Z;
        M20 = row2.X; M21 = row2.Y; M22 = row2.Z;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix3x3"/> struct from a <see cref="Matrix2x2"/>.
    /// </summary>
    public Matrix3x3(Matrix2x2 matrix)
    {
        M00 = matrix.M00; M01 = matrix.M01; M02 = 0;
        M10 = matrix.M10; M11 = matrix.M11; M12 = 0;
        M20 = 0; M21 = 0; M22 = 0;
    }
    
    /// <summary>
    /// Initializes a new copy of a <see cref="Matrix3x3"/>.
    /// </summary>
    public Matrix3x3(Matrix3x3 matrix)
    {
        M00 = matrix.M00; M01 = matrix.M01; M02 = matrix.M02;
        M10 = matrix.M10; M11 = matrix.M11; M12 = matrix.M12;
        M20 = matrix.M20; M21 = matrix.M21; M22 = matrix.M22;
    }

    #endregion

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
    public bool Equals(Matrix3x3 other)
    {
        return M00.Equals(other.M00) && M01.Equals(other.M01) &&
               M02.Equals(other.M02) &&
               // Row 1
               M10.Equals(other.M10) && M11.Equals(other.M11) &&
               M12.Equals(other.M12) &&
               // Row 2
               M20.Equals(other.M20) && M21.Equals(other.M21) &&
               M22.Equals(other.M22);
    }

    /// <summary>
    /// Compares this matrix with another for equality within a given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AboutEquals(Matrix3x3 other, float tolerance = HyperMath.FloatTolerance)
    {
        return M00.AboutEquals(other.M00, tolerance) && M01.AboutEquals(other.M01, tolerance) &
               M02.AboutEquals(other.M02, tolerance) &&
               // Row 1
               M10.AboutEquals(other.M10, tolerance) && M11.AboutEquals(other.M11, tolerance) &&
               M12.AboutEquals(other.M12, tolerance) &&
               // Row 2
               M20.AboutEquals(other.M20, tolerance) && M21.AboutEquals(other.M21, tolerance) &&
               M22.AboutEquals(other.M22, tolerance);
    }
    
    #region IEnumerable
    
    [MustDisposeResource, MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<float>) this).GetEnumerator();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<Vector3> IEnumerable<Vector3>.GetEnumerator()
    {
        yield return Row0;
        yield return Row1;
        yield return Row2;
    }

    IEnumerator<float> IEnumerable<float>.GetEnumerator()
    {
        yield return M00; yield return M01; yield return M02;
        yield return M10; yield return M11; yield return M12;
        yield return M20; yield return M21; yield return M22;
    }
    
    #endregion
    
    #region Deconstruct
        
    /// <summary>
    /// Deconstructs the matrix into its individual elements.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(
        out float m00, out float m01, out float m02,
        out float m10, out float m11, out float m12,
        out float m20, out float m21, out float m22)
    {
        m00 = M00; m01 = M01; m02 = M02;
        m10 = M10; m11 = M11; m12 = M12;
        m20 = M20; m21 = M21; m22 = M22;
    }
    
    /// <summary>
    /// Deconstructs the matrix into row vectors.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out Vector3 row0, out Vector3 row1, out Vector3 row2)
    {
        row0 = Row0;
        row1 = Row1;
        row2 = Row2;
    }
    
    #endregion
    
    #region Equality / ToString

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Matrix3x3 other && Equals(other);
    
    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(Row0, Row1, Row2);
    
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
        MatrixFormatter.Format(format, formatProvider, nameof(Matrix3x3), Dimensionality, Span);

    #endregion

    #region Advanced Math

    /// <summary>
    /// Calculates the determinant of the 4x4 matrix.
    /// </summary>
    public float Determinant()
    {
        // Laplace's rule for decomposition by row.
        return M00 * (M11 * M22 - M12 * M21) -
               M01 * (M10 * M22 - M12 * M20) +
               M02 * (M10 * M21 - M11 * M20);
    }

    /// <summary>
    /// Returns an inverted copy of this matrix.
    /// Throws <see cref="InvalidOperationException"/> if the matrix is singular (determinant is 0).
    /// </summary>
    public Matrix3x3 Inverted()
    {
        var det = Determinant();
        if (float.Abs(det) < float.Epsilon)
            throw new InvalidOperationException("Matrix is singular and cannot be inverted.");

        var invDet = 1.0f / det;

        // Calculation using a matrix of algebraic complements (Adjugate matrix)
        return new Matrix3x3(
            // Row 0
            invDet * (M11 * M22 - M12 * M21),
            invDet * (M02 * M21 - M01 * M22),
            invDet * (M01 * M12 - M02 * M11),
            // Row 1
            invDet * (M12 * M20 - M10 * M22),
            invDet * (M00 * M22 - M02 * M20),
            invDet * (M02 * M10 - M00 * M12),
            // Row 3
            invDet * (M10 * M21 - M11 * M20),
            invDet * (M01 * M20 - M00 * M21),
            invDet * (M00 * M11 - M01 * M10)
        );
    }

    #endregion
    
    #region Operations
    
    #region Equality
    
    /// <summary>
    /// Compares two matrices for equality using a tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Matrix3x3 a, Matrix3x3 b) => a.AboutEquals(b);

    /// <summary>
    /// Compares two matrices for inequality using a tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Matrix3x3 a, Matrix3x3 b) => !(a == b);
    
    #endregion
    
    #region Addition 
    
    /// <summary>
    /// Adds a scalar to each element of the matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator +(Matrix3x3 a, float b)
    {
        return new Matrix3x3(
            a.M00 + b, a.M01 + b, a.M02 + b,
            a.M10 + b, a.M11 + b, a.M12 + b,
            a.M20 + b, a.M21 + b, a.M22 + b
        );
    }

    /// <summary>
    /// Adds a scalar to each element of the matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator +(float a, Matrix3x3 b) => b + a;
    
    /// <summary>
    /// Adds a vector to each row of the matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator +(Matrix3x3 a, Vector3 v)
    {
        return new Matrix3x3(
            a.M00 + v.X, a.M01 + v.Y, a.M02 + v.Z,
            a.M10 + v.X, a.M11 + v.Y, a.M12 + v.Z,
            a.M20 + v.X, a.M21 + v.Y, a.M22 + v.Z
        );
    }
    
    /// <summary>
    /// Performs component-wise addition of two matrices.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator +(Matrix3x3 a, Matrix3x3 b)
    {
        return new Matrix3x3(
            a.M00 + b.M00, a.M01 + b.M01, a.M02 + b.M02,
            a.M10 + b.M10, a.M11 + b.M11, a.M12 + b.M12,
            a.M20 + b.M20, a.M21 + b.M21, a.M22 + b.M22
        );
    }
    
    #endregion
    
    #region Subtraction
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator -(Matrix3x3 a)
    {
        return new Matrix3x3(
            -a.M00, -a.M01, -a.M02,
            -a.M10, -a.M11, -a.M12,
            -a.M20, -a.M21, -a.M22
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator -(Matrix3x3 a, Matrix3x3 b)
    {
        return new Matrix3x3(
            a.M00 - b.M00, a.M01 - b.M01, a.M02 - b.M02,
            a.M10 - b.M10, a.M11 - b.M11, a.M12 - b.M12,
            a.M20 - b.M20, a.M21 - b.M21, a.M22 - b.M22
        );
    }
    
    #endregion
        
    #region Multiplication
    
    /// <summary>
    /// Multiplies each element of the matrix by a scalar.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator *(Matrix3x3 m, float s)
    {
        return new Matrix3x3(
            m.M00 * s, m.M01 * s, m.M02 * s,
            m.M10 * s, m.M11 * s, m.M12 * s,
            m.M20 * s, m.M21 * s, m.M22 * s
        );
    }

    /// <summary>
    /// Transforms a Vector3 by the matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]  
    public static Vector3 operator *(Matrix3x3 m, Vector3 v)
    {
        return new Vector3(
            m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z,
            m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z,
            m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z
        );
    }
    
    /// <summary>
    /// Multiplies two matrices (A * B).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
    {
        return new Matrix3x3(
            // Row 0
            a.M00 * b.M00 + a.M01 * b.M10 + a.M02 * b.M20,
            a.M00 * b.M01 + a.M01 * b.M11 + a.M02 * b.M21,
            a.M00 * b.M02 + a.M01 * b.M12 + a.M02 * b.M22,
            // Row 1
            a.M10 * b.M00 + a.M11 * b.M10 + a.M12 * b.M20,
            a.M10 * b.M01 + a.M11 * b.M11 + a.M12 * b.M21,
            a.M10 * b.M02 + a.M11 * b.M12 + a.M12 * b.M22,
            // Row 2
            a.M20 * b.M00 + a.M21 * b.M10 + a.M22 * b.M20,
            a.M20 * b.M01 + a.M21 * b.M11 + a.M22 * b.M21,
            a.M20 * b.M02 + a.M21 * b.M12 + a.M22 * b.M22
        );
    }
    
    #endregion
    
    #endregion
    
    #region Creation

    #region Identity

    /// <summary>
    /// Creates a 3x3 identity matrix from a 2x2 matrix basis.
    /// </summary>
    public static Matrix3x3 CreateIdentity(Matrix2x2 matrix)
    {
        return new Matrix3x3(
            matrix.M00, matrix.M01, 0,
            matrix.M10, matrix.M11, 0,
            0,          0,          1
        );
    }

    #endregion

    #region Transform

    /// <summary>
    /// Creates a 2D transformation matrix using T * R * S order.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateTransform(Vector2 position, Angle angle, Vector2 scale)
        => CreateTransform(position, (float) angle, scale);
    
    /// <summary>
    /// Creates a 2D transformation matrix using T * R * S order.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateTransform(Vector2 position, float angle, Vector2 scale)
    {
        var cos = float.Cos(angle);
        var sin = float.Sin(angle);
        var sx = scale.X;
        var sy = scale.Y;
        return new Matrix3x3(
            sx * cos, -sy * sin, position.X,
            sx * sin,  sy * cos, position.Y,
            0,         0,        1
        );
    }
    
    #endregion

    #region Scale

    /// <summary>
    /// Creates a uniform scaling matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateScale(float scale) => CreateScale(scale, scale);

    /// <summary>
    /// Creates a scaling matrix from a Vector2.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateScale(Vector2 scale) => CreateScale(scale.X, scale.Y);

    /// <summary>
    /// Creates a scaling matrix from X and Y components.
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
    /// Creates a 3D scaling matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateScale(Vector3 scale)
    {
        return new Matrix3x3(
            scale.X, 0,       0,
            0,       scale.Y, 0,
            0,       0,       scale.Z
        );
    }

    #endregion

    #region Rotation

    /// <summary>
    /// Creates a 2D rotation matrix from an Angle.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateRotation(Angle angle) => CreateRotation((float) angle);

    /// <summary>
    /// Creates a 2D rotation matrix from radians.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateRotation(float angle)
    {
        var cos = float.Cos(angle);
        var sin = float.Sin(angle);
        return new Matrix3x3(
            cos, -sin, 0,
            sin,  cos, 0,
            0,    0,   1
        );
    }

    /// <summary>
    /// Creates a 3D rotation matrix around an axis by an Angle.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateRotation(Vector3 axis, Angle angle) => CreateRotation(axis, (float)angle);

    /// <summary>
    /// Creates a 3D rotation matrix around an axis by radians.
    /// </summary>
    public static Matrix3x3 CreateRotation(Vector3 axis, float angle)
    {
        var cos = float.Cos(-angle);
        var sin = float.Sin(-angle);
        var t = 1.0f - cos;
        
        var direction = axis.Normalized;

        return new Matrix3x3(
            t * direction.X * direction.X + cos,
            t * direction.X * direction.Y - sin * direction.Z,
            t * direction.X * direction.Z + sin * direction.Y,

            t * direction.X * direction.Y + sin * direction.Z,
            t * direction.Y * direction.Y + cos,
            t * direction.Y * direction.Z - sin * direction.X,

            t * direction.X * direction.Z - sin * direction.Y,
            t * direction.Y * direction.Z + sin * direction.X,
            t * direction.Z * direction.Z + cos
        );
    }

    #endregion

    #region Translation

    /// <summary>
    /// Creates a 2D translation matrix from a Vector2.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateTranslation(Vector2 vector) => CreateTranslation(vector.X, vector.Y);

    /// <summary>
    /// Creates a 2D translation matrix from X and Y components.
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

    #endregion

    #region Projection

    /// <summary>
    /// Creates a 2D orthographic projection matrix from a Rect2.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateOrthographic(Rect2 rect2) =>
        CreateOrthographic(rect2.Width, rect2.Height);

    /// <summary>
    /// Creates a 2D orthographic projection matrix from size.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateOrthographic(Vector2 size) =>
        CreateOrthographic(size.X, size.Y);

    /// <summary>
    /// Creates a centered 2D orthographic projection matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix3x3 CreateOrthographic(float width, float height) =>
        CreateOrthographicOffCenter(-width / 2f, width / 2f, -height / 2f, height / 2f);

    /// <summary>
    /// Creates an off-center 2D orthographic projection matrix.
    /// </summary>
    public static Matrix3x3 CreateOrthographicOffCenter(float left, float right, float bottom, float top)
    {
        var invRL = 1.0f / (right - left);
        var invTB = 1.0f / (top - bottom);

        return new Matrix3x3(
            2f * invRL, 0,          -(right + left) * invRL,
            0,          2f * invTB, -(top + bottom) * invTB,
            0,          0,          1
        );
    }

    #endregion

    #endregion
}
