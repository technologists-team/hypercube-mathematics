using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Vectors.Vector2Tests;

[TestFixture]
public sealed class Clamping
{
    private static readonly object[] BinaryMinCases =
    [
        new object[] { new Vector2(1, 2), new Vector2(3, 1), new Vector2(1, 1) },
        new object[] { new Vector2(-1, 5), new Vector2(2, -3), new Vector2(-1, -3) },
        new object[] { Vector2.Zero, Vector2.Zero, Vector2.Zero }
    ];

    private static readonly object[] BinaryMaxCases =
    [
        new object[] { new Vector2(1, 2), new Vector2(3, 1), new Vector2(3, 2) },
        new object[] { new Vector2(-1, 5), new Vector2(2, -3), new Vector2(2, 5) },
        new object[] { Vector2.Zero, Vector2.Zero, Vector2.Zero }
    ];

    private static readonly object[] AggregateMinCases =
    [
        new object[]
        {
            new[]
            {
                new Vector2(3, 5),
                new Vector2(1, 7),
                new Vector2(2, -1)
            },
            new Vector2(1, -1)
        }
    ];

    private static readonly object[] AggregateMaxCases =
    [
        new object[]
        {
            new[]
            {
                new Vector2(3, 5),
                new Vector2(1, 7),
                new Vector2(2, -1)
            },
            new Vector2(3, 7)
        }
    ];

    private static readonly object[] ClampCases =
    [
        new object[] { new Vector2(5, 5), new Vector2(0, 0), new Vector2(10, 10), new Vector2(5, 5) },
        new object[] { new Vector2(-5, -1), new Vector2(0, 0), new Vector2(10, 10), new Vector2(0, 0) },
        new object[] { new Vector2(15, 20), new Vector2(0, 0), new Vector2(10, 10), new Vector2(10, 10) },
        new object[] { new Vector2(-5, 20), new Vector2(0, 0), new Vector2(10, 10), new Vector2(0, 10) }
    ];
    
    private static readonly Vector2[] EmptyArray = [];

    private static readonly Vector2[] SingleValue =
    [
        new Vector2(3, 4)
    ];

    private static readonly Vector2[] WithNaN =
    [
        new Vector2(1, 2),
        new Vector2(float.NaN, 0),
        new Vector2(3, 4)
    ];

    private static readonly Vector2[] WithInfinity =
    [
        new Vector2(1, 2),
        new Vector2(float.PositiveInfinity, 5),
        new Vector2(3, 4)
    ];

    private static readonly Vector2[] WithNegativeInfinity =
    [
        new Vector2(1, 2),
        new Vector2(float.NegativeInfinity, 5),
        new Vector2(3, 4)
    ];

    [TestCaseSource(nameof(BinaryMinCases))]
    public void Min_Binary_ReturnsExpected(
        Vector2 a,
        Vector2 b,
        Vector2 expected)
    {
        Assert.That(Vector2.Min(a, b), Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(BinaryMaxCases))]
    public void Max_Binary_ReturnsExpected(
        Vector2 a,
        Vector2 b,
        Vector2 expected)
    {
        Assert.That(Vector2.Max(a, b), Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(AggregateMinCases))]
    public void Min_Params_ReturnsExpected(
        Vector2[] values,
        Vector2 expected)
    {
        Assert.That(Vector2.Min(values), Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(AggregateMaxCases))]
    public void Max_Params_ReturnsExpected(
        Vector2[] values,
        Vector2 expected)
    {
        Assert.That(Vector2.Max(values), Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(AggregateMinCases))]
    public void Min_Span_ReturnsExpected(
        Vector2[] values,
        Vector2 expected)
    {
        ReadOnlySpan<Vector2> span = values;

        Assert.That(Vector2.Min(span), Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(AggregateMaxCases))]
    public void Max_Span_ReturnsExpected(
        Vector2[] values,
        Vector2 expected)
    {
        ReadOnlySpan<Vector2> span = values;

        Assert.That(Vector2.Max(span), Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(ClampCases))]
    public void Clamp_ReturnsExpected(
        Vector2 value,
        Vector2 min,
        Vector2 max,
        Vector2 expected)
    {
        Assert.That(Vector2.Clamp(value, min, max), Is.EqualTo(expected));
    }
    
    [Test]
    public void Min_Params_Empty_Throws()
    {
        Assert.Throws<ArgumentException>(() => Vector2.Min([]));
    }

    [Test]
    public void Max_Params_Empty_Throws()
    {
        Assert.Throws<ArgumentException>(() => Vector2.Max([]));
    }

    [Test]
    public void Min_Span_Empty_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            ReadOnlySpan<Vector2> span = [];
            Vector2.Min(span);
        });
    }

    [Test]
    public void Max_Span_Empty_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            ReadOnlySpan<Vector2> span = [];
            Vector2.Max(span);
        });
    }

    [Test]
    public void Min_SingleValue_ReturnsSame()
    {
        Assert.That(Vector2.Min(SingleValue), Is.EqualTo(SingleValue[0]));
    }

    [Test]
    public void Max_SingleValue_ReturnsSame()
    {
        Assert.That(Vector2.Max(SingleValue), Is.EqualTo(SingleValue[0]));
    }

    [Test]
    public void Min_WithNaN_PropagatesNaN()
    {
        var result = Vector2.Min(WithNaN);
        Assert.That(float.IsNaN(result.X) || float.IsNaN(result.Y), Is.True);
    }

    [Test]
    public void Max_WithInfinity_ReturnsInfinity()
    {
        var result = Vector2.Max(WithInfinity);
        Assert.That(float.IsInfinity(result.X), Is.True);
    }

    [Test]
    public void Min_WithNegativeInfinity_ReturnsNegativeInfinity()
    {
        var result = Vector2.Min(WithNegativeInfinity);
        Assert.That(float.IsNegativeInfinity(result.X), Is.True);
    }

    [Test]
    public void Clamp_MinGreaterThanMax_Throws()
    {
        var value = new Vector2(5, 5);
        var min = new Vector2(10, 10);
        var max = new Vector2(0, 0);

        Assert.Throws<ArgumentOutOfRangeException>(() => Vector2.Clamp(value, min, max));
    }

    [Test]
    public void Clamp_WithNaN_ReturnsNaN()
    {
        var result = Vector2.Clamp(new Vector2(float.NaN, 1), Vector2.Zero, new Vector2(10, 10));
        Assert.That(float.IsNaN(result.X), Is.True);
    }
}
