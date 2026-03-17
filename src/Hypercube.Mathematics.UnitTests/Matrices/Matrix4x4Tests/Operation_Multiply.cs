using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix4x4Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Operation_Multiply
{
    [Test]
    public void Multiply_Scalar()
    {
        var a = new Matrix4x4(
             1,  2,  3,  4,
             5,  6,  7,  8,
             9, 10, 11, 12,
            13, 14, 15, 16
        );

        var expected = new Matrix4x4(
             2,  4,  6,  8,
            10, 12, 14, 16,
            18, 20, 22, 24,
            26, 28, 30, 32
        );

        AssertAreEqual(a * 2, expected);
    }

    [Test]
    public void Multiply_Vector2()
    {
        var transform =
            Matrix4x4.CreateScale(2, 3, 1) *
            Matrix4x4.CreateTranslation(1, 2, 0);
        
        var vector = new Vector2(1, 1);
        var excepted = new Vector2(4, 9);
        AssertAreEqual(transform * vector, excepted);
    }

    [Test]
    public void Multiply_Vector3()
    {
        var transform =
            Matrix4x4.CreateScale(2, 3, 4) *
            Matrix4x4.CreateTranslation(1, 2, 3);
        
        var vector = new Vector3(1, 1, 1);
        var excepted = new Vector3(4, 9, 16);
        AssertAreEqual(transform * vector, excepted);
    }

    [Test]
    public void Multiply_Vector4()
    {
        var translation = new Matrix4x4(
            1, 0, 0, 10,
            0, 1, 0, 20,
            0, 0, 1, 30,
            0, 0, 0, 1
        );

        var scale = new Matrix4x4(
            2, 0, 0, 0,
            0, 3, 0, 0,
            0, 0, 4, 0,
            0, 0, 0, 1
        );

        AssertAreEqual(translation * new Vector4(1, 2, 3, 1), new Vector4(11, 22, 33, 1));
        AssertAreEqual(scale * new Vector4(1, 2, 3, 1), new Vector4(2, 6, 12, 1));
    }

    [Test]
    public void Multiply_Indentity_Vector4()
    {
        var original = new Vector4(1, 2, 3, 4);
        AssertAreEqual(Matrix4x4.Identity * original, original);
    }

    [Test]
    public void Multiply_Matrix4x4()
    {
        var a = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 8, 7, 6,
            5, 4, 3, 2
        );

        var b = new Matrix4x4(
            0,  1,  2,  3,
            4,  5,  6,  7,
            8,  9, 10, 11,
            12, 13, 14, 15
        );

        var expected = new Matrix4x4(
            80,  90, 100, 110,
            176, 202, 228, 254,
            160, 190, 220, 250,
            64,  78,  92, 106
        );

        AssertAreEqual(a * b, expected);
    }
}
