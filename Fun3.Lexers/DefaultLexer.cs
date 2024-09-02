using Microsoft.Extensions.DependencyInjection;

namespace Fun3.Lexers;

public class DefaultLexer(string[] lines) : ILexer
{
    public ITokenService TokenService { get; set; } =
        ProviderContainer.Provider.GetRequiredService<ITokenService>();

    public string[] Lines { get; set; } = lines;

    public void Lex()
    {
        throw new NotImplementedException();
    }
}
