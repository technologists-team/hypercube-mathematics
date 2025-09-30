using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
public readonly struct Color
{
    #region Colors
    
    public static readonly Color White        = new("ffffff");
    public static readonly Color Black        = new("000000");
    public static readonly Color Red          = new("ff0000");
    public static readonly Color Green        = new("00ff00");
    public static readonly Color Blue         = new("0000ff");
    public static readonly Color Yellow       = new("ffff00");
    public static readonly Color Cyan         = new("00ffff");
    public static readonly Color Magenta      = new("ff00ff");
    public static readonly Color Orange       = new("ffa500");
    public static readonly Color Purple       = new("800080");
    public static readonly Color Gray         = new("808080");
    public static readonly Color LightGray    = new("d3d3d3");
    public static readonly Color DarkGray     = new("404040");
    public static readonly Color Silver       = new("c0c0c0");
    public static readonly Color Gold         = new("ffd700");
    public static readonly Color Brown        = new("a52a2a");
    public static readonly Color Maroon       = new("800000");
    public static readonly Color Olive        = new("808000");
    public static readonly Color Lime         = new("bfff00");
    public static readonly Color Navy         = new("000080");
    public static readonly Color Teal         = new("008080");
    public static readonly Color Aqua         = new("00ffff");
    public static readonly Color Indigo       = new("4b0082");
    public static readonly Color Violet       = new("ee82ee");
    public static readonly Color Pink         = new("ffc0cb");
    public static readonly Color HotPink      = new("ff69b4");
    public static readonly Color Salmon       = new("fa8072");
    public static readonly Color Coral        = new("ff7f50");
    public static readonly Color Tomato       = new("ff6347");
    public static readonly Color OrangeRed    = new("ff4500");
    public static readonly Color Mint         = new("98ff98");
    public static readonly Color Turquoise    = new("40e0d0");
    public static readonly Color SkyBlue      = new("87ceeb");
    public static readonly Color SteelBlue    = new("4682b4");
    public static readonly Color RoyalBlue    = new("4169e1");
    public static readonly Color MidnightBlue = new("191970");
    public static readonly Color Beige        = new("f5f5dc");
    public static readonly Color Khaki        = new("f0e68c");
    public static readonly Color Tan          = new("d2b48c");
    public static readonly Color Wheat        = new("f5deb3");
    public static readonly Color Chocolate    = new("d2691e");
    public static readonly Color SaddleBrown  = new("8b4513");
        
    #endregion
    
    public float NormalizedR => R.ByteToNormalizedFloat();
    public float NormalizedG => G.ByteToNormalizedFloat();
    public float NormalizedB => B.ByteToNormalizedFloat();
    public float NormalizedA => A.ByteToNormalizedFloat();
    public int Int => (R << 24) | (G << 12) | (B << 8) | A;
    public uint Uint => (uint) Int;
    public Vector3 Vec3 => new(NormalizedR, NormalizedG, NormalizedB);
    public Vector4 Vec4 => new(NormalizedR, NormalizedG, NormalizedB, NormalizedA);
    public Color Abgr => new(A, B, G, R);
    public Color Bgr => new(B, G, R);
    
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    public readonly byte A;
    
    public Color(byte r, byte g, byte b, byte a = byte.MaxValue)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
    
    public Color(int value)
    {
        R = (byte) ((value >> 24) & 0xFF);
        G = (byte) ((value >> 16) & 0xFF);
        B = (byte) ((value >> 8) & 0xFF);
        A = (byte) (value & 0xFF);
    }
    
    public Color(float r, float g, float b, float a = 1f)
    {
        R = r.NormalizedFloatToByte();
        G = g.NormalizedFloatToByte();
        B = b.NormalizedFloatToByte();
        A = a.NormalizedFloatToByte();
    }
    
    public Color(Vector3 vector, float a = 1f)
    {
        R = vector.X.NormalizedFloatToByte();
        G = vector.Y.NormalizedFloatToByte();
        B = vector.Z.NormalizedFloatToByte();
        A = a.NormalizedFloatToByte();
    }

    public Color(Vector4 vector)
    {
        R = vector.X.NormalizedFloatToByte();
        G = vector.Y.NormalizedFloatToByte();
        B = vector.Z.NormalizedFloatToByte();
        A = vector.W.NormalizedFloatToByte();
    }
    
    public Color(Color color)
    {
        R = color.R;
        G = color.G;
        B = color.B;
        A = color.A;
    }
    
    /// <summary>
    /// Parses a hexadecimal color string (#RRGGBB or #RRGGBBAA).
    /// </summary>
    /// <param name="hex">The hexadecimal color string.</param>
    /// <exception cref="ArgumentException">Thrown if the hex string is invalid.</exception>
    public Color(string hex)
    {
        const char prefix = '#';
        const int rgbSize = 6;
        const int rgbaSize = 8;
        
        if (hex.StartsWith(prefix))
            hex = hex[1..];

        if (hex.Length != rgbSize && hex.Length != rgbaSize)
            throw new ArgumentException("Hex string must be 6 (RGB) or 8 (RGBA) characters long.");

        R = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        G = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        B = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        A = hex.Length == rgbaSize
            ? byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber)
            : byte.MaxValue;
    }

    /// <summary>
    /// Returns the color as a hexadecimal string in the format #RRGGBBAA.
    /// </summary>
    public override string ToString()
    {
        return $"#{R:X2}{G:X2}{B:X2}{A:X2}";
    }
    
    #region Implicit
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (byte, byte, byte)(Color color)
    {
        return (color.R, color.G, color.B);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (byte, byte, byte, byte)(Color color)
    {
        return (color.R, color.G, color.B, color.A);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (float, float, float)(Color color)
    {
        return (color.NormalizedR, color.NormalizedG, color.NormalizedB);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (float, float, float, float)(Color color)
    {
        return (color.NormalizedR, color.NormalizedG, color.NormalizedB, color.NormalizedA);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Color color)
    {
        return color.Vec3;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4(Color color)
    {
        return color.Vec4;
    }
    
    #endregion
}