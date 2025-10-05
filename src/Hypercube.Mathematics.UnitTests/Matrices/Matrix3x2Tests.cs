using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Matrices;

[TestFixture]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Matrix3x2Tests
{
    [Test]
    public void ZeroMatrix_HasAllZeroes()
    {
        var matrix = Matrix3x2.Zero;

        Assert.Multiple(() =>
        {
            Assert.That(matrix.M00, Is.EqualTo(0f));
            Assert.That(matrix.M01, Is.EqualTo(0f));
            Assert.That(matrix.M10, Is.EqualTo(0f));
            Assert.That(matrix.M11, Is.EqualTo(0f));
            Assert.That(matrix.M20, Is.EqualTo(0f));
            Assert.That(matrix.M21, Is.EqualTo(0f));
        });
    }

    [Test]
    public void OneMatrix_HasAllOnes()
    {
        var matrix = Matrix3x2.One;

        Assert.Multiple(() =>
        {
            Assert.That(matrix.M00, Is.EqualTo(1f));
            Assert.That(matrix.M01, Is.EqualTo(1f));
            Assert.That(matrix.M10, Is.EqualTo(1f));
            Assert.That(matrix.M11, Is.EqualTo(1f));
            Assert.That(matrix.M20, Is.EqualTo(1f));
            Assert.That(matrix.M21, Is.EqualTo(1f));
        });
    }

    [Test]
    public void IdentityMatrix_HasCorrectValues()
    {
        var matrix = Matrix3x2.Identity;

        Assert.Multiple(() =>
        {
            Assert.That(matrix.M00, Is.EqualTo(1f));
            Assert.That(matrix.M01, Is.EqualTo(0f));
            Assert.That(matrix.M10, Is.EqualTo(0f));
            Assert.That(matrix.M11, Is.EqualTo(1f));
            Assert.That(matrix.M20, Is.EqualTo(0f));
            Assert.That(matrix.M21, Is.EqualTo(0f));
        });
    }

    [Test]
    public void CreateTranslation_CorrectMatrix()
    {
        var matrix = Matrix3x2.CreateTranslation(3, 5);

        Assert.Multiple(() =>
        {
            Assert.That(matrix.M00, Is.EqualTo(1f));
            Assert.That(matrix.M01, Is.EqualTo(0f));
            Assert.That(matrix.M10, Is.EqualTo(0f));
            Assert.That(matrix.M11, Is.EqualTo(1f));
            Assert.That(matrix.M20, Is.EqualTo(3f));
            Assert.That(matrix.M21, Is.EqualTo(5f));
        });
    }

    [Test]
    public void CreateScale_CorrectMatrix()
    {
        var matrix = Matrix3x2.CreateScale(2, 3);

        Assert.Multiple(() =>
        {
            Assert.That(matrix.M00, Is.EqualTo(2f));
            Assert.That(matrix.M01, Is.EqualTo(0f));
            Assert.That(matrix.M10, Is.EqualTo(0f));
            Assert.That(matrix.M11, Is.EqualTo(3f));
            Assert.That(matrix.M20, Is.EqualTo(0f));
            Assert.That(matrix.M21, Is.EqualTo(0f));
        });
    }

    [Test]
    public void CreateRotation_90Degrees_RotatesCorrectly()
    {
        var angle = Math.PI / 2;
        var matrix = Matrix3x2.CreateRotation(angle);

        Assert.Multiple(() =>
        {
            Assert.That(matrix.M00, Is.EqualTo(0f).Within(1e-5));
            Assert.That(matrix.M01, Is.EqualTo(1f).Within(1e-5));
            Assert.That(matrix.M10, Is.EqualTo(-1f).Within(1e-5));
            Assert.That(matrix.M11, Is.EqualTo(0f).Within(1e-5));
        });
    }

    [Test]
    public void Transform_AppliesTransformation()
    {
        var matrix = new Matrix3x2(
            2, 0,  // scale X
            0, 3,  // scale Y
            1, 1   // translation
        );

        var v = new Vector2(1, 1);
        var result = matrix.Transform(v);

        // Expected: (1*2 + 1*0 + 1, 1*0 + 1*3 + 1) = (3, 4)
        Assert.That(result, Is.EqualTo(new Vector2(3, 4)));
    }
}