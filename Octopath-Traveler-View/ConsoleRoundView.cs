using Octopath_Traveler.Models;

namespace Octopath_Traveler_View;

public class ConsoleRoundView : ConsoleCombatView, IRoundView
{
    private const string RoundStartHeader = "INICIA RONDA";
    private const string PlayerTeamHeader = "Equipo del jugador";
    private const string EnemyTeamHeader = "Equipo del enemigo";
    private const string CurrentRoundQueueHeader = "Turnos de la ronda";
    private const string NextRoundQueueHeader = "Turnos de la siguiente ronda";
    private const string PlayerVictoryMessage = "Gana equipo del jugador";
    private const string EnemyVictoryMessage = "Gana equipo del enemigo";
    private const char FirstPositionLetter = 'A';
    private const string QueueSeparator = ".";

    public ConsoleRoundView(View view) : base(view) { }

    public void AnnounceRoundStart(int round)
        => ShowBlock($"{RoundStartHeader} {round}");

    public void ShowTeamsState(ValidatedTeam team)
    {
        ShowSeparator();
        ShowTeamState(PlayerTeamHeader, DescribeAll(team.Travelers));
        ShowTeamState(EnemyTeamHeader, DescribeAll(team.Beasts));
    }

    public void ShowTurnQueues(List<Unit> currentRound, List<Unit> nextRound)
    {
        ShowQueue(CurrentRoundQueueHeader, currentRound);
        ShowQueue(NextRoundQueueHeader, nextRound);
    }

    public void AnnouncePlayerVictory()
        => ShowBlock(PlayerVictoryMessage);

    public void AnnounceEnemyVictory()
        => ShowBlock(EnemyVictoryMessage);

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
