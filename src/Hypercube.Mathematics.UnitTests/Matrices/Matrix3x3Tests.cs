using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Matrices;

[TestFixture]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Matrix3x3Tests
{
    [Test]
    public void Multiply_IdentityMatrix()
    {
        var original = new Vector3(1, 2, 3);
        var result = Matrix3x3.Identity * original;
        AssertAreEqual(original, result);
    }

    [Test]
    public void Multiply_ScaleMatrix_Vector3()
    {
        var scale = new Matrix3x3(
            2, 0, 0,
            0, 3, 0,
            0, 0, 4
        );

        var result = scale * new Vector3(1, 2, 3);
        AssertAreEqual(new Vector3(2, 6, 12), result);
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
            30, 24, 18,
            84, 69, 54,
            138, 114, 90
        );

        var result = a * b;
        AssertAreEqual(expected, result);
    }

    [Test]
    public void Addition_AddTwoMatrices()
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

        var result = a + b;
        AssertAreEqual(expected, result);
    }

    [Test]
    public void Multiply_Scalar()
    {
        var a = new Matrix3x3(
            1, 2, 3,
            4, 5, 6,
            7, 8, 9
        );

        var expected = new Matrix3x3(
            2, 4, 6,
            8, 10, 12,
            14, 16, 18
        );

        var result = a * 2;
        AssertAreEqual(expected, result);
    }

    [Test]
    public void Equals_SameValues()
    {
        var a = new Matrix3x3(
            1, 2, 3,
            4, 5, 6,
            7, 8, 9
        );

        var b = new Matrix3x3(
            1, 2, 3,
            4, 5, 6,
            7, 8, 9
        );

        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    public void Equals_DifferentValues()
    {
        var a = new Matrix3x3(
            1, 2, 3,
            4, 5, 6,
            7, 8, 9
        );

        var b = new Matrix3x3(
            1, 2, 3,
            4, 5, 6,
            7, 8, 10
        );

        Assert.That(a, Is.Not.EqualTo(b));
    }

    [Test]
    public void Multiply_Vector3()
    {
        var m = new Matrix3x3(
            1, 0, 10,
            0, 1, 20,
            0, 0, 1
        );

        var v = new Vector3(1, 2, 1);
        var result = m * v;

        AssertAreEqual(new Vector3(11, 22, 1), result);
    }

    private static void AssertAreEqual(Matrix3x3 expected, Matrix3x3 actual, float delta = HyperMath.FloatTolerance)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.M00, Is.EqualTo(expected.M00).Within(delta));
            Assert.That(actual.M01, Is.EqualTo(expected.M01).Within(delta));
            Assert.That(actual.M02, Is.EqualTo(expected.M02).Within(delta));
            Assert.That(actual.M10, Is.EqualTo(expected.M10).Within(delta));
            Assert.That(actual.M11, Is.EqualTo(expected.M11).Within(delta));
            Assert.That(actual.M12, Is.EqualTo(expected.M12).Within(delta));
            Assert.That(actual.M20, Is.EqualTo(expected.M20).Within(delta));
            Assert.That(actual.M21, Is.EqualTo(expected.M21).Within(delta));
            Assert.That(actual.M22, Is.EqualTo(expected.M22).Within(delta));
        }
    }

    private static void AssertAreEqual(Vector3 expected, Vector3 actual, float delta = HyperMath.FloatTolerance)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(delta));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(delta));
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(delta));
        }
    }
}