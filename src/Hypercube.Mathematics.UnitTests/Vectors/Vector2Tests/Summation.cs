using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Vectors.Vector2Tests;

[TestFixture]
public sealed class Summation
{
    private static readonly object[] Cases =
    [
        new object[] { new Vector2(0f, 0f), 0f },
        new object[] { new Vector2(2f, 3f), 5f },
        new object[] { new Vector2(-2f, 3f), 1f },
        new object[] { new Vector2(-2f, -3f), -5f },
        new object[] { new Vector2(1f, 1f), 2f },
        new object[] { new Vector2(0.5f, 0.5f), 1f },
        new object[] { new Vector2(1000f, 2000f), 3000f },
        new object[] { new Vector2(float.Epsilon, float.Epsilon), 2f * float.Epsilon },
        new object[] { new Vector2(float.MaxValue, 0f), float.MaxValue },
        new object[] { new Vector2(float.MinValue, 0f), float.MinValue }
    ];

    [TestCaseSource(nameof(Cases))]
    public void Summation_ReturnsExpected(Vector2 v, float expected)
    {
        Assert.That(v.Summation, Is.EqualTo(expected));
    }

    [Test]
    public void Summation_OrderIndependent()
    {
        var a = new Vector2(2f, 3f);
        var b = new Vector2(3f, 2f);

        Assert.That(a.Summation, Is.EqualTo(b.Summation));
    }

    [Test]
    public void Summation_With_NaN_Propagates()
    {
        var v = new Vector2(float.NaN, 1f);

        Assert.That(float.IsNaN(v.Summation), Is.True);
    }

    [Test]
    public void Summation_With_Infinity()
    {
        var v = new Vector2(float.PositiveInfinity, 1f);

        Assert.That(float.IsPositiveInfinity(v.Summation), Is.True);
    }
}
