using System.Text.RegularExpressions;

namespace Fun3;

public interface ITokenService
{
    Dictionary<string, Type> Registry { get; }
    void PopulateRegistry();
    IToken? CreateToken(string pattern, int line, Match m);
}
