using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Vectors.Vector2Tests;

[TestFixture]
public sealed class Indexer
{
    private static readonly object[] IndexerCases =
    [
        new object[] { new Vector2(0f, 0f), 0, 0f },
        new object[] { new Vector2(1f, 2f), 0, 1f },
        new object[] { new Vector2(1f, 2f), 1, 2f },

        new object[] { new Vector2(-1f, -2f), 0, -1f },
        new object[] { new Vector2(-1f, -2f), 1, -2f },

        new object[] { new Vector2(3.5f, 4.5f), 0, 3.5f },
        new object[] { new Vector2(3.5f, 4.5f), 1, 4.5f },

        new object[] { new Vector2(float.Epsilon, 1f), 0, float.Epsilon },
        new object[] { new Vector2(1f, float.Epsilon), 1, float.Epsilon },

        new object[] { new Vector2(float.MaxValue, float.MinValue), 0, float.MaxValue },
        new object[] { new Vector2(float.MaxValue, float.MinValue), 1, float.MinValue }
    ];
    
    private static readonly object[] InvalidIndices =
    [
        new object[] { new Vector2(1f, 2f), -1 },
        new object[] { new Vector2(1f, 2f), 2 },
        new object[] { new Vector2(1f, 2f), 100 },
        new object[] { new Vector2(1f, 2f), int.MinValue },
        new object[] { new Vector2(1f, 2f), int.MaxValue }
    ];

    [TestCaseSource(nameof(IndexerCases))]
    public void Indexer_Returns_Correct_Value(
        Vector2 vector,
        int index,
        float expected)
    {
        Assert.That(vector[index], Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(IndexerCases))]
    public void Indexer_Is_Stable_On_Repeated_Access(
        Vector2 vector,
        int index,
        float expected)
    {
        Assert.That(vector[index], Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(IndexerCases))]
    public void Indexer_Matches_Component_Semantics(
        Vector2 vector,
        int index,
        float expected)
    {
        switch (index)
        {
            case 0:
                Assert.That(vector[index], Is.EqualTo(vector.X));
                break;
            case 1:
                Assert.That(vector[index], Is.EqualTo(vector.Y));
                break;
        }
    }
    
    [TestCaseSource(nameof(InvalidIndices))]
    public void Indexer_Throws_On_Invalid_Index(
        Vector2 vector,
        int index)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = vector[index];
        });
    }

    [Test]
    public void Indexer_Allows_Sequential_Access()
    {
        var vector = new Vector2(10f, 20f);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(vector[0], Is.EqualTo(10f));
            Assert.That(vector[1], Is.EqualTo(20f));
        }
    }
    
    [Test]
    public void Indexer_Works_For_Zero_Vector()
    {
        var vector = Vector2.Zero;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(vector[0], Is.Zero);
            Assert.That(vector[1], Is.Zero);
        }
    }
}