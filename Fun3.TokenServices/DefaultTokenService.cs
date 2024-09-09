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
        Rankings[typeof(WordToken)] = 1;
        Rankings[typeof(CommentToken)] = 3;
        Rankings[typeof(EllipsisToken)] = 1;
        Rankings[typeof(IndentToken)] = 0;
        Rankings[typeof(PlusToken)] = 1;
        Rankings[typeof(StringLiteralToken)] = 2;
        Rankings[typeof(IToken)] = -1;
    }

    public DefaultTokenService()
    {
        PopulateRegistry();
        PopulateRankings();
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

    public List<IToken?> CreateLineTokens(string line, int lineNumber)
    {
        return Registry
            .SelectMany(kvp =>
            {
                var rg = new Regex(kvp.Key);
                var matches = rg.Matches(line);
                return matches.Select(m => CreateToken(kvp.Key, lineNumber, m));
            })
            .ToList();
    }
}
