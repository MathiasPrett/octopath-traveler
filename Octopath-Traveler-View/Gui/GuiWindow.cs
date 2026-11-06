using Octopath_Traveler.Models;
using OctopathTravelerGUI;
using OctopathTravelerGUI.Models.Enums;
using OctopathTravelerGUI.Models.Interfaces;

namespace Octopath_Traveler_View.Gui;

public class GuiWindow
{
    private readonly OTGUI _gui = new OTGUI();

    private List<ITraveler> _travelers = new List<ITraveler>();
    private List<IBeast> _beasts = new List<IBeast>();
    private List<string> _currentRoundTurns = new List<string>();
    private List<string> _nextRoundTurns = new List<string>();

    public bool HasFled { get; private set; }

    public IEnumerable<string> TravelerNames => _travelers.Select(traveler => traveler.Name);
    public IEnumerable<string> BeastNames => _beasts.Select(beast => beast.Name);

    public void Start(Action program)
        => _gui.Start(program);

    public string[] GetTravelersTeam()
        => _gui.GetTravelersTeam();

    public string[] GetBeastsTeam()
        => _gui.GetBeastsTeam();

    public void ShowInvalidTeams()
        => _gui.ShowInvalidTeams();

    public void RememberTeam(ValidatedTeam team)
    {
        _travelers = team.Travelers.Select(traveler => (ITraveler)new TravelerGUI(traveler)).ToList();
        _beasts = team.Beasts.Select(beast => (IBeast)new BeastGUI(beast)).ToList();
    }

    public void RememberTurns(List<Unit> currentRound, List<Unit> nextRound)
    {
        _currentRoundTurns = GetNames(currentRound);
        _nextRoundTurns = GetNames(nextRound);
    }

    public string AskForOption(Option option, IReadOnlyList<string> options)
    {
        _gui.Update(new StateGUI(_travelers, _beasts, options, option, _currentRoundTurns, _nextRoundTurns));
        return WaitForClickOn(options);
    }

    public void MarkFled()
        => HasFled = true;

    public void ShowWinner(WinnerOption winner, IEnumerable<string> team)
        => _gui.ShowWinner(new WinnerGUI(winner, team));

    private string WaitForClickOn(IReadOnlyList<string> options)
    {
        IClickedElement clicked;
        do
        {
            clicked = _gui.GetClickedElement();
        } while (!options.Contains(clicked.Text));
        return clicked.Text;
    }

    private static List<string> GetNames(List<Unit> units)
        => units.Select(unit => unit.Name).ToList();
}
