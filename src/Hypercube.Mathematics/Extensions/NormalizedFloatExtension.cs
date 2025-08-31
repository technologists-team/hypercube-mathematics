using System.Runtime.CompilerServices;

namespace Hypercube.Mathematics.Extensions;

public static class NormalizedFloatExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ByteToNormalizedFloat(this byte value) => (float) value / byte.MaxValue;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ByteToNormalizedFloat(this int value) => (float) value / byte.MaxValue;
}