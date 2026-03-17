namespace Hypercube.Mathematics.Extensions;

public static class ArrayExtensions
{
    public static T[] With<T>(this T[] context, int index, T value)
    {
        Tools.ThrowIfOutOfRange(index, 0, context.Length);
        
        var storage = new T[context.Length];
        context.CopyTo(storage, 0);

        storage[index] = value;
        
        return storage;
    }
}