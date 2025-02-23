using DartsScoreLib;

namespace DartsScoreTests;

public class WinningRoundTests
{
    private static DartsGame game;
    private WinningRound? result;

    [OneTimeSetUp]
    public static void Setup()
    {
        game = new DartsGame();
    }

    [Test]
    public void ScoreNeedsToBeLowEnoughToWin()
    {
        result = game.CalculateWinningRound(400);
        AssertThatCannotWin();
    }

    [Test]
    public void CannotWinWithScoreOne()
    {
        result = game.CalculateWinningRound(1);
        AssertThatCannotWin();
    }

    [TestCase(947)]
    [TestCase(0)]
    public void RemainingScoreMustBeWithinGameLimits(int remainingScore)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => game.CalculateWinningRound((ushort)remainingScore));
    }

    [TestCase(2, 1)]
    [TestCase(18, 9)]
    [TestCase(40, 20)]
    [TestCase(50, 25)]
    public void WinWithOneThrow(int remainingScore, int doubleThrow)
    {
        result = game.CalculateWinningRound((ushort)remainingScore);

        Assert.Multiple(() =>
        {
            Assert.That(result.CanWin, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(doubleThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Double));
            Assert.That(result.SecondThrow, Is.Null);
            Assert.That(result.ThirdThrow, Is.Null);
        });
    }

    [TestCase(9, 7, 1)]
    [TestCase(51, 19, 16)]
    [TestCase(70, 20, 25)]
    public void WinWithTwoThrowsHittingSingleFirst(int remainingScore, int singleFirstThrow, int doubleSecondThrow)
    {
        result = game.CalculateWinningRound((ushort)remainingScore);

        Assert.Multiple(() =>
        {
            Assert.That(result.CanWin, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(singleFirstThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Single));
            Assert.That(result.SecondThrow!.Score, Is.EqualTo(doubleSecondThrow));
            Assert.That(result.SecondThrow.Multiplier, Is.EqualTo(Multiplier.Double));
            Assert.That(result.ThirdThrow, Is.Null);
        });
    }

    [TestCase(72, 20, 16)]
    [TestCase(80, 20, 20)]
    [TestCase(90, 20, 25)]
    public void WinWithTwoThrowsHittingDoubleFirst(int remainingScore, int doubleFirstThrow, int doubleSecondThrow)
    {
        result = game.CalculateWinningRound((ushort)remainingScore);

        Assert.Multiple(() =>
        {
            Assert.That(result.CanWin, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(doubleFirstThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Double));
            Assert.That(result.SecondThrow!.Score, Is.EqualTo(doubleSecondThrow));
            Assert.That(result.SecondThrow.Multiplier, Is.EqualTo(Multiplier.Double));
            Assert.That(result.ThirdThrow, Is.Null);
        });
    }

    [TestCase(92, 20, 16)]
    [TestCase(100, 20, 20)]
    [TestCase(110, 20, 25)]
    public void WinWithTwoThrowsHittingTripleFirst(int remainingScore, int tripleFirstThrow, int doubleSecondThrow)
    {
        result = game.CalculateWinningRound((ushort)remainingScore);

        Assert.Multiple(() =>
        {
            Assert.That(result.CanWin, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(tripleFirstThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Triple));
            Assert.That(result.SecondThrow!.Score, Is.EqualTo(doubleSecondThrow));
            Assert.That(result.SecondThrow.Multiplier, Is.EqualTo(Multiplier.Double));
            Assert.That(result.ThirdThrow, Is.Null);
        });
    }

    [TestCase(120, 20, 20, 20)]
    [TestCase(130, 20, 20, 25)]
    public void WinWithThreeThrowsHittingSingleFirst(int remainingScore, int singleFirstThrow, int tripleSecondThrow, int doubleThirdThrow)
    {
        result = game.CalculateWinningRound((ushort)remainingScore);

        Assert.Multiple(() =>
        {
            Assert.That(result.CanWin, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(singleFirstThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Single));
            Assert.That(result.SecondThrow!.Score, Is.EqualTo(tripleSecondThrow));
            Assert.That(result.SecondThrow.Multiplier, Is.EqualTo(Multiplier.Triple));
            Assert.That(result.ThirdThrow!.Score, Is.EqualTo(doubleThirdThrow));
            Assert.That(result.ThirdThrow.Multiplier, Is.EqualTo(Multiplier.Double));
        });
    }

    [TestCase(140, 20, 20, 20)]
    [TestCase(150, 20, 20, 25)]
    public void WinWithThreeThrowsHittingDoubleFirst(int remainingScore, int doubleFirstThrow, int tripleSecondThrow, int doubleThirdThrow)
    {
        result = game.CalculateWinningRound((ushort)remainingScore);

        Assert.Multiple(() =>
        {
            Assert.That(result.CanWin, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(doubleFirstThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Double));
            Assert.That(result.SecondThrow!.Score, Is.EqualTo(tripleSecondThrow));
            Assert.That(result.SecondThrow.Multiplier, Is.EqualTo(Multiplier.Triple));
            Assert.That(result.ThirdThrow!.Score, Is.EqualTo(doubleThirdThrow));
            Assert.That(result.ThirdThrow.Multiplier, Is.EqualTo(Multiplier.Double));
        });
    }

    [TestCase(160, 20, 20, 20)]
    [TestCase(170, 20, 20, 25)]
    public void WinWithThreeThrowsHittingTriplesFirst(int remainingScore, int tripleFirstThrow, int tripleSecondThrow, int doubleThirdThrow)
    {
        result = game.CalculateWinningRound((ushort)remainingScore);

        Assert.Multiple(() =>
        {
            Assert.That(result.CanWin, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(tripleFirstThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Triple));
            Assert.That(result.SecondThrow!.Score, Is.EqualTo(tripleSecondThrow));
            Assert.That(result.SecondThrow.Multiplier, Is.EqualTo(Multiplier.Triple));
            Assert.That(result.ThirdThrow!.Score, Is.EqualTo(doubleThirdThrow));
            Assert.That(result.ThirdThrow.Multiplier, Is.EqualTo(Multiplier.Double));
        });
    }

    private void AssertThatCannotWin()
    {
        Assert.Multiple(() =>
        {
            Assert.That(result!.CanWin, Is.False);
            Assert.That(result.FirstThrow, Is.Null);
            Assert.That(result.SecondThrow, Is.Null);
            Assert.That(result.ThirdThrow, Is.Null);
        });
    }
}
