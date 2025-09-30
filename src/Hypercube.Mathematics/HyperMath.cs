using JetBrains.Annotations;

namespace Hypercube.Mathematics;

/// <summary>
/// Represents a collection of mathematical constants and helper methods.
/// </summary>
[PublicAPI]
public static class HyperMath
{
    /// <summary>
    /// Represents the ratio of a circle's circumference to its diameter (π).
    /// </summary>
    public const double PI = Math.PI;
    
    /// <summary>
    /// Represents the ratio of a circle's circumference to its diameter (π), as a float.
    /// </summary>
    public const float PIf = MathF.PI;

    /// <summary>
    /// Represents π divided by 2.
    /// </summary>
    public const double PIOver2 = PI / 2;

    /// <summary>
    /// Represents π divided by 4.
    /// </summary>
    public const double PIOver4 = PI / 4;

    /// <summary>
    /// Represents π divided by 6.
    /// </summary>
    public const double PIOver6 = PI / 6;

    /// <summary>
    /// Represents 2π (a full circle in radians).
    /// </summary>
    public const double PITwo = PI * 2;

    /// <summary>
    /// Represents 3π divided by 2.
    /// </summary>
    public const double ThreePiOver2 = PI / 2 * 3;

    /// <summary>
    /// Multiplier to convert radians to degrees.
    /// </summary>
    public const double RadiansToDegrees = 180 / PI;

    /// <summary>
    /// Multiplier to convert degrees to radians.
    /// </summary>
    public const double DegreesToRadians = PI / 180;

    /// <summary>
    /// Represents π divided by 2 as a float.
    /// </summary>
    public const float PIOver2F = PIf / 2;

    /// <summary>
    /// Represents π divided by 4 as a float.
    /// </summary>
    public const float PIOver4F = PIf / 4;

    /// <summary>
    /// Represents π divided by 6 as a float.
    /// </summary>
    public const float PIOver6F = PIf / 6;

    /// <summary>
    /// Represents 2π as a float.
    /// </summary>
    public const float PITwoF = PIf * 2;

    /// <summary>
    /// Represents 3π divided by 2 as a float.
    /// </summary>
    public const float ThreePiOver2F = PIf / 2 * 3;

    /// <summary>
    /// Multiplier to convert radians to degrees (float).
    /// </summary>
    public const float RadiansToDegreesF = 180 / PIf;

    /// <summary>
    /// Multiplier to convert degrees to radians (float).
    /// </summary>
    public const float DegreesToRadiansF = PIf / 180;

    public const float FloatTolerance = 1e-15f;
    public const double DoubleTolerance = 1e-15d;
    
    /// <summary>
    /// Determines whether two floating-point values are approximately equal, within a specified tolerance.
    /// </summary>
    public static bool AboutEquals(float a, float b, float tolerance = FloatTolerance)
    {
        return float.Abs(a - b) <= float.Max(float.Abs(a), float.Abs(b)) * tolerance;
    }

    /// <summary>
    /// Determines whether two double-precision values are approximately equal, within a specified tolerance.
    /// </summary>
    public static bool AboutEquals(double a, double b, double tolerance = DoubleTolerance)
    {
        return double.Abs(a - b) <= double.Max(double.Abs(a), double.Abs(b)) * tolerance;
    }

    /// <summary>
    /// Moves a byte value toward a target value by a given distance without overshooting.
    /// </summary>
    public static byte MoveTowards(byte current, byte target, byte distance)
    {
        return current < target ?
            (byte) Math.Min(current + distance, target) :
            (byte) Math.Max(current - distance, target);
    }
    
    
    /// <summary>
    /// Moves a signed byte value toward a target value by a given distance without overshooting.
    /// </summary>
    public static sbyte MoveTowards(sbyte current,sbyte target, sbyte distance)
    {
        return current < target ?
            (sbyte) Math.Min(current + distance, target) :
            (sbyte) Math.Max(current - distance, target);
    }

    /// <summary>
    /// Moves a short value toward a target value by a given distance without overshooting.
    /// </summary>
    public static short MoveTowards(short current, short target, short distance)
    {
        return current < target ?
            (short) Math.Min(current + distance, target) :
            (short) Math.Max(current - distance, target);
    }
    
    /// <summary>
    /// Moves an unsigned short value toward a target value by a given distance without overshooting.
    /// </summary>
    public static ushort MoveTowards(ushort current, ushort target, ushort distance)
    {
        return current < target ?
            (ushort) Math.Min(current + distance, target) :
            (ushort) Math.Max(current - distance, target);
    }
    
    /// <summary>
    /// Moves an int value toward a target value by a given distance without overshooting.
    /// </summary>
    public static int MoveTowards(int current, int target, int distance)
    {
        if (distance < 0)
            throw new ArgumentOutOfRangeException(nameof(distance), "Distance must be non-negative.");
        
        return current < target ?
            int.Min(current + distance, target) :
            int.Max(current - distance, target);
    }

    /// <summary>
    /// Moves a native int value toward a target value by a given distance without overshooting.
    /// </summary>
    public static nint MoveTowards(nint current, nint target, nint distance)
    {
        return current < target ?
            nint.Min(current + distance, target) :
            nint.Max(current - distance, target);
    }
    
    /// <summary>
    /// Moves a long value toward a target value by a given distance without overshooting.
    /// </summary>
    public static long MoveTowards(long current, long target, long distance)
    {
        return current < target ?
            long.Min(current + distance, target) :
            long.Max(current - distance, target);
    }
    
    /// <summary>
    /// Moves an unsigned long value toward a target value by a given distance without overshooting.
    /// </summary>
    public static ulong MoveTowards(ulong current, ulong target, ulong distance)
    {
        return current < target ?
            ulong.Min(current + distance, target) :
            ulong.Max(current - distance, target);
    }

    /// <summary>
    /// Moves an unsigned int value toward a target value by a given distance without overshooting.
    /// </summary>
    public static uint MoveTowards(uint current, uint target, uint distance)
    {
        return current < target ?
            uint.Min(current + distance, target) :
            uint.Max(current - distance, target);
    }
    
    /// <summary>
    /// Moves a float value toward a target value by a given distance without overshooting.
    /// </summary>
    public static float MoveTowards(float current, float target, float distance)
    {
        return current < target ?
            float.Min(current + distance, target) :
            float.Max(current - distance, target);
    }
    
    /// <summary>
    /// Moves a double value toward a target value by a given distance without overshooting.
    /// </summary>
    public static double MoveTowards(double current, double target, double distance)
    {
        return current < target ?
            double.Min(current + distance, target) :
            double.Max(current - distance, target);
    }
}