namespace Hypercube.Mathematics.Dimensions;

public struct HDimRect
{
    public readonly HDim Bottom;
    public readonly HDim Left;
    public readonly HDim Top;
    public readonly HDim Right;

    public HDimRect(HDim bottom, HDim left, HDim top, HDim right)
    {
        Bottom = bottom;
        Left = left;
        Top = top;
        Right = right;
    }
}