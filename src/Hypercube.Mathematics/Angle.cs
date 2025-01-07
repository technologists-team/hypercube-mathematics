﻿using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential), DebuggerDisplay("{Theta} ({Degrees})")]
public readonly struct Angle : IEquatable<Angle>, IEquatable<double>
{
    public static readonly Angle Zero = new(0);
    
    public readonly double Theta;
    
    public double Degrees
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Theta * HyperMath.RadiansToDegrees;
    }

    public Vector2 Vector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(MathF.Cos((float) Theta), MathF.Sin((float) Theta));
    }

    public Angle(double theta)
    {
        Theta = theta;
    }
    
    public Angle(Vector2 value)
    {
        Theta = value.Angle;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Angle other)
    {
        return Theta.AboutEquals(other.Theta);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(double other)
    {
        return Theta.AboutEquals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return (obj is double theta && Equals(theta)) ||
               (obj is Angle angle && Equals(angle));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return Theta.GetHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"{Degrees} deg";
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Angle a, double b)
    {
        return a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Angle a, double b)
    {
        return !a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Angle a, Angle b)
    {
        return a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Angle a, Angle b)
    {
        return !a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator double(Angle angle)
    {
        return angle.Theta;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Angle(double radians)
    {
        return new Angle(radians);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Angle(float radians)
    {
        return new Angle(radians);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Angle FromDegrees(double degrees)
    {
        return new Angle(degrees * HyperMath.DegreesToRadians);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Angle FromDegrees(float degrees)
    {
        return new Angle(degrees * HyperMath.DegreesToRadians);
    }
}