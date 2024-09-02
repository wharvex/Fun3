namespace Fun3.Lexers;

public class DefaultLexer(string[] lines, ITokenService tokenService) : ILexer
{
    public ITokenService TokenService { get; set; } = tokenService;

    public string[] Lines { get; set; } = lines;

    public void Lex()
    {
        throw new NotImplementedException();
    }
}
