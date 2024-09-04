namespace Fun3;

public interface IToken
{
    int StartColumn { get; set; }
    int EndColumn { get; set; }
    int Line { get; set; }
    string Value { get; set; }
    bool Surrounds(IToken other) =>
        StartColumn <= other.StartColumn && EndColumn >= other.EndColumn;
    bool Overlaps(IToken other) =>
        (StartColumn >= other.StartColumn && StartColumn < other.EndColumn)
        || (EndColumn >= other.StartColumn && EndColumn < other.EndColumn);
}
