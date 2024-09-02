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
        TokenService
            .Registry.Select(kvp =>
            {
                var rg = new Regex(kvp.Key);
                return (Pattern: kvp.Key, Match: rg.Match(Lines[0]));
            })
            .Select(t => TokenService.CreateToken(t.Pattern, 0, t.Match))
            .ToList()
            .ForEach(Console.WriteLine);
    }
}
