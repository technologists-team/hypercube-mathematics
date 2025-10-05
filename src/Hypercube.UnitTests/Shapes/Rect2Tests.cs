using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.UnitTests.Shapes;

[TestFixture]
public sealed class Rect2Tests
{
    [Test]
    public void Constructor()
    {
        var rect = new Rect2(1, 2, 3, 4);
        Assert.Multiple(() =>
        {
            Assert.That(rect.Left, Is.EqualTo(1));
            Assert.That(rect.Top, Is.EqualTo(2));
            Assert.That(rect.Right, Is.EqualTo(3));
            Assert.That(rect.Bottom, Is.EqualTo(4));
        });
    }

    [Test]
    public void Properties()
    {
        var rect = new Rect2(0, 0, 10, 5);

        Assert.Multiple(() =>
        {
            Assert.That(rect.TopLeft, Is.EqualTo(new Vector2(0, 0)));
            Assert.That(rect.TopRight, Is.EqualTo(new Vector2(10, 0)));
            Assert.That(rect.BottomLeft, Is.EqualTo(new Vector2(0, 5)));
            Assert.That(rect.BottomRight, Is.EqualTo(new Vector2(10, 5)));

            Assert.That(rect.Width, Is.EqualTo(10));
            Assert.That(rect.Height, Is.EqualTo(5));
            Assert.That(rect.Size, Is.EqualTo(new Vector2(10, 5)));
            Assert.That(rect.Center, Is.EqualTo(new Vector2(5, 2.5f)));
            Assert.That(rect.Diagonal, Is.EqualTo(new Vector2(10, 5)));
            Assert.That(rect.Perimeter, Is.EqualTo(30));
            Assert.That(rect.Area, Is.EqualTo(50));
            Assert.That(rect.AspectRatio, Is.EqualTo(2));
        });
    }

    [Test]
    public void Inflate_ExpandsCorrectly()
    {
        var rect = new Rect2(0, 0, 10, 10);
        var inflated = rect.Inflate(new Vector2(2, 3));

        Assert.That(inflated.Left, Is.EqualTo(-2));
        Assert.That(inflated.Top, Is.EqualTo(-3));
        Assert.That(inflated.Right, Is.EqualTo(12));
        Assert.That(inflated.Bottom, Is.EqualTo(13));
    }

    [Test]
    public void Translate_ShiftsCorrectly()
    {
        var rect = new Rect2(1, 1, 3, 3);
        var moved = rect.Translate(new Vector2(2, -1));

        Assert.That(moved.Left, Is.EqualTo(3));
        Assert.That(moved.Top, Is.EqualTo(0));
        Assert.That(moved.Right, Is.EqualTo(5));
        Assert.That(moved.Bottom, Is.EqualTo(2));
    }

    [Test]
    public void Contains_Point_Works()
    {
        var rect = new Rect2(0, 0, 10, 10);

        Assert.That(rect.Contains(new Vector2(5, 5)), Is.True);
        Assert.That(rect.Contains(new Vector2(-1, 5)), Is.False);
        Assert.That(rect.Contains(new Vector2(5, 11)), Is.False);
    }

    [Test]
    public void Contains_Rect_Works()
    {
        var outer = new Rect2(0, 0, 10, 10);
        var inner = new Rect2(2, 2, 8, 8);

        Assert.That(outer.Contains(inner), Is.True);
        Assert.That(inner.Contains(outer), Is.False);
    }

    [Test]
    public void Intersects_And_Overlaps_Work()
    {
        var a = new Rect2(0, 0, 5, 5);
        var b = new Rect2(4, 4, 10, 10);
        var c = new Rect2(6, 6, 8, 8);

        Assert.That(a.Intersects(b), Is.True);
        Assert.That(a.Overlaps(b), Is.True);
        Assert.That(a.Intersects(c), Is.False);
        Assert.That(a.Overlaps(c), Is.False);
    }

    [Test]
    public void Operators_Addition_Subtraction_Work()
    {
        var a = new Rect2(1, 2, 3, 4);
        var b = new Rect2(2, 3, 4, 5);

        var sum = a + b;
        var diff = a - b;
        var neg = -a;

        Assert.That(sum, Is.EqualTo(new Rect2(3, 5, 7, 9)));
        Assert.That(diff, Is.EqualTo(new Rect2(-1, -1, -1, -1)));
        Assert.That(neg, Is.EqualTo(new Rect2(-1, -2, -3, -4)));
    }

    [Test]
    public void Equality_Works()
    {
        var a = new Rect2(1, 1, 2, 2);
        var b = new Rect2(1, 1, 2, 2);
        var c = new Rect2(0, 0, 1, 1);

        Assert.That(a == b, Is.True);
        Assert.That(a != c, Is.True);
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.Equals((object)b), Is.True);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }

    [Test]
    public void FactoryMethods_CreateExpectedRectangles()
    {
        var rect1 = Rect2.FromSize(new Vector2(0, 0), new Vector2(10, 5));
        Assert.That(rect1, Is.EqualTo(new Rect2(0, 5, 10, 0)));

        var rect2 = Rect2.FromCenter(new Vector2(5, 5), new Vector2(4, 4));
        Assert.That(rect2.Left, Is.EqualTo(3));
        Assert.That(rect2.Right, Is.EqualTo(7));
        Assert.That(rect2.Top, Is.EqualTo(3));
        Assert.That(rect2.Bottom, Is.EqualTo(7));

        var rect3 = Rect2.FromUnnormalized(10, 5, 0, 0);
        Assert.That(rect3.Left, Is.EqualTo(0));
        Assert.That(rect3.Right, Is.EqualTo(10));
        Assert.That(rect3.Top, Is.EqualTo(5));
        Assert.That(rect3.Bottom, Is.EqualTo(0));
    }

    [Test]
    public void ToString_And_TryFormat_Work()
    {
        var rect = new Rect2(1, 2, 3, 4);

        Assert.That(rect.ToString(), Is.EqualTo("1, 2, 3, 4"));

        Span<char> buffer = stackalloc char[32];
        var result = rect.TryFormat(buffer, out int written, default, null);
        Assert.That(result, Is.True);
        Assert.That(new string(buffer[..written]), Is.EqualTo("1, 2, 3, 4"));
    }

    [Test]
    public void Enumerator_YieldsVerticesInCorrectOrder()
    {
        var rect = new Rect2(0, 0, 2, 2);
        var points = rect.ToArray();

        Assert.That(points.Length, Is.EqualTo(4));
        Assert.That(points[0], Is.EqualTo(rect.TopLeft));
        Assert.That(points[1], Is.EqualTo(rect.TopRight));
        Assert.That(points[2], Is.EqualTo(rect.BottomRight));
        Assert.That(points[3], Is.EqualTo(rect.BottomLeft));
    }

    [Test]
    public void NearestPoint_ClampsCorrectly()
    {
        var rect = new Rect2(0, 0, 10, 10);

        var inside = rect.NearestPoint(new Vector2(5, 5));
        var left = rect.NearestPoint(new Vector2(-5, 5));
        var top = rect.NearestPoint(new Vector2(5, -5));
        var bottomRight = rect.NearestPoint(new Vector2(15, 15));

        Assert.That(inside, Is.EqualTo(new Vector2(5, 5)));
        Assert.That(left, Is.EqualTo(new Vector2(0, 5)));
        Assert.That(top, Is.EqualTo(new Vector2(5, 0)));
        Assert.That(bottomRight, Is.EqualTo(new Vector2(10, 10)));
    }
}
