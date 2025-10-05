using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Hypercube.Mathematics.Vectors;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public readonly partial struct Vector2
{
    /*
     * Self Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2d(Vector2 vector)
    {
        return new Vector2d(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2i(Vector2 vector)
    {
        return new Vector2i((int) vector.X, (int) vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Vector2 vector)
    {
        return new Vector3(vector.X, vector.Y, 0f);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3i(Vector2 vector)
    {
        return new Vector3i((int) vector.X, (int) vector.Y, 0);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4(Vector2 vector)
    {
        return new Vector4(vector.X, vector.Y, 0f, 0f);
    }
    
    /*
     * Tuple Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2((float x, float y) a)
    {
        return new Vector2(a.x, a.y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (float x, float y)(Vector2 a)
    {
        return (a.X, a.Y);
    }
    
    /*
     * System.Numerics Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(System.Numerics.Vector2 vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Vector2(Vector2 vector)
    {
        return new System.Numerics.Vector2(vector.X, vector.Y);
    }
}