using System.Reflection;
using System.Text.RegularExpressions;
using Fun3.TokensDefault;

namespace Fun3.TokenServices;

public class DefaultTokenService : ITokenService
{
    public Dictionary<string, Type> Registry { get; } = [];
    public Dictionary<Type, int> Rankings { get; } = [];

    public void PopulateRegistry()
    {
        Registry[@"\w+"] = typeof(WordToken);
        Registry[@"###.*"] = typeof(CommentToken);
        Registry[@"\.\.\."] = typeof(EllipsisToken);
        Registry[@"\s{4}|\t"] = typeof(IndentToken);
        Registry[@"\+"] = typeof(PlusToken);
        Registry["\".*\""] = typeof(StringLiteralToken);
    }

    public void PopulateRankings()
    {
        Rankings[typeof(WordToken)] = 0;
        Rankings[typeof(CommentToken)] = 2;
        Rankings[typeof(EllipsisToken)] = 0;
        Rankings[typeof(IndentToken)] = 0;
        Rankings[typeof(PlusToken)] = 0;
        Rankings[typeof(StringLiteralToken)] = 1;
        Rankings[typeof(IToken)] = -1;
    }

    public IToken? CreateToken(string pattern, int line, Match m)
    {
        if (!m.Success)
            return null;

        var ret = (IToken)(
            Activator.CreateInstance(Registry[pattern]) ?? throw new InvalidOperationException()
        );
        ret.Line = line;
        ret.Value = m.ToString();
        ret.StartColumn = m.Index;
        ret.EndColumn = m.Index + m.Length - 1;
        return ret;
    }
}
