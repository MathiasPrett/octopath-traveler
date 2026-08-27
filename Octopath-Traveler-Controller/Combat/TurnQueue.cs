using Octopath_Traveler.Models;

namespace Octopath_Traveler.Combat;

public class TurnQueue
{
    private readonly List<Unit> _order;
    private int _cursor;

    public TurnQueue(ValidatedTeam team)
    {
        _order = Order(team);
        _cursor = 0;
    }

    public static List<Unit> Order(ValidatedTeam team)
        => team.LivingUnits().OrderByDescending(unit => unit.Stats.Speed).ToList();

    public bool HasPendingUnits()
        => FirstPendingIndex() < _order.Count;

    public Unit StartCurrentTurn()
    {
        _cursor = FirstPendingIndex();
        return _order[_cursor];
    }

    public void FinishCurrentTurn()
        => _cursor++;

    public List<Unit> PendingUnits()
        => _order.Skip(FirstPendingIndex()).Where(unit => unit.Alive).ToList();

    private int FirstPendingIndex()
    {
        for (int index = _cursor; index < _order.Count; index++)
            if (_order[index].Alive)
                return index;
        return _order.Count;
    }
}
