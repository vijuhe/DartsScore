namespace DartsScoreLib;

public class DartsGame
{
    private static readonly byte[] descendingPointsWithTriplesAndDoubles = [20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1];
    private static readonly byte[] descendingPointsWithDoublesOnly = [25];
    private readonly byte[] descendingPointsWithDoubles;

    public DartsGame()
    {
        descendingPointsWithDoubles = [..descendingPointsWithDoublesOnly.Concat(descendingPointsWithTriplesAndDoubles).OrderDescending()];
    }
    
    public WinningFinish CalculateWinningFinish(ushort remainingScore, byte remainingThrows)
    {
        if (remainingScore > 501 || remainingScore == 0)
        {
            throw new ArgumentOutOfRangeException($"Darts game cannot have a remaining score of {remainingScore}.");
        }
        if (remainingThrows > 3 || remainingThrows == 0)
        {
            throw new ArgumentOutOfRangeException($"Darts game round cannot have {remainingThrows} throws left.");
        }

        if (TooHighToWin(remainingScore) || remainingScore < 2)
        {
            return new WinningFinish
            {
                IsPossible = false
            };
        }

        if (CanWinWithOneThrow(remainingScore))
        {
            return new WinningFinish
            {
                IsPossible = true,
                FirstThrow = new Throw
                {
                    Score = (byte)(remainingScore / 2),
                    Multiplier = Multiplier.Double
                }
            };
        }

        if (remainingThrows == 1)
        {
            return new WinningFinish
            {
                IsPossible = false
            };
        }

        foreach (byte point in descendingPointsWithTriplesAndDoubles)
        {
            if (CanWinWithOneThrow((ushort)(remainingScore - point)))
            {
                return new WinningFinish
                {
                    IsPossible = true,
                    FirstThrow = new Throw
                    {
                        Score = point
                    },
                    SecondThrow = new Throw
                    {
                        Score = (byte)((remainingScore - point) / 2),
                        Multiplier = Multiplier.Double
                    }
                };
            }
        }

        foreach (byte point in descendingPointsWithTriplesAndDoubles)
        {
            byte @double = (byte)(point * 2);
            if (CanWinWithOneThrow((ushort)(remainingScore - @double)))
            {
                return new WinningFinish
                {
                    IsPossible = true,
                    FirstThrow = new Throw
                    {
                        Score = point,
                        Multiplier = Multiplier.Double
                    },
                    SecondThrow = new Throw
                    {
                        Score = (byte)((remainingScore - @double) / 2),
                        Multiplier = Multiplier.Double
                    }
                };
            }
        }

        foreach (byte point in descendingPointsWithTriplesAndDoubles)
        {
            byte triple = (byte)(point * 3);
            if (CanWinWithOneThrow((ushort)(remainingScore - triple)))
            {
                return new WinningFinish
                {
                    IsPossible = true,
                    FirstThrow = new Throw
                    {
                        Score = point,
                        Multiplier = Multiplier.Triple
                    },
                    SecondThrow = new Throw
                    {
                        Score = (byte)((remainingScore - triple) / 2),
                        Multiplier = Multiplier.Double
                    }
                };
            }
        }

        if (remainingThrows == 2)
        {
            return new WinningFinish
            {
                IsPossible = false
            };
        }

        foreach (byte point in descendingPointsWithTriplesAndDoubles)
        {
            foreach (byte secondPoint in descendingPointsWithTriplesAndDoubles)
            {
                byte tripleSecond = (byte)(secondPoint * 3);
                if (CanWinWithOneThrow((ushort)(remainingScore - point - tripleSecond)))
                {
                    return new WinningFinish
                    {
                        IsPossible = true,
                        FirstThrow = new Throw
                        {
                            Score = point
                        },
                        SecondThrow = new Throw
                        {
                            Score = secondPoint,
                            Multiplier = Multiplier.Triple
                        },
                        ThirdThrow = new Throw
                        {
                            Score = (byte)((remainingScore - point - tripleSecond) / 2),
                            Multiplier = Multiplier.Double
                        }
                    };
                }
            }
        }

        foreach (byte point in descendingPointsWithTriplesAndDoubles)
        {
            byte doubleFirst = (byte)(point * 2);
            foreach (byte secondPoint in descendingPointsWithTriplesAndDoubles)
            {
                byte tripleSecond = (byte)(secondPoint * 3);
                if (CanWinWithOneThrow((ushort)(remainingScore - doubleFirst - tripleSecond)))
                {
                    return new WinningFinish
                    {
                        IsPossible = true,
                        FirstThrow = new Throw
                        {
                            Score = point,
                            Multiplier = Multiplier.Double
                        },
                        SecondThrow = new Throw
                        {
                            Score = secondPoint,
                            Multiplier = Multiplier.Triple
                        },
                        ThirdThrow = new Throw
                        {
                            Score = (byte)((remainingScore - doubleFirst - tripleSecond) / 2),
                            Multiplier = Multiplier.Double
                        }
                    };
                }
            }

        }

        foreach (byte point in descendingPointsWithTriplesAndDoubles)
        {
            byte tripleFirst = (byte)(point * 3);
            foreach (byte secondPoint in descendingPointsWithTriplesAndDoubles)
            {
                byte tripleSecond = (byte)(secondPoint * 3);
                if (CanWinWithOneThrow((ushort)(remainingScore - tripleFirst - tripleSecond)))
                {
                    return new WinningFinish
                    {
                        IsPossible = true,
                        FirstThrow = new Throw
                        {
                            Score = point,
                            Multiplier = Multiplier.Triple
                        },
                        SecondThrow = new Throw
                        {
                            Score = secondPoint,
                            Multiplier = Multiplier.Triple
                        },
                        ThirdThrow = new Throw
                        {
                            Score = (byte)((remainingScore - tripleFirst - tripleSecond) / 2),
                            Multiplier = Multiplier.Double
                        }
                    };
                }
            }
        }

        return new WinningFinish
        {
            IsPossible = false
        };
    }

    private bool CanWinWithOneThrow(ushort remainingScore)
    {
        return remainingScore % 2 == 0 && descendingPointsWithDoubles.Any(p => p == remainingScore / 2);
    }

    private bool TooHighToWin(ushort remainingScore)
    {
        return remainingScore >
            3 * descendingPointsWithTriplesAndDoubles[0] +
            3 * descendingPointsWithTriplesAndDoubles[0] +
            2 * descendingPointsWithDoubles[0];
    }
}
