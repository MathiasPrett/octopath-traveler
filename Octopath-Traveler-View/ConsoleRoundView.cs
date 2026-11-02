using Octopath_Traveler.Models;

namespace Octopath_Traveler_View;

public class ConsoleRoundView : ConsoleCombatView, IRoundView
{
    private const string RoundStartHeader = "INICIA RONDA";
    private const string TravelerTeamHeader = "Equipo del jugador";
    private const string BeastTeamHeader = "Equipo del enemigo";
    private const string CurrentRoundQueueHeader = "Turnos de la ronda";
    private const string NextRoundQueueHeader = "Turnos de la siguiente ronda";
    private const string TravelerVictoryMessage = "Gana equipo del jugador";
    private const string BeastVictoryMessage = "Gana equipo del enemigo";
    private const char FirstPositionLetter = 'A';
    private const string QueueSeparator = ".";

    public ConsoleRoundView(View view) : base(view) { }

    public void AnnounceRoundStart(int round)
        => ShowBlock($"{RoundStartHeader} {round}");

    public void ShowTeamsState(ValidatedTeam team)
    {
        ShowSeparator();
        ShowTeamState(TravelerTeamHeader, DescribeAll(team.Travelers));
        ShowTeamState(BeastTeamHeader, DescribeAll(team.Beasts));
    }

    public void ShowTurnQueues(List<Unit> currentRound, List<Unit> nextRound)
    {
        ShowQueue(CurrentRoundQueueHeader, currentRound);
        ShowQueue(NextRoundQueueHeader, nextRound);
    }

    public void AnnounceTravelerVictory()
        => ShowBlock(TravelerVictoryMessage);

    public void AnnounceBeastVictory()
        => ShowBlock(BeastVictoryMessage);

    private void ShowTeamState(string header, List<string> descriptions)
    {
        WriteLine(header);
        for (int index = 0; index < descriptions.Count; index++)
            WriteLine($"{GetPositionLetter(index)}-{descriptions[index]}");
    }

    private void ShowQueue(string header, List<Unit> units)
        => ShowNumberedList(header, GetNames(units), QueueSeparator);

    private static List<string> GetNames(List<Unit> units)
        => units.Select(unit => unit.Name).ToList();

    private static char GetPositionLetter(int index)
        => (char)(FirstPositionLetter + index);
}
