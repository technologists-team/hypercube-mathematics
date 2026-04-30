using System.Runtime.CompilerServices;

namespace Hypercube.Mathematics.Matrices.Utilities;

public static class MatrixMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T SquareGet<T>(int index, in T begin, int length)
        where T : unmanaged
    {
        Tools.ThrowIfOutOfRange(index, 0, length - 1);
        
        ref var start = ref Unsafe.AsRef(in begin);
        return Unsafe.Add(ref start, index);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T SquareGet<T>(int row, int col, in T begin, int dimensionality, bool transposed = false)
        where T : unmanaged
    {
        Tools.ThrowIfOutOfRange(row, 0, dimensionality - 1);
        Tools.ThrowIfOutOfRange(col, 0, dimensionality - 1);

        ref var start = ref Unsafe.AsRef(in begin);
        var offset = transposed
            ? row * dimensionality + col
            : col * dimensionality + row;
        
        return Unsafe.Add(ref start, offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span<T> ToSpan<T>(in T begin, int length)
        where T : unmanaged
    {
        fixed (T* ptr = &Unsafe.AsRef(in begin))
            return new Span<T>(ptr, length);
    }
}