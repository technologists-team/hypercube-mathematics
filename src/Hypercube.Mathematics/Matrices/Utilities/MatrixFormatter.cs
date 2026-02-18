using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace Hypercube.Mathematics.Matrices.Utilities;

[PublicAPI]
public static class MatrixFormatter
{
    public const char FormatBars = 'b';
    public const char FormatCsv = 'c';
    public const char FormatInitializer = 'i';
    public const char FormatFlat = 'f';
    public const string DefaultInnerFormat = "G4";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(string? format, IFormatProvider? provider, string typeName, int dimensionality,
        ReadOnlySpan<float> elements)
    {
        if (string.IsNullOrWhiteSpace(format))
            format = FormatBars.ToString();

        provider ??= CultureInfo.InvariantCulture;

        var mode = char.ToLowerInvariant(format[0]);
        var inner = format.Length > 1 ? format[1..] : DefaultInnerFormat;

        return mode switch
        {
            FormatBars => FormatAsBars(elements, dimensionality, inner, provider),
            FormatCsv => FormatAsCsv(elements, dimensionality, inner, provider),
            FormatInitializer => FormatAsInitializer(typeName, elements, dimensionality, inner, provider),
            FormatFlat => FormatAsFlat(elements, dimensionality, inner, provider),
            _ => throw new FormatException($"The format '{format}' is not supported.")
        };
    }

    private static string FormatAsBars(ReadOnlySpan<float> elements, int dim, string inner, IFormatProvider provider)
    {
        var sb = new StringBuilder();
        for (var r = 0; r < dim; r++)
        {
            sb.Append("| ");
            for (var c = 0; c < dim; c++)
            {
                var val = elements[c * dim + r].ToString(inner, provider);
                sb.Append($"{val,8} ");
            }

            sb.Append('|');
            if (r < dim - 1) sb.Append('\n');
        }

        return sb.ToString();
    }

    private static string FormatAsCsv(ReadOnlySpan<float> elements, int dim, string inner, IFormatProvider provider)
    {
        var sb = new StringBuilder();
        for (var r = 0; r < dim; r++)
        {
            for (var c = 0; c < dim; c++)
            {
                sb.Append(elements[c * dim + r].ToString(inner, provider));
                if (c < dim - 1) sb.Append(';');
            }

            if (r < dim - 1) sb.Append('\n');
        }

        return sb.ToString();
    }

    private static string FormatAsInitializer(string name, ReadOnlySpan<float> elements, int dim, string inner,
        IFormatProvider provider)
    {
        var sb = new StringBuilder();
        sb.Append("new ").Append(name).Append('(');

        for (var r = 0; r < dim; r++)
        {
            for (var c = 0; c < dim; c++)
            {
                sb.Append(elements[c * dim + r].ToString(inner, provider)).Append('f');
                if (r < dim - 1 || c < dim - 1) sb.Append(", ");
            }
        }

        sb.Append(')');
        return sb.ToString();
    }

    private static string FormatAsFlat(ReadOnlySpan<float> elements, int dim, string inner, IFormatProvider provider)
    {
        var sb = new StringBuilder();
        for (var r = 0; r < dim; r++)
        {
            for (var c = 0; c < dim; c++)
            {
                sb.Append(elements[c * dim + r].ToString(inner, provider));
                if (r < dim - 1 || c < dim - 1) sb.Append(", ");
            }
        }

        return sb.ToString();
    }
}