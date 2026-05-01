using System.Collections;
using Hypercube.Mathematics.Vectors;

// ReSharper disable LoopCanBeConvertedToQuery

namespace Hypercube.Mathematics.UnitTests.Vectors.Vector2Tests;

[TestFixture]
public sealed class Enumeration
{
      [Test]
    public void GetEnumerator_Generic_Iterates_X_Y()
    {
        var v = new Vector2(10f, 20f);

        using var e = v.GetEnumerator();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(e.MoveNext(), Is.True);
            Assert.That(e.Current, Is.EqualTo(10f));
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(e.MoveNext(), Is.True);
            Assert.That(e.Current, Is.EqualTo(20f));
        }

        Assert.That(e.MoveNext(), Is.False);
    }

    [Test]
    public void GetEnumerator_NonGeneric_Iterates_X_Y()
    {
        var v = new Vector2(10f, 20f);

        var e = ((IEnumerable) v).GetEnumerator();
        
        try
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(e.MoveNext(), Is.True);
                Assert.That(e.Current, Is.EqualTo(10f));
            }

            using (Assert.EnterMultipleScope())
            {
                Assert.That(e.MoveNext(), Is.True);
                Assert.That(e.Current, Is.EqualTo(20f));
            }

            Assert.That(e.MoveNext(), Is.False);
        }
        finally
        {
            ((IDisposable) e).Dispose();
        }
    }

    [Test]
    public void Generic_And_NonGeneric_Return_Same_Values()
    {
        var v = new Vector2(3f, 4f);

        var g = v.GetEnumerator();
        var n = ((IEnumerable) v).GetEnumerator();

        try
        {
            while (g.MoveNext() && n.MoveNext())
            {
                Assert.That(g.Current, Is.EqualTo((float)n.Current));
            }

            using (Assert.EnterMultipleScope())
            {
                Assert.That(g.MoveNext(), Is.False);
                Assert.That(n.MoveNext(), Is.False);
            }
        }
        finally
        {
            g.Dispose();
            ((IDisposable) n).Dispose();
        }
    }

    [Test]
    public void Foreach_Returns_X_Y_In_Order()
    {
        var v = new Vector2(5f, 6f);

        var result = new List<float>();
        foreach (var value in v)
        {
            result.Add(value);
        }

        Assert.That(result, Is.EqualTo([5f, 6f]));
    }

    [Test]
    public void Foreach_Works_For_Zero_Vector()
    {
        var v = Vector2.Zero;

        var result = new List<float>();

        foreach (var value in v)
        {
            result.Add(value);
        }

        Assert.That(result, Is.EqualTo([0f, 0f]));
    }

    [Test]
    public void Multiple_Enumerations_Are_Independent()
    {
        var v = new Vector2(1f, 2f);

        using var e1 = v.GetEnumerator();
        using var e2 = v.GetEnumerator();

        e1.MoveNext();
        e2.MoveNext();

        Assert.That(e1.Current, Is.EqualTo(e2.Current));

        e1.MoveNext();
        e2.MoveNext();

        Assert.That(e1.Current, Is.EqualTo(e2.Current));
    }

    [Test]
    public void Enumerator_Does_Not_Leak_State()
    {
        var v = new Vector2(7f, 8f);

        using var e = v.GetEnumerator();

        e.MoveNext();
        Assert.That(e.Current, Is.EqualTo(7f));

        // Restart enumerator
        using var e2 = v.GetEnumerator();

        e2.MoveNext();
        Assert.That(e2.Current, Is.EqualTo(7f));
    }

    [Test]
    public void Handles_Negative_Values()
    {
        var v = new Vector2(-1f, -2f);

        var result = new List<float>();

        foreach (var value in v)
        {
            result.Add(value);
        }

        Assert.That(result, Is.EqualTo([-1f, -2f]));
    }

    [Test]
    public void Handles_Mixed_Sign_Values()
    {
        var v = new Vector2(-3f, 4f);

        var result = new List<float>();
        foreach (var value in v)
        {
            result.Add(value);
        }

        Assert.That(result, Is.EqualTo([-3f, 4f]));
    }

    [Test]
    public void Handles_Float_Special_Values()
    {
        var v = new Vector2(float.NaN, float.PositiveInfinity);

        var result = new List<float>();
        foreach (var value in v)
        {
            result.Add(value);
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(float.IsNaN(result[0]), Is.True);
            Assert.That(float.IsPositiveInfinity(result[1]), Is.True);
        }
    }
}