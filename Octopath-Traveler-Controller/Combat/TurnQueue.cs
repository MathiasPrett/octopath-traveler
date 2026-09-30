using Octopath_Traveler.Models;

namespace Octopath_Traveler.Combat;

public class TurnQueue
{
    private readonly List<Unit> _participants;
    private readonly List<Unit> _alreadyPlayed = new List<Unit>();

    public TurnQueue(ValidatedTeam team)
    {
        _participants = team.LivingUnits.Where(unit => unit.CanActThisRound).ToList();
    }

    public static List<Unit> GetNextRoundOrder(ValidatedTeam team)
        => SortByPriority(team.LivingUnits.Where(unit => unit.CanActNextRound),
            unit => unit.PriorityNextRound);

    public bool HasPendingUnits()
        => PendingUnits.Count > 0;

    public Unit NextUnit
        => PendingUnits.First();

    public void MarkPlayed(Unit unit)
        => _alreadyPlayed.Add(unit);

    public List<Unit> PendingUnits
        => SortByPriority(_participants.Where(IsPending), unit => unit.PriorityThisRound);

    private bool IsPending(Unit unit)
        => !_alreadyPlayed.Contains(unit) && unit.CanActThisRound;

    private static List<Unit> SortByPriority(IEnumerable<Unit> units, Func<Unit, TurnPriority> priority)
        => units.OrderBy(priority).ThenByDescending(unit => unit.Speed).ToList();
}
