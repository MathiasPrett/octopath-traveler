namespace Octopath_Traveler.Models;

public class LegholdTrap
{
    private const int NoRounds = 0;
    private const int LastRound = 1;

    private int _remainingRounds = NoRounds;

    public void Add(int rounds)
        => _remainingRounds += rounds;

    public void AdvanceRound()
        => _remainingRounds = Math.Max(NoRounds, _remainingRounds - 1);

    public TurnPriority AdjustThisRound(TurnPriority priority)
        => Adjust(priority, _remainingRounds > NoRounds);

    public TurnPriority AdjustNextRound(TurnPriority priority)
        => Adjust(priority, _remainingRounds > LastRound);

    private static TurnPriority Adjust(TurnPriority priority, bool isActive)
        => isActive && priority != TurnPriority.RecoveringFromBreak ? TurnPriority.Trapped : priority;
}
