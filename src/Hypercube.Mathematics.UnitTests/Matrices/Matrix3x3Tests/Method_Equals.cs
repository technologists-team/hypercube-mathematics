using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix3x3Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Method_Equals
{
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
}