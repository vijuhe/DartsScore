using DartsScoreLib;

namespace DartsScoreTests;

public class WinningFinishTests
{
    private static DartsGame game;
    private WinningFinish? result;

    [OneTimeSetUp]
    public static void Setup()
    {
        game = new DartsGame();
    }

    [Test]
    public void ScoreNeedsToBeLowEnoughToWin()
    {
        result = game.CalculateWinningFinish(400, 3);
        AssertThatCannotWin();
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void CannotWinWithScoreOne(int throwsRemaining)
    {
        result = game.CalculateWinningFinish(1, (byte)throwsRemaining);
        AssertThatCannotWin();
    }

    [TestCase(947)]
    [TestCase(0)]
    public void RemainingScoreMustBeWithinGameLimits(int remainingScore)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => game.CalculateWinningFinish((ushort)remainingScore, 3));
    }

    [TestCase(4)]
    [TestCase(0)]
    public void RemainingThrowsMustBeWithinGameLimits(int remainingThrows)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => game.CalculateWinningFinish(21, (byte)remainingThrows));
    }

    [TestCase(2, 1, 1)]
    [TestCase(18, 2, 9)]
    [TestCase(40, 3, 20)]
    [TestCase(50, 1, 25)]
    public void WinWithOneThrow(int remainingScore, int throwsRemaining, int doubleThrow)
    {
        result = game.CalculateWinningFinish((ushort)remainingScore, (byte)throwsRemaining);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsPossible, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(doubleThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Double));
            Assert.That(result.SecondThrow, Is.Null);
            Assert.That(result.ThirdThrow, Is.Null);
        });
    }

    [TestCase(9, 2, 7, 1)]
    [TestCase(51, 3, 19, 16)]
    [TestCase(70, 2, 20, 25)]
    public void WinWithTwoThrowsHittingSingleFirst(int remainingScore, int throwsRemaining, int singleFirstThrow, int doubleSecondThrow)
    {
        result = game.CalculateWinningFinish((ushort)remainingScore, (byte)throwsRemaining);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsPossible, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(singleFirstThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Single));
            Assert.That(result.SecondThrow!.Score, Is.EqualTo(doubleSecondThrow));
            Assert.That(result.SecondThrow.Multiplier, Is.EqualTo(Multiplier.Double));
            Assert.That(result.ThirdThrow, Is.Null);
        });
    }

    [TestCase(72, 2, 20, 16)]
    [TestCase(80, 3, 20, 20)]
    [TestCase(90, 3, 20, 25)]
    public void WinWithTwoThrowsHittingDoubleFirst(int remainingScore, int remainingThrows, int doubleFirstThrow, int doubleSecondThrow)
    {
        result = game.CalculateWinningFinish((ushort)remainingScore, (byte)remainingThrows);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsPossible, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(doubleFirstThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Double));
            Assert.That(result.SecondThrow!.Score, Is.EqualTo(doubleSecondThrow));
            Assert.That(result.SecondThrow.Multiplier, Is.EqualTo(Multiplier.Double));
            Assert.That(result.ThirdThrow, Is.Null);
        });
    }

    [TestCase(92, 3, 20, 16)]
    [TestCase(100, 2, 20, 20)]
    [TestCase(110, 2, 20, 25)]
    public void WinWithTwoThrowsHittingTripleFirst(int remainingScore, int remainingThrows, int tripleFirstThrow, int doubleSecondThrow)
    {
        result = game.CalculateWinningFinish((ushort)remainingScore, (byte)remainingThrows);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsPossible, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(tripleFirstThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Triple));
            Assert.That(result.SecondThrow!.Score, Is.EqualTo(doubleSecondThrow));
            Assert.That(result.SecondThrow.Multiplier, Is.EqualTo(Multiplier.Double));
            Assert.That(result.ThirdThrow, Is.Null);
        });
    }

    [Test]
    public void CannotWinWithTwoThrowsIfOnlyOneThrowLeft()
    {
        result = game.CalculateWinningFinish(92, 1);
        AssertThatCannotWin();
    }

    [TestCase(120, 20, 20, 20)]
    [TestCase(130, 20, 20, 25)]
    public void WinWithThreeThrowsHittingSingleFirst(int remainingScore, int singleFirstThrow, int tripleSecondThrow, int doubleThirdThrow)
    {
        result = game.CalculateWinningFinish((ushort)remainingScore, 3);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsPossible, Is.True);
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
        result = game.CalculateWinningFinish((ushort)remainingScore, 3);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsPossible, Is.True);
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
        result = game.CalculateWinningFinish((ushort)remainingScore, 3);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsPossible, Is.True);
            Assert.That(result.FirstThrow!.Score, Is.EqualTo(tripleFirstThrow));
            Assert.That(result.FirstThrow.Multiplier, Is.EqualTo(Multiplier.Triple));
            Assert.That(result.SecondThrow!.Score, Is.EqualTo(tripleSecondThrow));
            Assert.That(result.SecondThrow.Multiplier, Is.EqualTo(Multiplier.Triple));
            Assert.That(result.ThirdThrow!.Score, Is.EqualTo(doubleThirdThrow));
            Assert.That(result.ThirdThrow.Multiplier, Is.EqualTo(Multiplier.Double));
        });
    }

    [TestCase(1)]
    [TestCase(2)]
    public void CannotWinWithThreeThrowsIfNotEnoughThrowsLeft(int remainingThrows)
    {
        result = game.CalculateWinningFinish(160, (byte)remainingThrows);
        AssertThatCannotWin();
    }

    private void AssertThatCannotWin()
    {
        Assert.Multiple(() =>
        {
            Assert.That(result!.IsPossible, Is.False);
            Assert.That(result.FirstThrow, Is.Null);
            Assert.That(result.SecondThrow, Is.Null);
            Assert.That(result.ThirdThrow, Is.Null);
        });
    }
}
