using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Hypercube.Mathematics.Matrices;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial struct Matrix4x4
{
    /*
     * System.Numerics Compatibility
     */

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Matrix4x4(Matrix4x4 matrix4X4)
    {
        return new System.Numerics.Matrix4x4(
            matrix4X4.M00, matrix4X4.M01, matrix4X4.M02, matrix4X4.M03,
            matrix4X4.M10, matrix4X4.M11, matrix4X4.M12, matrix4X4.M13,
            matrix4X4.M20, matrix4X4.M21, matrix4X4.M22, matrix4X4.M23,
            matrix4X4.M30, matrix4X4.M31, matrix4X4.M32, matrix4X4.M33
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix4x4(System.Numerics.Matrix4x4 matrix4X4)
    {
        return new Matrix4x4(
            matrix4X4.M11, matrix4X4.M12, matrix4X4.M13, matrix4X4.M14,
            matrix4X4.M21, matrix4X4.M22, matrix4X4.M23, matrix4X4.M24,
            matrix4X4.M31, matrix4X4.M32, matrix4X4.M33, matrix4X4.M34,
            matrix4X4.M41, matrix4X4.M42, matrix4X4.M43, matrix4X4.M44
        );
    }
}