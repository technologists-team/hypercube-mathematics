using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix4x4Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Accessor_Data
{
    [Test]
    public void Array_Get()
    {
        var matrix = new Matrix4x4(
             1,  2,  3,  4,
             5,  6,  7,  8,
             9, 10, 11, 12,
            13, 14, 15, 16
        );

        float[] expected =
        [
             1,  2,  3,  4,
             5,  6,  7,  8,
             9, 10, 11, 12,
            13, 14, 15, 16
        ];

        var actual = matrix.Array;
        
        Assert.That(actual, Has.Length.EqualTo(Matrix4x4.Length));
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Array_Is_Not_Reference()
    {
        const float newValue = 100f;
        
        var matrix = Matrix4x4.Identity;
        var actual = matrix.Array;

        actual[0] = newValue;

        Assert.That(matrix.M00, Is.Not.EqualTo(newValue));
    }
    
    [Test]
    public void Span_Get()
    {
        var matrix = new Matrix4x4(
             1,  2,  3,  4,
             5,  6,  7,  8,
             9, 10, 11, 12,
            13, 14, 15, 16
        );
        
        var actual = matrix.Span;
        Assert.That(actual.Length, Is.EqualTo(Matrix4x4.Length));
        
        using (Assert.EnterMultipleScope())
        {
            for (var i = 0; i < actual.Length; i++)
            {
                Assert.That(actual[i], Is.EqualTo(matrix[i]));
            }
        }
    }

    [Test]
    public void UnsafeSpan_Is_Reference()
    {
        const float newValue = 100f;
        
        var matrix = Matrix4x4.Identity;
        var actual = matrix.UnsafeSpan;
        
        // It's fucking cursed, we have immutable struct,
        // but can mutate with span, lmao
        actual[0] = newValue;

        Assert.That(matrix.M00, Is.EqualTo(newValue));
    }
}