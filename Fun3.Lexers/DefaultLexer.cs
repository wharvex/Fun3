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
        var pattern = @"\w+";
        var rg = new Regex(pattern);

        var match = rg.Match(Lines[0]);
        Console.WriteLine(match.Success ? match.Index : "not found");
        TokenService.PopulateRegistry();
        Console.WriteLine(TokenService.Registry[pattern].FullName);
    }
}
