using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix3x3Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Method_Index_And_Get
{
    [Test]
    public void Get_Unsafe()
    {
        var matrix = new Matrix3x3(
            17, 20, 30,
            40, 52, 24,
            37, 42, 28
        );
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(matrix.Get(0, 0), Is.EqualTo(17));
            Assert.That(matrix.Get(1, 0), Is.EqualTo(20));
            Assert.That(matrix.Get(2, 0), Is.EqualTo(30));

            Assert.That(matrix.Get(0, 1), Is.EqualTo(40));
            Assert.That(matrix.Get(1, 1), Is.EqualTo(52));
            Assert.That(matrix.Get(2, 1), Is.EqualTo(24));

            Assert.That(matrix.Get(0, 2), Is.EqualTo(37));
            Assert.That(matrix.Get(1, 2), Is.EqualTo(42));
            Assert.That(matrix.Get(2, 2), Is.EqualTo(28));
        }
    }

    [Test]
    public void Get_Indexer()
    {
        var matrix = new Matrix3x3(
            17, 20, 30,
            40, 52, 24,
            37, 42, 28
        );
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(matrix[0, 0], Is.EqualTo(17));
            Assert.That(matrix[1, 0], Is.EqualTo(20));
            Assert.That(matrix[2, 0], Is.EqualTo(30));

            Assert.That(matrix[0, 1], Is.EqualTo(40));
            Assert.That(matrix[1, 1], Is.EqualTo(52));
            Assert.That(matrix[2, 1], Is.EqualTo(24));

            Assert.That(matrix[0, 2], Is.EqualTo(37));
            Assert.That(matrix[1, 2], Is.EqualTo(42));
            Assert.That(matrix[2, 2], Is.EqualTo(28));
        }
    }
}