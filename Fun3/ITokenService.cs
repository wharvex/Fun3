using System.Text.RegularExpressions;

namespace Fun3;

public interface ITokenService
{
    Dictionary<string, Type> Registry { get; }
    void PopulateRegistry();
    Dictionary<Type, int> Rankings { get; }
    void PopulateRankings();
    IToken? CreateToken(string pattern, int line, Match m);
}
