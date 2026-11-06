using Octopath_Traveler.Data;

namespace Octopath_Traveler_View.Gui;

public class GuiTeamView : ITeamView
{
    private readonly GuiWindow _window;

    public GuiTeamView(GuiWindow window)
    {
        _window = window;
    }

    public string[] AskForTeamFileLines()
    {
        IEnumerable<string> travelerSection = _window.GetTravelersTeam().Prepend(TeamParser.TravelerTeamHeader);
        IEnumerable<string> beastSection = _window.GetBeastsTeam().Prepend(TeamParser.BeastTeamHeader);
        return travelerSection.Concat(beastSection).ToArray();
    }

    public void AnnounceInvalidTeamFile()
        => _window.ShowInvalidTeams();
}
