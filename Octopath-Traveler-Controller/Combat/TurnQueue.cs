using Octopath_Traveler.Models;

namespace Octopath_Traveler.Combat;

public class TurnQueue
{
    // Quiénes juegan la ronda se fija al empezarla: una unidad revivida a mitad de ronda
    // no alcanza a jugarla. El orden, en cambio, se recalcula en cada turno.
    private readonly List<Unit> _participants;
    private readonly List<Unit> _alreadyPlayed = new List<Unit>();

    public TurnQueue(ValidatedTeam team)
    {
        _participants = team.LivingUnits().Where(unit => unit.CanActThisRound).ToList();
    }

    public static List<Unit> NextRoundOrder(ValidatedTeam team)
        => ByPriority(team.LivingUnits().Where(unit => unit.CanActNextRound),
            unit => unit.PriorityNextRound);

    public bool HasPendingUnits()
        => PendingUnits().Count > 0;

    public Unit NextUnit()
        => PendingUnits().First();

    public void MarkPlayed(Unit unit)
        => _alreadyPlayed.Add(unit);

    public List<Unit> PendingUnits()
        => ByPriority(_participants.Where(IsPending), unit => unit.PriorityThisRound);

    private bool IsPending(Unit unit)
        => !_alreadyPlayed.Contains(unit) && unit.CanActThisRound;

    // OrderBy de LINQ es estable y LivingUnits() ya viene en orden de tablero, con los
    // viajeros antes que las bestias, así que los empates quedan resueltos solos.
    private static List<Unit> ByPriority(IEnumerable<Unit> units, Func<Unit, TurnPriority> priority)
        => units.OrderBy(priority).ThenByDescending(unit => unit.Speed).ToList();
}
