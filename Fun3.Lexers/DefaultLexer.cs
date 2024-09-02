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
                TokenService
                    .Registry.Select(kvp =>
                    {
                        var rg = new Regex(kvp.Key);
                        var matches = rg.Matches(line);
                        return (
                            Tokens: matches.Select(m => TokenService.CreateToken(kvp.Key, i, m)),
                            KVP: kvp
                        );
                    })
                    .ToList()
                    .ForEach(tuple =>
                    {
                        var found = tuple.Tokens.Any(t => t is not null);
                        var foundString = found ? "yes" : "no";
                        var tokenVals = string.Join(
                            ", ",
                            tuple.Tokens.Select(t => t?.Value ?? "N/A")
                        );
                        var searchedToken = tuple.KVP.Value.FullName ?? "??";
                        var tokenRanges = string.Join(
                            "; ",
                            tuple.Tokens.Select(
                                (t, j) =>
                                    $"Token number: {j}, Token start column: {t?.StartColumn ?? -1}, Token end column: {t?.EndColumn ?? -1}"
                            )
                        );
                        Console.WriteLine($"Token type searched: {searchedToken}");
                        Console.WriteLine($"Any matches? {foundString}");
                        Console.WriteLine($"Found token vals: {tokenVals}");
                        Console.WriteLine($"Found token ranges: {tokenRanges}");
                        Console.WriteLine("\n");
                    });
            });
    }
}
