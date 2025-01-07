using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Hypercube.Mathematics.Matrices;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial struct Matrix3x2
{
    /*
     * System.Numerics Compatibility
     */

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Matrix3x2(Matrix3x2 matrix4X4)
    {
        return new System.Numerics.Matrix3x2(
            matrix4X4.M00, matrix4X4.M01,
            matrix4X4.M10, matrix4X4.M11,
            matrix4X4.M20, matrix4X4.M21
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix3x2(System.Numerics.Matrix3x2 matrix4X4)
    {
        return new Matrix3x2(
            matrix4X4.M11, matrix4X4.M12,
            matrix4X4.M21, matrix4X4.M22,
            matrix4X4.M31, matrix4X4.M32
        );
    }
}