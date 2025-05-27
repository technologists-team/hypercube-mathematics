using System.Diagnostics.CodeAnalysis;

namespace Hypercube.Mathematics.Matrices;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public readonly partial struct Matrix4x4
{
    /*
     * System.Numerics Compatibility
     */
    
    public static implicit operator System.Numerics.Matrix4x4(Matrix4x4 matrix)
    {
        return new System.Numerics.Matrix4x4(
            matrix.M00, matrix.M01, matrix.M02, matrix.M03,
            matrix.M10, matrix.M11, matrix.M12, matrix.M13,
            matrix.M20, matrix.M21, matrix.M22, matrix.M23,
            matrix.M30, matrix.M31, matrix.M32, matrix.M33
        );
    }
    
    public static implicit operator Matrix4x4(System.Numerics.Matrix4x4 matrix)
    {
        return new Matrix4x4(
            matrix.M11, matrix.M12, matrix.M13, matrix.M14,
            matrix.M21, matrix.M22, matrix.M23, matrix.M24,
            matrix.M31, matrix.M32, matrix.M33, matrix.M34,
            matrix.M41, matrix.M42, matrix.M43, matrix.M44
        );
    }
}