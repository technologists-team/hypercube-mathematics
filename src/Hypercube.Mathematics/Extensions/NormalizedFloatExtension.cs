using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Extensions;

[PublicAPI]
public static class NormalizedFloatExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ByteToNormalizedFloat(this byte value) => (float) value / byte.MaxValue;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ByteToNormalizedFloat(this int value) => (float) value / byte.MaxValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte NormalizedFloatToByte(this float value) => value is >= 0 and <= 1
        ? (byte)(value * byte.MaxValue)
        : throw new ArgumentOutOfRangeException(nameof(value));
}