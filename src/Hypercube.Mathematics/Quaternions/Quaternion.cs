using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Quaternions;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
public readonly struct Quaternion : IEquatable<Quaternion> 
{
    private const float SingularityThreshold = 0.4999995f;

    /// <summary>
    /// Represents a quaternion with no rotation (0, 0, 0, 1).
    /// </summary>
    public static readonly Quaternion Identity = new(0, 0, 0, 1);

    /// <summary>
    /// Represents a quaternion with all components set to zero.
    /// </summary>
    public static readonly Quaternion Zero = new(0, 0, 0, 0);
    
    public readonly Vector4 Vector;

    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.LengthSquared;
    }
    
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.Length;
    }
    
    public Quaternion Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new (Vector.Normalized);
    }
    
    public Vector3 Direction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.Xyz;
    }
    
    public Angle Angle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Vector.W);
    }
    
    public float X
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.X;
    }
    
    public float Y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.Y;
    }
    
    public float Z
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.Z;
    }
    
    public float W
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vector.W;
    }
    
    public Quaternion(Vector4 vector)
    {
        Vector = vector;
    }
    
    public Quaternion(Vector3 vector3) : this(FromEuler(vector3).Vector)
    {
    }
    
    public Quaternion(Quaternion quaternion) : this(quaternion.Vector)
    {
    }
    
    public Quaternion(float x, float y, float z, float w) : this(new Vector4(x, y, z, w))
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithX(float value)
    {
        return new Quaternion(Vector.WithX(value));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithY(float value)
    {
        return new Quaternion(Vector.WithY(value));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithZ(float value)
    {
        return new Quaternion(Vector.WithZ(value));
    }    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Quaternion WithW(float value)
    {
        return new Quaternion(Vector.WithW(value));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToEuler()
    {
        return ToEuler(this);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToEulerDeg()
    {
        return ToEuler(this) * HyperMath.RadiansToDegreesF;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Quaternion other)
    {
        return Vector == other.Vector;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Quaternion other && Equals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return Vector.GetHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return Vector.ToString();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Quaternion a, Quaternion b)
    {
        return a.Equals(b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Quaternion a, Quaternion b)
    {
        return !a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion FromEuler(Vector3 vector) => FromEuler(vector.X, vector.Y, vector.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion FromEulerZ(float z) => FromEuler(0, 0, z);

    /// <summary>
    /// Created new <see cref="Quaternion"/> from given Euler angles in radians.
    /// </summary>
    public static Quaternion FromEuler(float x, float y, float z)
    {
        var nx = x / 2f;
        var ny = y / 2f;
        var nz = z / 2f;
        var cx = float.Cos(nx);
        var cy = float.Cos(ny);
        var cz = float.Cos(nz);
        var sx = float.Sin(nx);
        var sy = float.Sin(ny);
        var sz = float.Sin(nz);
        return new Quaternion(
            sx * cy * cz + cx * sy * sz,
            cx * sy * cz - sx * cy * sz,
            cx * cy * sz + sx * sy * cz,
            cx * cy * cz - sx * sy * sz
        );
    }

    /// <summary>
    /// Convert this instance to an Euler angle representation.
    /// <remarks>
    /// Taken from <a href="https://github.com/opentk/opentk/blob/master/src/OpenTK.Mathematics/Data/Quaternion.cs#L194">OpenTK.Mathematics/Data/Quaternion.cs</a>
    /// </remarks>
    /// </summary>
    /// <returns>Euler angle in radians</returns>
    public static Vector3 ToEuler(Quaternion quaternion)
    {
        var sqx = quaternion.X * quaternion.X;
        var sqy = quaternion.Y * quaternion.Y;
        var sqz = quaternion.Z * quaternion.Z;
        var sqw = quaternion.W * quaternion.W;
        
        var unit = sqx + sqy + sqz + sqw; // If normalized is one, otherwise is correction factor
        var singularityTest = quaternion.X * quaternion.Z + quaternion.W * quaternion.Y;
        
        if (singularityTest > SingularityThreshold * unit)
            // Singularity at North Pole
            return new Vector3(
                0,
                HyperMath.PIOver2F,
                2f * float.Atan2(quaternion.X, quaternion.W)
            );

        if (singularityTest < -SingularityThreshold * unit)
            // Singularity at South Pole
            return new Vector3(
                0,
                -HyperMath.PIOver2F,
                -2f * float.Atan2(quaternion.X, quaternion.W)
            );

        return new Vector3(
            float.Atan2(2 * (quaternion.W * quaternion.X - quaternion.Y * quaternion.Z), sqw - sqx - sqy + sqz),
            float.Asin(2 * singularityTest / unit),
            float.Atan2(2 * (quaternion.W * quaternion.Z - quaternion.X * quaternion.Y), sqw + sqx - sqy - sqz)
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion operator +(Quaternion a, Quaternion b) => new(a.Vector + b.Vector);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion operator -(Quaternion a, Quaternion b) => new(a.Vector - b.Vector);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion operator -(Quaternion a) => new(-a.Vector);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
        return new Quaternion(
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
            a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X,
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z
        );
    }
}