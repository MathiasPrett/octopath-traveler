using Octopath_Traveler.Models;

namespace Octopath_Traveler_View;

public interface IRoundView
{
    void AnnounceRoundStart(int round);
    void ShowTeamsState(ValidatedTeam team);
    void ShowTurnQueues(List<Unit> currentRound, List<Unit> nextRound);
    void AnnouncePlayerVictory();
    void AnnounceEnemyVictory();
}
