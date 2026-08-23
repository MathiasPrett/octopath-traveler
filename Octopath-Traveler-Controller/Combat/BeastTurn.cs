using Octopath_Traveler.Models;

namespace Octopath_Traveler.Combat;

public class BeastTurn
{
    private readonly CombatRenderer _renderer;
    private readonly ValidatedTeam _team;

    public BeastTurn(CombatRenderer renderer, ValidatedTeam team)
    {
        _renderer = renderer;
        _team = team;
    }

    public void Play(Beast beast)
    {
        Traveler target = ChooseTarget();
        int damage = DamageCalculator.Calculate(beast, target);
        target.ReceiveDamage(damage);
        _renderer.ShowBeastAttack(new AttackOutcome(beast, target, damage), beast.Skill);
    }

    private Traveler ChooseTarget()
        => _team.Travelers.Where(traveler => traveler.Alive)
            .OrderByDescending(traveler => traveler.Stats.HpCurrent).First();
}
