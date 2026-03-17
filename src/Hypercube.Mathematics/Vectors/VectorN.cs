using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

/// <summary>
/// Warning: EXPEREMENTAL API
/// </summary>
[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public readonly ref struct VectorN : IEquatable<VectorN>
{
    private readonly Span<float> _storage;

    public int Dimensionality
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _storage.Length;
    }

    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _storage[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _storage[index] = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VectorN(Span<float> storage)
    {
        _storage = storage;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VectorN(params float[] values) : this(new Span<float>(values))
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VectorN(VectorN other)
    {
        other._storage.CopyTo(_storage);
    }
    
    #region Cast

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float[] AsArray() => _storage.ToArray();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<float> AsSpan() => _storage;

    #endregion
    
    #region Equality
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Compatible(Vector2 other) => AreCompatible(this, other);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Compatible(Vector3 other) => AreCompatible(this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Compatible(Vector4 other) => AreCompatible(this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Compatible(Vector5 other) => AreCompatible(this, other);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Compatible(VectorN other) => AreCompatible(this, other);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => throw new NotSupportedException("VectorN is a ref struct and cannot be compared to object.");

    public bool Equals(VectorN other)
    {
        if (!Compatible(other))
            return false;
        
        for (var i = 0; i < _storage.Length; i++)
            if (!_storage[i].Equals(other[i]))
                return false;
        
        return true;
    }

    public bool AboutEquals(VectorN other, float tolerance = HyperMath.FloatTolerance)
    {
        if (!Compatible(other))
            return false;
        
        for (var i = 0; i < _storage.Length; i++)
            if (!_storage[i].AboutEquals(other[i], tolerance))
                return false;
        
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _storage.GetHashCode();

    #endregion

    #region Static Math

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AreCompatible(VectorN a, Vector2 b) => a.Dimensionality == Vector2.Dimensionality;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AreCompatible(VectorN a, Vector3 b) => a.Dimensionality == Vector3.Dimensionality;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AreCompatible(VectorN a, Vector4 b) => a.Dimensionality == Vector4.Dimensionality;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AreCompatible(VectorN a, Vector5 b) => a.Dimensionality == Vector5.Dimensionality;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AreCompatible(VectorN a, VectorN b) => a.Dimensionality == b.Dimensionality;

    public static VectorN Abs(VectorN vector)
    {
        for (var i = 0; i < vector.Dimensionality; i++)
            vector[i] = float.Abs(vector[i]);
        
        return vector;
    }

    public static VectorN Round(VectorN vector)
    {
        for (var i = 0; i < vector.Dimensionality; i++)
            vector[i] = float.Round(vector[i]);
        
        return vector;
    }

    public static VectorN Floor(VectorN vector)
    {
        for (var i = 0; i < vector.Dimensionality; i++)
            vector[i] = float.Floor(vector[i]);

        return vector;
    }

    public static VectorN Ceiling(VectorN vector)
    {
        for (var i = 0; i < vector.Dimensionality; i++)
            vector[i] = float.Ceiling(vector[i]);
        
        return vector;
    }

    public float Dot(VectorN other)
    {
        AreCompatible(this, other);
        
        var result = 0f;
        for (var i = 0; i < _storage.Length; i++)
            result += _storage[i] * other[i];
        
        return result;
    }

    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(VectorN a, VectorN b) => a.AboutEquals(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(VectorN a, VectorN b) => !(a == b);

    public static VectorN operator +(VectorN a, VectorN b)
    {
        AreCompatible(a, b);

        var dimensionality = a.Dimensionality;
        var result = new float[dimensionality];
        
        for (var i = 0; i < dimensionality; i++)
            result[i] = a[i] + b[i];
        
        return new VectorN(result);
    }

    public static VectorN operator -(VectorN a, VectorN b)
    {
        AreCompatible(a, b);
        
        var dimensionality = a.Dimensionality;
        var result = new float[dimensionality];
        
        for (var i = 0; i < dimensionality; i++)
            result[i] = a[i] - b[i];
        
        return new VectorN(result);
    }

    public static VectorN operator *(VectorN a, VectorN b)
    {
        AreCompatible(a, b);
        
        var dimensionality = a.Dimensionality;
        var result = new float[dimensionality];

        for (var i = 0; i < dimensionality; i++)
            result[i] = a[i] * b[i];
        
        return new VectorN(result);
    }

    public static VectorN operator *(VectorN a, float b)
    {
        var dimensionality = a.Dimensionality;
        var result = new float[dimensionality];

        for (var i = 0; i < dimensionality; i++)
            result[i] = a[i] * b;
        
        return new VectorN(result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorN operator *(float a, VectorN b) => b * a;

    public static VectorN operator /(VectorN a, VectorN b)
    {
        AreCompatible(a, b);
        
        var dimensionality = a.Dimensionality;
        var result = new float[dimensionality];

        for (var i = 0; i < dimensionality; i++)
            result[i] = a[i] / b[i];
        
        return new VectorN(result);
    }

    public static VectorN operator /(VectorN a, float b)
    {
        var dimensionality = a.Dimensionality;
        var result = new float[dimensionality];

        for (var i = 0; i < dimensionality; i++)
            result[i] = a[i] / b;
        
        return new VectorN(result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorN operator /(float a, VectorN b) => b / a;

    #endregion
}