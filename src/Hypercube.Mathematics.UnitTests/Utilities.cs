using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests;

public static class Utilities
{
    public static void AssertAreEqual(Matrix3x3 expected, Matrix3x3 actual, float delta = HyperMath.FloatTolerance)
    {
        using (Assert.EnterMultipleScope())
        {
            // Row 0
            Assert.That(actual.M00, Is.EqualTo(expected.M00).Within(delta));
            Assert.That(actual.M01, Is.EqualTo(expected.M01).Within(delta));
            Assert.That(actual.M02, Is.EqualTo(expected.M02).Within(delta));
            // Row 1
            Assert.That(actual.M10, Is.EqualTo(expected.M10).Within(delta));
            Assert.That(actual.M11, Is.EqualTo(expected.M11).Within(delta));
            Assert.That(actual.M12, Is.EqualTo(expected.M12).Within(delta));
            // Row 2
            Assert.That(actual.M20, Is.EqualTo(expected.M20).Within(delta));
            Assert.That(actual.M21, Is.EqualTo(expected.M21).Within(delta));
            Assert.That(actual.M22, Is.EqualTo(expected.M22).Within(delta));
        }
    }
    
    public static void AssertAreEqual(
        Matrix4x4 expected,
        Matrix4x4 actual,
        float delta = HyperMath.FloatTolerance,
        NUnitString message = default)
    {
        using (Assert.EnterMultipleScope())
        {
            // Row 0
            Assert.That(actual.M00, Is.EqualTo(expected.M00).Within(delta), message);
            Assert.That(actual.M01, Is.EqualTo(expected.M01).Within(delta), message);
            Assert.That(actual.M02, Is.EqualTo(expected.M02).Within(delta), message);
            Assert.That(actual.M03, Is.EqualTo(expected.M03).Within(delta), message);
            // Row 1
            Assert.That(actual.M10, Is.EqualTo(expected.M10).Within(delta), message);
            Assert.That(actual.M11, Is.EqualTo(expected.M11).Within(delta), message);
            Assert.That(actual.M12, Is.EqualTo(expected.M12).Within(delta), message);
            Assert.That(actual.M13, Is.EqualTo(expected.M13).Within(delta), message);
            // Row 2
            Assert.That(actual.M20, Is.EqualTo(expected.M20).Within(delta), message);
            Assert.That(actual.M21, Is.EqualTo(expected.M21).Within(delta), message);
            Assert.That(actual.M22, Is.EqualTo(expected.M22).Within(delta), message);
            Assert.That(actual.M23, Is.EqualTo(expected.M23).Within(delta), message);
            // Row 3
            Assert.That(actual.M30, Is.EqualTo(expected.M30).Within(delta), message);
            Assert.That(actual.M31, Is.EqualTo(expected.M31).Within(delta), message);
            Assert.That(actual.M32, Is.EqualTo(expected.M32).Within(delta), message);
            Assert.That(actual.M33, Is.EqualTo(expected.M33).Within(delta), message);
        }
    }

    public static void AssertAreEqual(
        Vector2 actual,
        Vector2 expected,
        float delta = HyperMath.FloatTolerance,
        NUnitString message = default)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(delta), message);
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(delta), message);
        }
    }

    public static void AssertAreEqual(
        Vector3 actual,
        Vector3 expected,
        float delta = HyperMath.FloatTolerance,
        NUnitString message = default)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(delta), message);
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(delta), message);
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(delta), message);
        }
    }

    public static void AssertAreEqual(
        Vector4 expected,
        Vector4 actual,
        float delta = HyperMath.FloatTolerance,
        NUnitString message = default)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(delta), message);
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(delta), message);
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(delta), message);
            Assert.That(actual.W, Is.EqualTo(expected.W).Within(delta), message);
        }
    }
    
    public static void AssertAreEqual(
        Vector5 expected,
        Vector5 actual,
        float delta = HyperMath.FloatTolerance,
        NUnitString message = default)
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(delta), message);
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(delta), message);
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(delta), message);
            Assert.That(actual.W, Is.EqualTo(expected.W).Within(delta), message);
            Assert.That(actual.V, Is.EqualTo(expected.V).Within(delta), message);
        }
    }
}