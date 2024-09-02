namespace Fun3.TokensDefault;

public class PlusToken : IToken
{
    public int StartColumn { get; set; }
    public int EndColumn { get; set; }
    public int Line { get; set; }
    public string Value { get; set; }
}
