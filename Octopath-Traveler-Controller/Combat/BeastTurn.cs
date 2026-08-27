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
        => _renderer.ShowBeastAttack(beast.Attack(ChooseTarget()), beast.Skill);

    private Traveler ChooseTarget()
        => _team.LivingTravelers()
            .OrderByDescending(traveler => traveler.Stats.HpCurrent).First();
}
