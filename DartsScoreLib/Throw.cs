namespace DartsScoreLib;

public record Throw
{
    public Multiplier Multiplier { get; internal set; }
    public byte Score { get; internal set; }
}