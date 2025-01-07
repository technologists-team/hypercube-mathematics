using JetBrains.Annotations;

namespace Hypercube.Mathematics.Extensions;

[PublicAPI]
public static class FloatingPointEqualsExtension
{
    public static bool AboutEquals(this float a, float b, float tolerance = 1E-15f)
    {
        return HyperMath.AboutEquals(a, b, tolerance);
    }

    public static bool AboutEquals(this double a, double b, double tolerance = 1E-15d)
    {
        return HyperMath.AboutEquals(a, b, tolerance);
    }
}