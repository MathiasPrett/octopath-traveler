using OctopathTravelerGUI.Models.Enums;
using OctopathTravelerGUI.Models.Interfaces;

namespace Octopath_Traveler_View.Gui;

public class WinnerGUI(WinnerOption winnerOption, IEnumerable<string> team) : IWinner
{
    public WinnerOption WinnerOption { get; } = winnerOption;
    public IEnumerable<string> Team { get; } = team;
}
