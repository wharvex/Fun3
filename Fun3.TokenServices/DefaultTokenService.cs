using System.Reflection;
using System.Text.RegularExpressions;
using Fun3.TokensDefault;

namespace Fun3.TokenServices;

public class DefaultTokenService : ITokenService
{
    public Dictionary<string, Type> Registry { get; } = [];

    public void PopulateRegistry()
    {
        Registry[@"^\w+"] = typeof(WordToken);
        Registry[@"^###.*"] = typeof(CommentToken);
        Registry[@"^\.\.\."] = typeof(EllipsisToken);
    }

    public IToken CreateToken(string pattern, int line, Match m)
    {
        var ret = (IToken)(
            Activator.CreateInstance(Registry[pattern]) ?? throw new InvalidOperationException()
        );
        ret.Line = line;
        ret.Value = m.ToString();
        ret.StartColumn = m.Index;
        ret.EndColumn = m.Index + m.Length;
        return ret;
    }
}
