using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix4x4Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Method_Math
{
    [Test]
    public void Math_Transpose()
    {
        var original = new Matrix4x4(
            1,  2,  3,  4,
            5,  6,  7,  8,
            9, 10, 11, 12,
            13, 14, 15, 16
        );

        var expected = new Matrix4x4(
            1, 5,  9, 13,
            2, 6, 10, 14,
            3, 7, 11, 15,
            4, 8, 12, 16
        );

        AssertAreEqual(original.Transposed, expected);
    }
}