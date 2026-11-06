using OctopathTravelerGUI.Models.Enums;
using OctopathTravelerGUI.Models.Interfaces;

namespace Octopath_Traveler_View.Gui;

public class StateGUI(
    IEnumerable<ITraveler> travelers, IEnumerable<IBeast> beasts,
    IEnumerable<string> options, Option option,
    IEnumerable<string> currentRoundTurns, IEnumerable<string> nextRoundTurns)
    : IState
{
    public IEnumerable<ITraveler> Travelers { get; } = travelers;
    public IEnumerable<IBeast> Beasts { get; } = beasts;
    public IEnumerable<string> Options { get; } = options;
    public Option Option { get; } = option;
    public IEnumerable<string> CurrentRoundTurns { get; } = currentRoundTurns;
    public IEnumerable<string> NextRoundTurns { get; } = nextRoundTurns;
}
