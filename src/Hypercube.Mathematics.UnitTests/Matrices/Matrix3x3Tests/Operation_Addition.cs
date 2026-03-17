using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix3x3Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Operation_Addition
{
    [Test]
    public void Addition_Scalar()
    {
        var a = new Matrix3x3(
            1,  2,  3,
            5,  6,  7,
            9, 10, 11
        );
        
        var expected = new Matrix3x3(
            5,  6,  7,
            9, 10, 11,
            13, 14, 15
        );

        AssertAreEqual(a + 8, 8 + a);
        AssertAreEqual(a + 4, expected);
    }
    
    [Test]
    public void Addition_Matrix3x3()
    {
        var a = new Matrix3x3(
            1, 2, 3,
            4, 5, 6,
            7, 8, 9
        );

        var b = new Matrix3x3(
            9, 8, 7,
            6, 5, 4,
            3, 2, 1
        );

        var expected = new Matrix3x3(
            10, 10, 10,
            10, 10, 10,
            10, 10, 10
        );
        
        AssertAreEqual(a + b, expected);
    }
}