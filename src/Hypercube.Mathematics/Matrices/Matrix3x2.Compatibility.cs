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
    public static implicit operator System.Numerics.Matrix3x2(Matrix3x2 matrix)
    {
        return new System.Numerics.Matrix3x2(
            matrix.M00, matrix.M01,
            matrix.M10, matrix.M11,
            matrix.M20, matrix.M21
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Matrix3x2(System.Numerics.Matrix3x2 matrix)
    {
        return new Matrix3x2(
            matrix.M11, matrix.M12,
            matrix.M21, matrix.M22,
            matrix.M31, matrix.M32
        );
    }
}