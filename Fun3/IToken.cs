namespace Fun3;

public interface IToken
{
    int StartColumn { get; set; }
    int EndColumn { get; set; }
    int Line { get; set; }
    string Value { get; set; }
}
