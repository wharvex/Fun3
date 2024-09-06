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
        TokenService.PopulateRegistry();
        TokenService.PopulateRankings();
        Enumerable
            .Range(0, Lines.Length)
            .ToList()
            .ForEach(i =>
            {
                var line = Lines[i];
                var finalIndexOnLine = line.Length - 1;
                Console.WriteLine($"Line {i}: {line}");
                Console.WriteLine($"Line {i} final idx: {finalIndexOnLine}\n");
                var w = TokenService
                    .Registry.SelectMany(kvp =>
                    {
                        var rg = new Regex(kvp.Key);
                        var matches = rg.Matches(line);
                        return matches.Select(m => TokenService.CreateToken(kvp.Key, i, m));
                    })
                    .ToList();
                List<(IToken, IToken)> overlaps = [];
                for (int j = 0; j < w.Count - 1; j++)
                {
                    var tok1 = w[j];
                    if (tok1 is null)
                        continue;
                    for (int k = j + 1; k < w.Count; k++)
                    {
                        var tok2 = w[k];
                        if (tok2 is null)
                            continue;
                        if (tok1.Overlaps(tok2))
                            overlaps.Add((tok1, tok2));
                    }
                }
                var tokenValues = string.Join(", ", w.Select(t => t?.Value ?? "N/A"));
                var tokenTypes = string.Join(", ", w.Select(t => t?.ToString() ?? "N/A"));
                var tokenOverlaps = string.Join(
                    "\n",
                    overlaps.Select(
                        tuple =>
                            tuple.Item1
                            + " ("
                            + TokenService.Rankings[tuple.Item1.GetType()]
                            + ") and "
                            + tuple.Item2
                            + " ("
                            + TokenService.Rankings[tuple.Item2.GetType()]
                            + ")"
                    )
                );
                Console.WriteLine($"\nTOKEN VALUES FOUND: {tokenValues}");
                Console.WriteLine($"\nTOKEN TYPES FOUND: {tokenTypes}");
                Console.WriteLine($"\nOVERLAPS FOUND: {tokenOverlaps}");
                Console.WriteLine("\n");
            });
    }
}
