using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hypercube.Mathematics.Matrices;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix3x3Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Formatting_ToString
{
    private static readonly Matrix3x3 TestMatrix = new(
        1.123456f, 2.2f, 3.3f,
             4.4f, 5.5f, 6.6f,
             7.7f, 8.8f, 9.9f
    );

    [Test]
    public void ToString_Default_IsFlat()
    {
        const string expected = "1.123, 2.2, 3.3, 4.4, 5.5, 6.6, 7.7, 8.8, 9.9";
        Assert.That(TestMatrix.ToString(), Is.EqualTo(expected));
    }

    [Test]
    public void ToString_Bars_Format()
    {
        const string expected = "|    1.123      2.2      3.3 |\n" +
                                "|      4.4      5.5      6.6 |\n" +
                                "|      7.7      8.8      9.9 |";
        Assert.That(TestMatrix.ToString("b"), Is.EqualTo(expected));
    }

    [Test]
    public void ToString_Csv_WithInnerFormat()
    {
        const string expected = "1.12;2.20;3.30\n" +
                                "4.40;5.50;6.60\n" +
                                "7.70;8.80;9.90";
        Assert.That(TestMatrix.ToString("cF2"), Is.EqualTo(expected));
    }

    [Test]
    public void ToString_Initializer_Format()
    {
        const string expected = "new Matrix3x3(1.123f, 2.2f, 3.3f, 4.4f, 5.5f, 6.6f, 7.7f, 8.8f, 9.9f)";
        Assert.That(TestMatrix.ToString("i"), Is.EqualTo(expected));
    }

    [Test]
    public void ToString_CustomCulture_Format()
    {
        var culture = new CultureInfo("ru-RU");
        var result = TestMatrix.ToString("f", culture);
        Assert.That(result, Contains.Substring("1,123"));
    }

    [Test]
    public void ToString_InvalidFormat_ThrowsException()
    {
        Assert.Throws<FormatException>(() =>
        {
            _ = TestMatrix.ToString("z");
        });
    }
}