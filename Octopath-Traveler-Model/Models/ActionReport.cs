namespace Octopath_Traveler.Models;

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

    public int TotalDamageTo(Unit target)
    {
        IEnumerable<DamageEvent> damages = _events.OfType<DamageEvent>();
        return damages.Where(damage => damage.Target == target).Sum(damage => damage.Damage);
    }

    public void Record(CombatEvent combatEvent)
        => _events.Add(combatEvent);
}
