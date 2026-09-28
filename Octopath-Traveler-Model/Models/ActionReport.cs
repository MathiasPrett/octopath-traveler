namespace Octopath_Traveler.Models;

// Todo lo que provocó una acción, en el orden en que debe anunciarse.
public class ActionReport
{
    private readonly List<CombatEvent> _events = new List<CombatEvent>();

    public ActionReport(Unit actor)
    {
        Actor = actor;
    }

    public Unit Actor { get; }

    public IReadOnlyList<CombatEvent> Events => _events;

    public IReadOnlyList<Unit> UnitsWithHpChanges
        => _events.Where(combatEvent => combatEvent.ChangedHp)
            .Select(combatEvent => combatEvent.Target).Distinct().ToList();

    public void Record(CombatEvent combatEvent)
        => _events.Add(combatEvent);
}
