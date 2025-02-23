using DartsScoreLib;
using System.Text;

namespace DartsScore;

public partial class MainPage : ContentPage
{
    private readonly DartsGame game;

    public MainPage()
    {
        InitializeComponent();
        game = new DartsGame();
    }

    private void SetCalculateWinningRoundButtonState(object sender, EventArgs e)
    {
        if (RemainingScoreInput.Text.Length > 0 && ushort.TryParse(RemainingScoreInput.Text, out ushort score))
        {
            CalculateWinningRoundButton.IsEnabled = score < 502 && score > 0;
        }
        else
        {
            CalculateWinningRoundButton.IsEnabled = false;
        }
    }

    private void CalculateWinningRound(object sender, EventArgs e)
    {
        ushort remainingScore = ushort.Parse(RemainingScoreInput.Text);
        WinningRound round = game.CalculateWinningRound(remainingScore);
        if (round.CanWin)
        {
            StringBuilder answerBuilder = new(ToString(round.FirstThrow!));
            if (round.SecondThrow != null)
            {
                answerBuilder.Append(' ');
                answerBuilder.Append(ToString(round.SecondThrow));
                if (round.ThirdThrow != null)
                {
                    answerBuilder.Append(' ');
                    answerBuilder.Append(ToString(round.ThirdThrow));
                }
            }
            Answer.Text = answerBuilder.ToString();
        }
        else
        {
            Answer.Text = "No";
        }
    }

    private static string ToString(Throw @throw)
    {
        if (@throw.Multiplier == Multiplier.Double)
        {
            return $"D{@throw.Score}";
        }
        if (@throw.Multiplier == Multiplier.Triple)
        {
            return $"T{@throw.Score}";
        }
        return @throw.Score.ToString();
    }
}
