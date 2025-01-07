﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Hypercube.Mathematics.Vectors;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public readonly partial struct Vector3i 
{
    /*
     * Self Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Vector3i value)
    {
        return new Vector3(value.X, value.Y, value.Z);
    }
    
    /*
     * Tuple Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3i((int x, int y, int z) value)
    {
        return new Vector3i(value.x, value.y, value.z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (int x, int y, int z)(Vector3i value)
    {
        return (value.X, value.Y, value.Z);
    }
    
    /*
     * System.Numerics Compatibility
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3i(System.Numerics.Vector3 value)
    {
        return new Vector3i((int) value.X, (int) value.Y, (int) value.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Vector3(Vector3i value)
    {
        return new System.Numerics.Vector3(value.X, value.Y, value.Z);
    }
}