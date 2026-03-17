using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix4x4Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Operation_Addition
{
    [Test]
    public void Addition_Scalar()
    {
        var a = new Matrix4x4(
            1,  2,  3,  4,
            5,  6,  7,  8,
            9, 10, 11, 12,
            13, 14, 15, 16
        );
        
        var expected = new Matrix4x4(
             5,  6,  7,  8,
             9, 10, 11, 12,
            13, 14, 15, 16,
            17, 18, 19, 20
        );

        AssertAreEqual(a + 8, 8 + a);
        AssertAreEqual(a + 4, expected);
    }
    
    [Test]
    public void Addition_Matrix4x4()
    {
        var a = new Matrix4x4(
             1,  2,  3,  4,
             5,  6,  7,  8,
             9, 10, 11, 12,
            13, 14, 15, 16
        );

        var b = new Matrix4x4(
            16, 15, 14, 13,
            12, 11, 10,  9,
             8,  7,  6,  5,
             4,  3,  2,  1
        );

        var expected = new Matrix4x4(
            17, 17, 17, 17,
            17, 17, 17, 17,
            17, 17, 17, 17,
            17, 17, 17, 17
        );

        AssertAreEqual(a + b, expected);
    }
}