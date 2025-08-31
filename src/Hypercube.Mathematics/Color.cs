using System.Globalization;
using System.Runtime.InteropServices;
using Hypercube.Mathematics.Extensions;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics;

[PublicAPI, Serializable, StructLayout(LayoutKind.Sequential)]
public readonly struct Color
{
    private const float DefaultAlpha = 1f;
    
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

    #region To byte

    public byte ByteR => (byte) (R * byte.MaxValue);
    public byte ByteG => (byte) (G * byte.MaxValue);
    public byte ByteB => (byte) (B * byte.MaxValue);
    public byte ByteA => (byte) (A * byte.MaxValue);

    #endregion
    
    public int Int => (ByteR << 24) | (ByteG << 12) | (ByteB << 8) | ByteA;
    public uint Uint => (uint) Int;
    public Vector3 Vec3 => new(R, G, B);
    public Vector4 Vec4 => new(R, G, B, A);
    public Color Abgr => new(A, B, G, R);
    public Color Bgr => new(B, G, R);
    
    public readonly float R;
    public readonly float G;
    public readonly float B;
    public readonly float A;

    public Color(int value)
    {
        R = ((value >> 24) & 0xFF).ByteToNormalizedFloat();
        G = ((value >> 16) & 0xFF).ByteToNormalizedFloat();
        B = ((value >> 8) & 0xFF).ByteToNormalizedFloat();
        A = (value & 0xFF).ByteToNormalizedFloat();
    }
    
    public Color(float r, float g, float b, float a = DefaultAlpha)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(byte r, byte g, byte b, byte a = byte.MaxValue)
    {
        R = r.ByteToNormalizedFloat();
        G = g.ByteToNormalizedFloat();
        B = b.ByteToNormalizedFloat();
        A = a.ByteToNormalizedFloat();
    }
    
    public Color(Vector3 vector, float a = DefaultAlpha)
    {
        R = vector.X;
        G = vector.Y;
        B = vector.Z;
        A = a;
    }

    public Color(Vector4 vector)
    {
        R = vector.X;
        G = vector.Y;
        B = vector.Z;
        A = vector.W;
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
        const int rgbSize = 6;
        const int rgbaSize = 8;
        
        if (hex.StartsWith('#'))
            hex = hex[1..];

        if (hex.Length != rgbSize && hex.Length != rgbaSize)
            throw new ArgumentException("Hex string must be 6 (RGB) or 8 (RGBA) characters long.");

        R = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber).ByteToNormalizedFloat();
        G = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber).ByteToNormalizedFloat();
        B = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber).ByteToNormalizedFloat();
        
        A = hex.Length == rgbaSize
            ? int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber).ByteToNormalizedFloat()
            : DefaultAlpha;
    }

    /// <summary>
    /// Returns the color as a hexadecimal string in the format #RRGGBBAA.
    /// </summary>
    public override string ToString()
    {
        return $"#{ByteR:X2}{ByteG:X2}{ByteB:X2}{ByteA:X2}";
    }
}