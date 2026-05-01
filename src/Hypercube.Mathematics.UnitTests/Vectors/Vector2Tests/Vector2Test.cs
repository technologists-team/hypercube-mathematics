using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Vectors.Vector2Tests;

// TODO: separate on different tests

[TestFixture]
public class Vector2Tests
{
    [Test]
    public void AspectRatio()
    {
        var vector = new Vector2(16, 9);
        
        Assert.That(vector.AspectRatio, Is.EqualTo(16f / 9f));
    }

    [Test]
    public void Normalized()
    {
        var vector = new Vector2(3f, 4f).Normalized;
        
        Assert.That(float.Abs(vector.Length - 1f), Is.LessThan(1e-3));
    }

    [Test]
    public void Arithmetic_Operators()
    {
        var a = new Vector2(2, 3);
        var b = new Vector2(4, 1);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(a + b, Is.EqualTo(new Vector2(6f, 4f)));
            Assert.That(a - b, Is.EqualTo(new Vector2(-2f, 2f)));
            Assert.That(a * b, Is.EqualTo(new Vector2(8f, 3f)));
            Assert.That(a / b, Is.EqualTo(new Vector2(0.5f, 3f)));   
        }
    }

    [Test]
    public void Negation_Operator()
    {
        var vector = new Vector2(2, -3);
        
        Assert.That(-vector, Is.EqualTo(new Vector2(-2, 3)));
    }

    [Test]
    public void Scalar_Operators()
    {
        var vector = new Vector2(2, -4);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(vector + 2, Is.EqualTo(new Vector2(4, -2)));
            Assert.That(vector - 1, Is.EqualTo(new Vector2(1, -5)));
            Assert.That(vector * 2, Is.EqualTo(new Vector2(4, -8)));
            Assert.That(vector / 2, Is.EqualTo(new Vector2(1, -2)));   
        }
    }

    [Test]
    public void Dot()
    {
        var a = new Vector2(1, 2);
        var b = new Vector2(3, 4);
        
        Assert.That(Vector2.Dot(a, b), Is.EqualTo(11));
    }

    [Test]
    public void Distance_DistanceSquared()
    {
        var a = new Vector2(0, 0);
        var b = new Vector2(3, 4);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(Vector2.DistanceSquared(a, b), Is.EqualTo(25f).Within(1e-3));
            Assert.That(Vector2.Distance(a, b), Is.EqualTo(5f).Within(1e-3));   
        }
    }

    [Test]
    public void Reflect()
    {
        var vector = new Vector2(1, -1);
        var normal = new Vector2(0, 1);
        
        Assert.That(Vector2.Reflect(vector, normal), Is.EqualTo(new Vector2(1, 1)));
    }
    
    [Test]
    public void ToStringConverting()
    {
        var vector = new Vector2(1.23f, 4.56f);
        
        Assert.That(vector.ToString(), Is.EqualTo("1.23, 4.56"));
    }
}