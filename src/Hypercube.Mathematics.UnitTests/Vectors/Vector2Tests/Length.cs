using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Vectors.Vector2Tests;

[TestFixture]
public sealed class Length
{
    // Floating-point math is inherently imprecise.
    // Use absolute tolerance for exact algorithms.
    // Use percentage tolerance for approximate or magnitude-dependent results.
    private const float Tolerance = 1e-5f;
    private const float PercentTolerance = 1e-1f;
    
    private static readonly object[] LengthCases =
    [
        new object[] { 0f, 0f, 0f },

        new object[] { 1f, 0f, 1f },
        new object[] { 0f, 1f, 1f },
        new object[] { -1f, 0f, 1f },
        new object[] { 0f, -1f, 1f },

        new object[] { 1f, 1f, float.Sqrt(2f) },
        new object[] { -1f, -1f, float.Sqrt(2f) },

        new object[] { 3f, 4f, 5f },
        new object[] { -3f, -4f, 5f },
        new object[] { -3f, 4f, 5f },
        new object[] { 3f, -4f, 5f },

        new object[] { 5f, 12f, 13f },
        new object[] { 8f, 15f, 17f },

        new object[] { 0.5f, 0.5f, float.Sqrt(0.5f) },
        new object[] { -0.5f, 0.5f, float.Sqrt(0.5f) },

        new object[] { float.Epsilon, float.Epsilon, float.Sqrt(2f * float.Epsilon * float.Epsilon) },

        new object[] { 1000f, 1000f, float.Sqrt(2_000_000f) },
        new object[] { 1e10f, 1e10f, float.Sqrt(2e20f) }
    ];

    private static readonly object[] LengthSquaredCases =
    [
        new object[] { 0f, 0f, 0f },

        new object[] { 1f, 0f, 1f },
        new object[] { 0f, 1f, 1f },

        new object[] { 1f, 1f, 2f },
        new object[] { -1f, -1f, 2f },

        new object[] { 3f, 4f, 25f },
        new object[] { -3f, -4f, 25f },

        new object[] { 5f, 12f, 169f },
        new object[] { 8f, 15f, 289f },

        new object[] { 0.5f, 0.5f, 0.5f },

        new object[] { 1000f, 1000f, 2_000_000f }
    ];

    [TestCaseSource(nameof(LengthCases))]
    public void Length_IsSignInvariant(
        float x,
        float y,
        float _)
    {
        var positive = new Vector2(x, y);
        var negative = new Vector2(-x, -y);
        Assert.That(positive.Length, Is.EqualTo(negative.Length).Within(Tolerance));
    }

    [Test]
    public void Length_Overflow_ReturnsInfinity()
    {
        var vector = new Vector2(float.MaxValue / 4f, 0f);

        Assert.That(vector.Length, Is.EqualTo(float.PositiveInfinity));
    }

    [TestCaseSource(nameof(LengthCases))]
    public void Length_ReturnsExpected(
        float x,
        float y,
        float expected)
    {
        var vector = new Vector2(x, y);
        Assert.That(vector.Length, Is.EqualTo(expected).Within(Tolerance));
    }

    [TestCaseSource(nameof(LengthCases))]
    public void LengthFast_ApproximatesLength(
        float x,
        float y,
        float expected)
    {
        var vector = new Vector2(x, y);
        Assert.That(vector.LengthFast, Is.EqualTo(expected).Within(PercentTolerance).Percent);
    }

    [TestCaseSource(nameof(LengthCases))]
    public void LengthNormalized_ReturnsExpected(
        float x,
        float y,
        float expected)
    {
        var vector = new Vector2(x, y);
        var (length, _) = vector.LengthNormalized;
        Assert.That(length, Is.EqualTo(expected).Within(Tolerance));
    }

    [TestCaseSource(nameof(LengthCases))]
    public void LengthSquared_Matches_Length_Multiplied(
        float x,
        float y,
        float _)
    {
        var vector = new Vector2(x, y);
        
        // However, in this case we have much more accurate data
        // compared to the Fast methods, so we can use the standard tolerance,
        // but still expressed as a percentage
        Assert.That(vector.LengthSquared, Is.EqualTo(vector.Length * vector.Length).Within(Tolerance).Percent);
    }

    [TestCaseSource(nameof(LengthSquaredCases))]
    public void LengthSquared_ReturnsExpected(
        float x,
        float y,
        float expected)
    {
        var vector = new Vector2(x, y);
        Assert.That(vector.LengthSquared, Is.EqualTo(expected).Within(Tolerance));
    }
}