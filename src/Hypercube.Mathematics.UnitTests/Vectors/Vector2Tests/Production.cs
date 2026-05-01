using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Vectors.Vector2Tests;

[TestFixture]
public sealed class Production
{
    private static readonly object[] Cases =
    [
        new object[] { new Vector2(0f, 0f), 0f },
        new object[] { new Vector2(2f, 3f), 6f },
        new object[] { new Vector2(-2f, 3f), -6f },
        new object[] { new Vector2(-2f, -3f), 6f },
        new object[] { new Vector2(1f, 1f), 1f },
        new object[] { new Vector2(0.5f, 0.5f), 0.25f },
        new object[] { new Vector2(1000f, 2000f), 2_000_000f },
        new object[] { new Vector2(float.Epsilon, float.Epsilon), float.Epsilon * float.Epsilon },
        new object[] { new Vector2(float.MaxValue, 0f), 0f },
        new object[] { new Vector2(float.MinValue, 0f), 0f }
    ];

    [TestCaseSource(nameof(Cases))]
    public void Production_ReturnsExpected(Vector2 v, float expected)
    {
        Assert.That(v.Production, Is.EqualTo(expected));
    }

    [Test]
    public void Production_OrderIndependent()
    {
        var a = new Vector2(2f, 3f);
        var b = new Vector2(3f, 2f);
        Assert.That(a.Production, Is.EqualTo(b.Production));
    }

    [Test]
    public void Production_With_Zero_Is_Zero()
    {
        var v = new Vector2(0f, 999f);
        Assert.That(v.Production, Is.Zero);
    }

    [Test]
    public void Production_With_NaN_Propagates()
    {
        var v = new Vector2(float.NaN, 1f);
        Assert.That(float.IsNaN(v.Production), Is.True);
    }

    [Test]
    public void Production_With_Infinity()
    {
        var v = new Vector2(float.PositiveInfinity, 2f);
        Assert.That(float.IsPositiveInfinity(v.Production), Is.True);
    }
}
