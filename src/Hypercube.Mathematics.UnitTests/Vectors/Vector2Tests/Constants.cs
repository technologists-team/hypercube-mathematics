using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Vectors.Vector2Tests;

[TestFixture]
public sealed class Constants
{
    [Test]
    public void Basic()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Vector2.Zero, Is.EqualTo(new Vector2(0, 0)));
            Assert.That(Vector2.One,  Is.EqualTo(new Vector2(1, 1)));
        }
    }
    
    [Test]
    public void Units()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Vector2.UnitX, Is.EqualTo(new Vector2(1, 0)));
            Assert.That(Vector2.UnitY, Is.EqualTo(new Vector2(0, 1)));
        }
    }
    
    [Test]
    public void NaN()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Vector2.NaN, Is.EqualTo(new Vector2(float.NaN)));
        }
    }

    [Test]
    public void Infinity()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Vector2.PositiveInfinity, Is.EqualTo(new Vector2(float.PositiveInfinity)));
            Assert.That(Vector2.NegativeInfinity, Is.EqualTo(new Vector2(float.NegativeInfinity)));
        }
    }
    
    [Test]
    public void BorderValues()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Vector2.MaxValue, Is.EqualTo(new Vector2(float.MaxValue)));
            Assert.That(Vector2.MinValue, Is.EqualTo(new Vector2(float.MinValue)));
        }
    }
    
    [Test]
    public void Identity()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Vector2.AdditiveIdentity, Is.EqualTo(Vector2.Zero));
            Assert.That(Vector2.MultiplicativeIdentity, Is.EqualTo(new Vector2(1)));
        }
    }
}