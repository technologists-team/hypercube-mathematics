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
        using (Assert.EnterMultipleScope())
        {
            Assert.That(matrix.M00, Is.Zero);
            Assert.That(matrix.M01, Is.Zero);
            Assert.That(matrix.M10, Is.Zero);
            Assert.That(matrix.M11, Is.Zero);
            Assert.That(matrix.M20, Is.Zero);
            Assert.That(matrix.M21, Is.Zero);
        }
    }

    [Test]
    public void OneMatrix()
    {
        var matrix = Matrix3x2.One;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(matrix.M00, Is.EqualTo(1));
            Assert.That(matrix.M01, Is.EqualTo(1));
            Assert.That(matrix.M10, Is.EqualTo(1));
            Assert.That(matrix.M11, Is.EqualTo(1));
            Assert.That(matrix.M20, Is.EqualTo(1));
            Assert.That(matrix.M21, Is.EqualTo(1));
        }
    }

    [Test]
    public void IdentityMatrix()
    {
        var matrix = Matrix3x2.Identity;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(matrix.M00, Is.EqualTo(1));
            Assert.That(matrix.M01, Is.Zero);
            Assert.That(matrix.M10, Is.Zero);
            Assert.That(matrix.M11, Is.EqualTo(1));
            Assert.That(matrix.M20, Is.Zero);
            Assert.That(matrix.M21, Is.Zero);
        }
    }

    [Test]
    public void CreateTranslation()
    {
        var matrix = Matrix3x2.CreateTranslation(3, 5);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(matrix.M00, Is.EqualTo(1));
            Assert.That(matrix.M01, Is.Zero);
            Assert.That(matrix.M10, Is.Zero);
            Assert.That(matrix.M11, Is.EqualTo(1));
            Assert.That(matrix.M20, Is.EqualTo(3));
            Assert.That(matrix.M21, Is.EqualTo(5));
        }
    }

    [Test]
    public void CreateScale()
    {
        var matrix = Matrix3x2.CreateScale(2, 3);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(matrix.M00, Is.EqualTo(2));
            Assert.That(matrix.M01, Is.Zero);
            Assert.That(matrix.M10, Is.Zero);
            Assert.That(matrix.M11, Is.EqualTo(3));
            Assert.That(matrix.M20, Is.Zero);
            Assert.That(matrix.M21, Is.Zero);
        }
    }

    [Test]
    public void CreateRotation()
    {
        var matrix = Matrix3x2.CreateRotation(HyperMath.PIOver2);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(matrix.M00, Is.EqualTo(0f).Within(1e-5));
            Assert.That(matrix.M01, Is.EqualTo(1f).Within(1e-5));
            Assert.That(matrix.M10, Is.EqualTo(-1f).Within(1e-5));
            Assert.That(matrix.M11, Is.EqualTo(0f).Within(1e-5));
        }
    }

    [Test]
    public void TransformVector2()
    {
        var result = new Matrix3x2(
            2, 0, 
            0, 3, 
            1, 1
        ) * Vector2.One;

        // Expected: (1 * 2 + 1 * 0 + 1, 1 * 0 + 1 * 3 + 1) = (3, 4)
        Assert.That(result, Is.EqualTo(new Vector2(3, 4)));
    }
}