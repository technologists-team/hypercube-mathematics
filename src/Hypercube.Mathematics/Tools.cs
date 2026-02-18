using System.Runtime.CompilerServices;

namespace Hypercube.Mathematics;

// TODO: Move in other repo
public static class Tools
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfOutOfRange(int value, int min, int max, [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(paramName, value, null);
    }
}