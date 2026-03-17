using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix4x4Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Method_Index_And_Get
{
    [Test]
    public void Get_Unsafe()
    {
        var matrix = new Matrix4x4(
            17, 20, 30, 11,
            40, 52, 24, 12,
            37, 42, 28, 13,
            45, 48, 36, 14
        );
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(matrix.Get(0, 0), Is.EqualTo(17));
            Assert.That(matrix.Get(1, 0), Is.EqualTo(20));
            Assert.That(matrix.Get(2, 0), Is.EqualTo(30));
            Assert.That(matrix.Get(3, 0), Is.EqualTo(11));

            Assert.That(matrix.Get(0, 1), Is.EqualTo(40));
            Assert.That(matrix.Get(1, 1), Is.EqualTo(52));
            Assert.That(matrix.Get(2, 1), Is.EqualTo(24));
            Assert.That(matrix.Get(3, 1), Is.EqualTo(12));

            Assert.That(matrix.Get(0, 2), Is.EqualTo(37));
            Assert.That(matrix.Get(1, 2), Is.EqualTo(42));
            Assert.That(matrix.Get(2, 2), Is.EqualTo(28));
            Assert.That(matrix.Get(3, 2), Is.EqualTo(13));

            Assert.That(matrix.Get(0, 3), Is.EqualTo(45));
            Assert.That(matrix.Get(1, 3), Is.EqualTo(48));
            Assert.That(matrix.Get(2, 3), Is.EqualTo(36));
            Assert.That(matrix.Get(3, 3), Is.EqualTo(14));
        }
    }

    [Test]
    public void Get_Indexer()
    {
        var matrix = new Matrix4x4(
            17, 20, 30, 11,
            40, 52, 24, 12,
            37, 42, 28, 13,
            45, 48, 36, 14
        );
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(matrix[0, 0], Is.EqualTo(17));
            Assert.That(matrix[1, 0], Is.EqualTo(20));
            Assert.That(matrix[2, 0], Is.EqualTo(30));
            Assert.That(matrix[3, 0], Is.EqualTo(11));

            Assert.That(matrix[0, 1], Is.EqualTo(40));
            Assert.That(matrix[1, 1], Is.EqualTo(52));
            Assert.That(matrix[2, 1], Is.EqualTo(24));
            Assert.That(matrix[3, 1], Is.EqualTo(12));

            Assert.That(matrix[0, 2], Is.EqualTo(37));
            Assert.That(matrix[1, 2], Is.EqualTo(42));
            Assert.That(matrix[2, 2], Is.EqualTo(28));
            Assert.That(matrix[3, 2], Is.EqualTo(13));

            Assert.That(matrix[0, 3], Is.EqualTo(45));
            Assert.That(matrix[1, 3], Is.EqualTo(48));
            Assert.That(matrix[2, 3], Is.EqualTo(36));
            Assert.That(matrix[3, 3], Is.EqualTo(14));
        }
    }
}
