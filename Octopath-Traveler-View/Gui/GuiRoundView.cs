using Octopath_Traveler.Models;
using OctopathTravelerGUI.Models.Enums;

namespace Octopath_Traveler_View.Gui;

public class GuiRoundView : IRoundView
{
    private readonly GuiWindow _window;

    public GuiRoundView(GuiWindow window)
    {
        _window = window;
    }

    public void AnnounceRoundStart(int round)
    {
        // La ventana no tiene un aviso de ronda: el tablero se redibuja en cada turno.
    }

    public void ShowTeamsState(ValidatedTeam team)
        => _window.RememberTeam(team);

    public void ShowTurnQueues(List<Unit> currentRound, List<Unit> nextRound)
        => _window.RememberTurns(currentRound, nextRound);

    public void AnnounceTravelerVictory()
        => _window.ShowWinner(WinnerOption.Travelers, _window.TravelerNames);

    public void AnnounceBeastVictory()
    {
        if (_window.HasFled)
            _window.ShowWinner(WinnerOption.RunAway, _window.TravelerNames);
        else
            _window.ShowWinner(WinnerOption.Beasts, _window.BeastNames);
    }
}
