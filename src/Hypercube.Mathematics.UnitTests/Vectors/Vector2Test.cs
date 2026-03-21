using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Vectors;

[TestFixture]
public class Vector2Tests
{
    [Test]
    public void Constants()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Vector2.Zero, Is.EqualTo(new Vector2(0)));
            Assert.That(Vector2.One, Is.EqualTo(new Vector2(1)));
            Assert.That(Vector2.UnitX, Is.EqualTo(new Vector2(1, 0)));
            Assert.That(Vector2.UnitY, Is.EqualTo(new Vector2(0, 1)));
        }
    }

    [Test]
    public void Constructor()
    {
        var vector = new Vector2(3.5f, -2f);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(vector.X, Is.EqualTo(3.5f));
            Assert.That(vector.Y, Is.EqualTo(-2f));   
        }
    }

    [Test]
    public void Constructor_SingleValue()
    {
        var vector = new Vector2(5f);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(vector.X, Is.EqualTo(5f));
            Assert.That(vector.Y, Is.EqualTo(5f));   
        }
    }

    [Test]
    public void Indexer()
    {
        var vector = new Vector2(3f, 4f);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(vector[0], Is.EqualTo(3f));
            Assert.That(vector[1], Is.EqualTo(4f));
            Assert.Throws<ArgumentOutOfRangeException>(() => { _ = vector[2]; });   
        }
    }

    [Test]
    public void Length_LengthSquared()
    {
        var vector = new Vector2(3f, 4f);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(vector.Length, Is.EqualTo(5f).Within(1e-3));
            Assert.That(vector.LengthSquared, Is.EqualTo(25f).Within(1e-3));   
        }
    }

    [Test]
    public void Summation_Production()
    {
        var vector = new Vector2(2f, 3f);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(vector.Summation, Is.EqualTo(5f));
            Assert.That(vector.Production, Is.EqualTo(6f));   
        }
    }

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
    public void Clamp()
    {
        var vector = new Vector2(5, -2);
        
        Assert.That(vector.Clamp(new Vector2(0), new Vector2(3)), Is.EqualTo(new Vector2(3, 0)));
    }

    [Test]
    public void GetEnumerator()
    {
        var vector = new Vector2(10, 20);
        using var enumerator = vector.GetEnumerator();

        using (Assert.EnterMultipleScope())
        {
            enumerator.MoveNext();
            Assert.That(enumerator.Current, Is.EqualTo(10f));
            enumerator.MoveNext();
            Assert.That(enumerator.Current, Is.EqualTo(20f));
        }
    }

    [Test]
    public void ToStringConverting()
    {
        var vector = new Vector2(1.23f, 4.56f);
        
        Assert.That(vector.ToString(), Is.EqualTo("1.23, 4.56"));
    }
}