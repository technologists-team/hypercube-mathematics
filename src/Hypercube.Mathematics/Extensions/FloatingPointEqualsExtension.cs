using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Extensions;

[PublicAPI]
public static class FloatingPointEqualsExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AboutEquals(this float @this, float value, float tolerance = HyperMath.FloatTolerance)
    {
        return HyperMath.AboutEquals(@this, value, tolerance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AboutEquals(this double @this, double value, double tolerance = HyperMath.DoubleTolerance)
    {
        return HyperMath.AboutEquals(@this, value, tolerance);
    }
}