using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix4x4Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Method_Equals
{
    [Test]
    public void Equals_SameValues()
    {
        var a = Matrix4x4.Identity;
        var b = new Matrix4x4(a);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    public void Equals_DifferentValues()
    {
        Assert.That(Matrix4x4.Identity, Is.Not.EqualTo(Matrix4x4.Zero));
    }
}
