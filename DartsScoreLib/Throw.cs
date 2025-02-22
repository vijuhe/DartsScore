namespace DartsScoreLib;

public record Throw
{
    public bool IsDouble { get; internal set; }
    public bool IsTriple { get; internal set; }
    public byte Score { get; internal set; }
}