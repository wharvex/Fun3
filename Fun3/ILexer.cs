namespace Fun3;

public interface ILexer
{
    ITokenService TokenService { get; }

    string[] Lines { get; set; }

    void Lex();
}
