using Hypercube.Mathematics;
using Hypercube.Mathematics.Matrices;

namespace Hypercube.Tests;

public static class Program
{
    public static void Main(string[] _)
    {
        var numericsTranslation = System.Numerics.Matrix4x4.CreateTranslation(10, 15, 20);
        var numericsRotation = System.Numerics.Matrix4x4.CreateRotationZ(HyperMath.DegreesToRadiansF * 90f);
        var numericsScale = System.Numerics.Matrix4x4.CreateScale(1.5f, 2f, 5f);
        var numericsMatrix = numericsTranslation * numericsRotation * numericsScale;
        
        var hyperTranslation = Matrix4x4.CreateTranslation(10, 15, 20);
        var hyperRotation = Matrix4x4.CreateRotationZ(HyperMath.DegreesToRadiansF * 90f);
        var hyperScale = Matrix4x4.CreateScale(1.5f, 2f, 5f);
        var hyperMatrix = hyperTranslation * hyperRotation * hyperScale;
        
        Console.WriteLine(((Matrix4x4) numericsMatrix).ToString());
        Console.WriteLine("======");
        Console.WriteLine(hyperMatrix.ToString());
    }
}