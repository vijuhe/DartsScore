namespace DartsScoreLib;

public record WinningRound
{
    public bool CanWin { get; internal set; }
    public Throw? FirstThrow { get; internal set; }
    public Throw? SecondThrow { get; internal set; }
    public Throw? ThirdThrow { get; internal set; }
}