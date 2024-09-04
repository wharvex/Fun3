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
                    overlaps.Select(tuple => tuple.Item1 + " overlaps " + tuple.Item2)
                );
                Console.WriteLine(tokenValues);
                Console.WriteLine(tokenTypes);
                Console.WriteLine(tokenOverlaps);
                //TokenService
                //    .Registry.Select(kvp =>
                //    {
                //        var rg = new Regex(kvp.Key);
                //        var matches = rg.Matches(line);
                //        return (
                //            Tokens: matches.Select(m => TokenService.CreateToken(kvp.Key, i, m)),
                //            KVP: kvp
                //        );
                //    })
                //    .ToList()
                //    .ForEach(tuple =>
                //    {
                //        var found = tuple.Tokens.Any(t => t is not null);
                //        var foundString = found ? "yes" : "no";
                //        var tokenVals = string.Join(
                //            ", ",
                //            tuple.Tokens.Select(t => t?.Value ?? "N/A")
                //        );
                //        var searchedToken = tuple.KVP.Value.FullName ?? "??";
                //        var tokenRanges = string.Join(
                //            "; ",
                //            tuple.Tokens.Select(
                //                (t, j) =>
                //                    $"Token number: {j}, Token start column: {t?.StartColumn ?? -1}, Token end column: {t?.EndColumn ?? -1}"
                //            )
                //        );
                //        Console.WriteLine($"Token type searched: {searchedToken}");
                //        Console.WriteLine($"Any matches? {foundString}");
                //        Console.WriteLine($"Found token vals: {tokenVals}");
                //        Console.WriteLine($"Found token ranges: {tokenRanges}");
                //        Console.WriteLine("\n");
                //    });
            });
    }
}
