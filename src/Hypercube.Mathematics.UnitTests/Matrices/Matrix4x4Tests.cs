using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Matrices;

[TestFixture]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Matrix4x4Tests
{
    [Test]
    public void Multiply_IdentityMatrix_VectorEqualsOriginal()
    {
        var identity = Matrix4x4.Identity;
        var vector = new Vector4(1, 2, 3, 4);
        
        var result = identity * vector;
        Assert.Multiple(() =>
        {
            Assert.That(result.X, Is.EqualTo(vector.X).Within(1e-5));
            Assert.That(result.Y, Is.EqualTo(vector.Y).Within(1e-5));
            Assert.That(result.Z, Is.EqualTo(vector.Z).Within(1e-5));
            Assert.That(result.W, Is.EqualTo(vector.W).Within(1e-5));
        });
    }

    [Test]
    public void Multiply_TranslationMatrix_VectorTranslated()
    {
        var translation = new Matrix4x4(
            1, 0, 0, 10,
            0, 1, 0, 20,
            0, 0, 1, 30,
            0, 0, 0, 1
        );
        
        var vector = new Vector4(1, 2, 3, 1);
        var result = translation * vector;
        Assert.Multiple(() =>
        {
            Assert.That(result.X, Is.EqualTo(11).Within(1e-5));
            Assert.That(result.Y, Is.EqualTo(22).Within(1e-5));
            Assert.That(result.Z, Is.EqualTo(33).Within(1e-5));
            Assert.That(result.W, Is.EqualTo(1).Within(1e-5));
        });
    }

    [Test]
    public void Multiply_ScalingMatrix_VectorScaled()
    {
        var scaling = new Matrix4x4(
            2, 0, 0, 0,
            0, 3, 0, 0,
            0, 0, 4, 0,
            0, 0, 0, 1
        );

        var vector = new Vector4(1, 2, 3, 1);
        var result = scaling * vector;
        Assert.Multiple(() =>
        {
            Assert.That(result.X, Is.EqualTo(2).Within(1e-5));
            Assert.That(result.Y, Is.EqualTo(6).Within(1e-5));
            Assert.That(result.Z, Is.EqualTo(12).Within(1e-5));
            Assert.That(result.W, Is.EqualTo(1).Within(1e-5));
        });
    }
    
    [Test]
    public void Addition_AddTwoMatrices_ReturnsCorrectResult()
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
        AssertMatricesAreEqual(expected, result);
    }

    [Test]
    public void Multiply_Scalar_ReturnsScaledMatrix()
    {
        var a = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16);

        const float scalar = 2f;
        var expected = new Matrix4x4(
            2, 4, 6, 8,
            10, 12, 14, 16,
            18, 20, 22, 24,
            26, 28, 30, 32);

        var result = a * scalar;
        AssertMatricesAreEqual(expected, result);
    }

    [Test]
    public void Multiply_Matrix_ReturnsCorrectResult()
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
        AssertMatricesAreEqual(expected, result);
    }

    [Test]
    public void Equals_SameValues_ReturnsTrue()
    {
        var a = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16);

        var b = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16);

        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var a = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 16);

        var b = new Matrix4x4(
            1, 2, 3, 4,
            5, 6, 7, 8,
            9, 10, 11, 12,
            13, 14, 15, 17);
        
        Assert.That(a, Is.Not.EqualTo(b));
    }

    [Test]
    public void Multiply_Vector4_ReturnsCorrectResult()
    {
        var m = new Matrix4x4(
            1, 0, 0, 10,
            0, 1, 0, 20,
            0, 0, 1, 30,
            0, 0, 0, 1);

        var v = new Vector4(1, 2, 3, 1);
        var expected = new Vector4(11, 22, 33, 1);
        var result = m * v;
        AssertVectorsAreEqual(expected, result);
    }
    
    private static void AssertMatricesAreEqual(Matrix4x4 expected, Matrix4x4 actual, float delta = 1e-5f)
    {
        Assert.Multiple(() =>
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
        });
    }
    
    private static void AssertVectorsAreEqual(Vector4 expected, Vector4 actual, float delta = 1e-5f)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(delta));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(delta));
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(delta));
            Assert.That(actual.W, Is.EqualTo(expected.W).Within(delta));
        });
    }
}