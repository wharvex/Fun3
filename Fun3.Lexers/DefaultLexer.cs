using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace Fun3.Lexers;

public class DefaultLexer(string[] lines) : ILexer
{
    public ITokenService TokenService { get; set; } =
        ProviderContainer.Provider.GetRequiredService<ITokenService>();

    public string[] Lines { get; set; } = lines;

    public void Lex()
    {
        for (var i = 0; i < Lines.Length; i++)
        {
            var line = Lines[i];
            var lineNumber = i + 1;
            Console.WriteLine($"Line {lineNumber}: {line}");
            var lineTokens = TokenService.CreateLineTokens(line, lineNumber);
            var overlaps = CollectOverlaps(lineTokens);
            DebugPrint(lineTokens, overlaps);
        }
    }

    private static List<(IToken, IToken)> CollectOverlaps(List<IToken?> lineTokens)
    {
        List<(IToken, IToken)> ret = [];
        for (var i = 0; i < lineTokens.Count - 1; i++)
        {
            var tok1 = lineTokens[i];
            if (tok1 is null)
                continue;
            for (var j = i + 1; j < lineTokens.Count; j++)
            {
                var tok2 = lineTokens[j];
                if (tok2 is null)
                    continue;
                if (tok1.Overlaps(tok2))
                    ret.Add((tok1, tok2));
            }
        }

        return ret;
    }

    private void DebugPrint(List<IToken?> lineTokens, List<(IToken, IToken)> overlaps)
    {
        var tokenValues = string.Join(", ", lineTokens.Select(t => t?.Value ?? "N/A"));
        var tokenTypes = string.Join(", ", lineTokens.Select(t => t?.ToString() ?? "N/A"));
        var tokenOverlaps = string.Join(
            "\n",
            overlaps.Select(tuple =>
                tuple.Item1
                + " (Rank "
                + TokenService.Rankings[tuple.Item1.GetType()]
                + ") and "
                + tuple.Item2
                + " (Rank "
                + TokenService.Rankings[tuple.Item2.GetType()]
                + ")"
            )
        );
        Console.WriteLine($"\nTOKEN VALUES FOUND: {tokenValues}");
        Console.WriteLine($"\nTOKEN TYPES FOUND: {tokenTypes}");
        Console.WriteLine($"\nOVERLAPS FOUND: {tokenOverlaps}");
        Console.WriteLine("\n");
    }
}
