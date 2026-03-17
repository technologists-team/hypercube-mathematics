using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix3x3Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Operation_Multiply
{
    [Test]
    public void Multiply_Scalar()
    {
        var a = new Matrix3x3(
            1, 2, 3,
            4, 5, 6,
            7, 8, 9
        );

        var expected = new Matrix3x3(
             2,  4,  6,
             8, 10, 12,
            14, 16, 18
        );

        AssertAreEqual(a * 2, expected);
    }

    [Test]
    public void Multiply_Vector3()
    {
        var matrix = new Matrix3x3(
            1, 0, 10,
            0, 1, 20,
            0, 0,  1
        );
        
        AssertAreEqual(matrix * new Vector3(1, 2, 1), new Vector3(11, 22, 1));
    }

    [Test]
    public void Multiply_Vector3_Scale()
    {
        var scale = new Matrix3x3(
            2, 0, 0,
            0, 3, 0,
            0, 0, 4
        );
        
        AssertAreEqual(scale * new Vector3(1, 2, 3), new Vector3(2, 6, 12));
    }

    [Test]
    public void Multiply_Indentity_Vector3()
    {
        var original = new Vector3(1, 2, 3);
        AssertAreEqual(Matrix3x3.Identity * original, original);
    }

    [Test]
    public void Multiply_Matrix3x3()
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
             30,  24, 18,
             84 , 69, 54,
            138, 114, 90
        );
        
        AssertAreEqual(a * b, expected);
    }
}