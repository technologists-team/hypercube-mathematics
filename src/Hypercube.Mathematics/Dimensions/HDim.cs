using JetBrains.Annotations;

namespace Hypercube.Mathematics.Dimensions;

[PublicAPI]
public readonly struct HDim
{
    public static readonly HDim Zero = new(0, 0);
    
    public readonly float Scalar;
    public readonly float Offset;

    public HDim(float scalar, float offset)
    {
        Scalar = scalar;
        Offset = offset;
    }
    
    public float Resolve(float space) => space * Scalar + Offset;
}