using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Vectors;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[DebuggerDisplay("{ToString()}")]
public readonly partial struct Vector5 : IEquatable<Vector5>, IComparable<Vector5>, IComparable<float>, IEnumerable<float>
{
    public static readonly Vector5 NaN = new(float.NaN);
    public static readonly Vector5 PositiveInfinity = new(float.PositiveInfinity);
    public static readonly Vector5 NegativeInfinity = new(float.NegativeInfinity);

    public static readonly Vector5 Zero = new(0);
    public static readonly Vector5 One  = new(1);

    public static readonly Vector5 UnitX = new(1, 0, 0, 0, 0);
    public static readonly Vector5 UnitY = new(0, 1, 0, 0, 0);
    public static readonly Vector5 UnitZ = new(0, 0, 1, 0, 0);
    public static readonly Vector5 UnitW = new(0, 0, 0, 1, 0);
    public static readonly Vector5 UnitV = new(0, 0, 0, 0, 1);
    
    public readonly float X;
    public readonly float Y;
    public readonly float Z;
    public readonly float W;
    public readonly float V;
    
    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * X + Y * Y + Z * Z + W * W + V * V;
    }

    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 1f / MathF.ReciprocalSqrtEstimate(LengthSquared);
    }

    public Vector5 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this / Length;
    }

    public float Summation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X + Y + Z + W + V;
    }

    public float Production
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * Y * Z * W * V;
    }

    public Vector2 Xy
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X, Y);
    }

    public Vector3 Xyz
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X, Y, Z);
    }

    public Vector4 Xyzw
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(X, Y, Z, W);
    }
    
    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => index switch
        {
            0 => X,
            1 => Y,
            2 => Z,
            3 => W,
            4 => V,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };
    }

    public Vector5(float x, float y, float z, float w, float v)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
        V = v;
    }

    public Vector5(float value)
    {
        X = value;
        Y = value;
        Z = value;
        W = value;
        V = value;
    }

    public Vector5(double x, double y, double z, double w, double v)
    {
        X = (float) x;
        Y = (float) y;
        Z = (float) z;
        W = (float) w;
        V = (float) v;
    }

    public Vector5(double value)
    {
        X = (float) value;
        Y = (float) value;
        Z = (float) value;
        W = (float) value;
        V = (float) value;
    }

    public Vector5(Vector4 value, float v)
    {
        X = value.X;
        Y = value.Y;
        Z = value.Z;
        W = value.W;
        V = v;
    }

    public Vector5(Vector5 value)
    {
        X = value.X;
        Y = value.Y;
        Z = value.Z;
        W = value.W;
        V = value.V;
    }
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 FromPoint(Vector4 v)
    {
        return new Vector5(v.X, v.Y, v.Z, v.W, 1f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 FromDirection(Vector4 v)
    {
        return new Vector5(v.X, v.Y, v.Z, v.W, 0f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 ToPoint4D()
    {
        if (V.AboutEquals(0f) || V.AboutEquals(1f))
            return new Vector4(X, Y, Z, W);
        
        var inv = 1f / V;
        return new Vector4(X * inv, Y * inv, Z * inv, W * inv);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5 WithX(float value)
    {
        return new Vector5(value, Y, Z, W, V);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5 WithY(float value)
    {
        return new Vector5(X, value, Z, W, V);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5 WithZ(float value)
    {
        return new Vector5(X, Y, value, W, V);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5 WithW(float value)
    {
        return new Vector5(X, Y, Z, value, V);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector5 WithV(float value)
    {
        return new Vector5(X, Y, Z, W, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(Vector5 other)
    {
        return LengthSquared.CompareTo(other.LengthSquared);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(float other)
    {
        return LengthSquared.CompareTo(other * other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<float> GetEnumerator()
    {
        yield return X;
        yield return Y;
        yield return Z;
        yield return W;
        yield return V;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector5 other)
    {
        return X.AboutEquals(other.X) &&
               Y.AboutEquals(other.Y) &&
               Z.AboutEquals(other.Z) &&
               W.AboutEquals(other.W) &&
               V.AboutEquals(other.V);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector5 other, float tolerance)
    {
        return X.AboutEquals(other.X, tolerance) &&
               Y.AboutEquals(other.Y, tolerance) &&
               Z.AboutEquals(other.Z, tolerance) &&
               W.AboutEquals(other.W, tolerance) &&
               V.AboutEquals(other.V, tolerance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Vector5 other && Equals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"{X}, {Y}, {Z}, {W}, {V}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W, V);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator +(Vector5 a, Vector5 b)
    {
        return new Vector5(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W, a.V + b.V);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator -(Vector5 a)
    {
        return new Vector5(-a.X, -a.Y, -a.Z, -a.W, -a.V);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator -(Vector5 a, Vector5 b)
    {
        return new Vector5(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W, a.V - b.V);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator *(Vector5 a, float b)
    {
        return new Vector5(a.X * b, a.Y * b, a.Z * b, a.W * b, a.V * b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator *(float a, Vector5 b)
    {
        return b * a;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5 operator /(Vector5 a, float b)
    {
        return new Vector5(a.X / b, a.Y / b, a.Z / b, a.W / b, a.V / b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector5 a, Vector5 b)
    {
        return a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector5 a, Vector5 b)
    {
        return !a.Equals(b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector5 a, Vector5 b)
    {
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W + a.V * b.V;
    }
}
