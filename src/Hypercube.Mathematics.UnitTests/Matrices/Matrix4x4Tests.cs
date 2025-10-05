using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Matrices;

[TestFixture]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Matrix4x4Tests
{
    [Test]
    public void Multiply_IdentityMatrix()
    {
        var original = new Vector4(1, 2, 3, 4);
        var result = Matrix4x4.Identity * original;
        AssertAreEqual(result, original);
    }

    [Test]
    public void Multiply_TranslationMatrix_Vector4()
    {
        var result = new Matrix4x4(
            1, 0, 0, 10,
            0, 1, 0, 20,
            0, 0, 1, 30,
            0, 0, 0, 1
        ) * new Vector4(1, 2, 3, 1);
        AssertAreEqual(result, new Vector4(11, 22, 33, 1));
    }

    [Test]
    public void Multiply_ScaleMatrix_Vector4()
    {
        var result = new Matrix4x4(
            2, 0, 0, 0,
            0, 3, 0, 0,
            0, 0, 4, 0,
            0, 0, 0, 1
        ) * new Vector4(1, 2, 3, 1);
        AssertAreEqual(result, new Vector4(2, 6, 12, 1));
    }
    
    [Test]
    public void Addition_AddTwoMatrices()
    {
        var a = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16
        );

        var b = new Matrix4x4(
            16, 15, 14, 13,
            12, 11, 10, 9,
            8, 7, 6, 5,
            4, 3, 2, 1
        );

        var expected = new Matrix4x4(
            17, 17, 17, 17,
            17, 17, 17, 17,
            17, 17, 17, 17,
            17, 17, 17, 17
        );

        var result = a + b;
        AssertAreEqual(expected, result);
    }

    [Test]
    public void Multiply_Scalar()
    {
        var a = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16
        );
        
        var expected = new Matrix4x4(
            2, 4, 6, 8,
            10, 12, 14, 16,
            18, 20, 22, 24,
            26, 28, 30, 32
        );

        var result = a * 2;
        AssertAreEqual(expected, result);
    }

    [Test]
    public void Multiply_Matrix4x4()
    {
        var a = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 8, 7, 6,
            5, 4, 3, 2);

        var b = new Matrix4x4(
            0, 1, 2, 3,
            4, 5, 6, 7,
            8, 9, 10, 11,
            12, 13, 14, 15);

        var expected = new Matrix4x4(
            80, 90, 100, 110,
            176, 202, 228, 254,
            160, 190, 220, 250,
            64, 78, 92, 106
        );

        var result = a * b;
        AssertAreEqual(expected, result);
    }

    [Test]
    public void Equals_SameValues()
    {
        var a = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16
        );

        var b = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16
        );

        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    public void Equals_DifferentValues()
    {
        var a = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16
        );

        var b = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 17
        );
        
        Assert.That(a, Is.Not.EqualTo(b));
    }

    [Test]
    public void Multiply_Vector4()
    {
        var expected = new Vector4(11, 22, 33, 1);
        var v = new Vector4(1, 2, 3, 1);
        var m = new Matrix4x4(
            1, 0, 0, 10,
            0, 1, 0, 20,
            0, 0, 1, 30,
            0, 0, 0, 1
        );

        var result = m * v;
        AssertAreEqual(expected, result);
    }
    
    private static void AssertAreEqual(Matrix4x4 expected, Matrix4x4 actual, float delta = HyperMath.FloatTolerance)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.M00, Is.EqualTo(expected.M00).Within(delta));
            Assert.That(actual.M01, Is.EqualTo(expected.M01).Within(delta));
            Assert.That(actual.M02, Is.EqualTo(expected.M02).Within(delta));
            Assert.That(actual.M03, Is.EqualTo(expected.M03).Within(delta));
            Assert.That(actual.M10, Is.EqualTo(expected.M10).Within(delta));
            Assert.That(actual.M11, Is.EqualTo(expected.M11).Within(delta));
            Assert.That(actual.M12, Is.EqualTo(expected.M12).Within(delta));
            Assert.That(actual.M13, Is.EqualTo(expected.M13).Within(delta));
            Assert.That(actual.M20, Is.EqualTo(expected.M20).Within(delta));
            Assert.That(actual.M21, Is.EqualTo(expected.M21).Within(delta));
            Assert.That(actual.M22, Is.EqualTo(expected.M22).Within(delta));
            Assert.That(actual.M23, Is.EqualTo(expected.M23).Within(delta));
            Assert.That(actual.M30, Is.EqualTo(expected.M30).Within(delta));
            Assert.That(actual.M31, Is.EqualTo(expected.M31).Within(delta));
            Assert.That(actual.M32, Is.EqualTo(expected.M32).Within(delta));
            Assert.That(actual.M33, Is.EqualTo(expected.M33).Within(delta));
        }
    }
    
    private static void AssertAreEqual(Vector4 expected, Vector4 actual, float delta = HyperMath.FloatTolerance)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(delta));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(delta));
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(delta));
            Assert.That(actual.W, Is.EqualTo(expected.W).Within(delta));
        }
    }
}