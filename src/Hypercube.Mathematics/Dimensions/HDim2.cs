using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Dimensions;

[PublicAPI]
public readonly struct HDim2
{
    public static readonly HDim2 Zero = new(HDim.Zero, HDim.Zero);
    public static readonly HDim2 ScalarHalf = new(0.5f, 0, 0.5f, 0.5f);
    public static readonly HDim2 ScalarOne = new(1, 0, 1, 0);

    public readonly HDim X;
    public readonly HDim Y;


    public HDim2(HDim x, HDim y)
    {
        X = x;
        Y = y;
    }

    public HDim2(float scalarX, float offsetX, float scalarY, float offsetY)
    {
        X = new HDim(scalarX, offsetX);
        Y = new HDim(scalarY, offsetY);
    }

    public Vector2 Resolve(Vector2 space) =>
        new (X.Resolve(space.X), Y.Resolve(space.Y));
}