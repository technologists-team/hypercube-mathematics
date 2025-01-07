﻿using System.Runtime.CompilerServices;

namespace Hypercube.Mathematics.Vectors;

public readonly partial struct Vector3
{
    /*
     * Self Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(Vector3 vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2i(Vector3 vector)
    {
        return new Vector2i((int)vector.X, (int)vector.Y);
    }
    
    /*
     * Tuple Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3((float x, float y, float z) a)
    {
        return new Vector3(a.x, a.y, a.z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (float x, float y, float z)(Vector3 a)
    {
        return (a.X, a.Y, a.Z);
    }
    
    /*
     * System.Numerics Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Vector3(Vector3 vector3)
    {
        return new System.Numerics.Vector3(vector3.X, vector3.Y, vector3.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(System.Numerics.Vector3 vector3)
    {
        return new Vector3(vector3.X, vector3.Y, vector3.Z);
    }
}