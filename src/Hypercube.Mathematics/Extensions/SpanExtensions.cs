namespace Hypercube.Mathematics.Extensions;

public static class SpanExtensions
{
    public static T[] With<T>(this Span<T> context, int index, T value)
    {
        Tools.ThrowIfOutOfRange(index, 0, context.Length);
        
        var storage = new T[context.Length];
        context.CopyTo(storage);

        storage[index] = value;
        
        return storage;
    }
}