using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Vectors.Vector2Tests;

[TestFixture]
public sealed class Linq
{
       [Test]
    public void LINQ_ToArray_Returns_X_Y()
    {
        var v = new Vector2(1f, 2f);

        var result = v.ToArray();
        Assert.That(result, Is.EqualTo([1f, 2f]));
    }

    [Test]
    public void LINQ_ToList_Returns_X_Y()
    {
        var v = new Vector2(3f, 4f);

        var result = v.ToList();
        Assert.That(result, Is.EqualTo(new List<float> { 3f, 4f }));
    }

    [Test]
    public void LINQ_First_Returns_X()
    {
        var v = new Vector2(10f, 20f);

        var first = v.First();
        Assert.That(first, Is.EqualTo(10f));
    }

    [Test]
    public void LINQ_Last_Returns_Y()
    {
        var v = new Vector2(10f, 20f);

        var last = v.Last();
        Assert.That(last, Is.EqualTo(20f));
    }

    [Test]
    public void LINQ_Count_Returns_2()
    {
        var v = new Vector2(10f, 20f);

        var count = v.Count();
        Assert.That(count, Is.EqualTo(2));
    }

    [Test]
    public void LINQ_Where_Filters_Correctly()
    {
        var v = new Vector2(1f, 2f);

        var result = v.Where(x => x > 1f).ToArray();
        Assert.That(result, Is.EqualTo([2f]));
    }

    [Test]
    public void LINQ_Select_Transforms_Correctly()
    {
        var v = new Vector2(2f, 3f);

        var result = v.Select(x => x * 2).ToArray();
        Assert.That(result, Is.EqualTo([4f, 6f]));
    }

    [Test]
    public void LINQ_Aggregate_Sum_Works()
    {
        var v = new Vector2(5f, 7f);

        var sum = v.Sum();
        Assert.That(sum, Is.EqualTo(12f));
    }

    [Test]
    public void LINQ_Average_Works()
    {
        var v = new Vector2(2f, 4f);

        var avg = v.Average();
        Assert.That(avg, Is.EqualTo(3f));
    }

    [Test]
    public void LINQ_Empty_After_Filtering()
    {
        var v = new Vector2(1f, 2f);

        var result = v.Where(x => x > 100f).ToArray();
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void LINQ_Multiple_Enumeration_Is_Safe()
    {
        var v = new Vector2(10f, 20f);

        var a = v.ToArray();
        var b = v.ToArray();

        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    public void LINQ_Reverse_Works()
    {
        var v = new Vector2(1f, 2f);

        var result = v.Reverse().ToArray();
        Assert.That(result, Is.EqualTo([2f, 1f]));
    }

    [Test]
    public void LINQ_All_Works()
    {
        var v = new Vector2(2f, 4f);

        var allPositive = v.All(x => x > 0f);

        Assert.That(allPositive, Is.True);
    }

    [Test]
    public void LINQ_Any_Works()
    {
        var v = new Vector2(-1f, 2f);

        var anyPositive = v.Any(x => x > 0f);
        Assert.That(anyPositive, Is.True);
    }
}