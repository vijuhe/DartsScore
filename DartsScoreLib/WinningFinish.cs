namespace DartsScoreLib;

public record WinningFinish
{
    public bool IsPossible { get; internal set; }
    public Throw? FirstThrow { get; internal set; }
    public Throw? SecondThrow { get; internal set; }
    public Throw? ThirdThrow { get; internal set; }
}