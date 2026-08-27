using Octopath_Traveler.Models;
using Octopath_Traveler.Utils;
using Octopath_Traveler_View;

namespace Octopath_Traveler.Combat;

public class TravelerTurn
{
    private const int BasicAttackOption = 1;
    private const int SkillOption = 2;
    private const int FleeOption = 4;

    private readonly CombatRenderer _renderer;
    private readonly ValidatedTeam _team;
    private readonly OptionReader _optionReader;

    public TravelerTurn(View view, CombatRenderer renderer, ValidatedTeam team)
    {
        _renderer = renderer;
        _team = team;
        _optionReader = new OptionReader(view);
    }

    public TurnResult Play(Traveler traveler)
    {
        TurnResult result = TurnResult.Cancelled;
        while (result == TurnResult.Cancelled)
            result = ChooseAndExecuteAction(traveler);
        return result;
    }

    private TurnResult ChooseAndExecuteAction(Traveler traveler)
    {
        _renderer.ShowActionMenu(traveler);
        return ExecuteAction(traveler, _optionReader.Read());
    }

    private TurnResult ExecuteAction(Traveler traveler, int option)
    {
        if (option == BasicAttackOption) return TryBasicAttack(traveler);
        if (option == SkillOption) return BrowseSkills(traveler);
        if (option == FleeOption) return Flee();
        return TurnResult.Completed;
    }

    private TurnResult TryBasicAttack(Traveler traveler)
    {
        string? weapon = ChooseWeapon(traveler);
        if (weapon == null) return TurnResult.Cancelled;
        Beast? target = ChooseTarget(traveler);
        if (target == null) return TurnResult.Cancelled;
        AskBoostPoints();
        Attack(traveler, target, weapon);
        return TurnResult.Completed;
    }

    private string? ChooseWeapon(Traveler traveler)
    {
        _renderer.ShowWeaponMenu(traveler);
        int option = _optionReader.Read();
        return IsCancel(option, traveler.Weapons.Count) ? null : traveler.Weapons[option - 1];
    }

    private Beast? ChooseTarget(Traveler traveler)
    {
        List<Beast> targets = LivingBeasts();
        _renderer.ShowTargetMenu(traveler, targets);
        int option = _optionReader.Read();
        return IsCancel(option, targets.Count) ? null : targets[option - 1];
    }

    private void AskBoostPoints()
    {
        _renderer.ShowBoostPointPrompt();
        _optionReader.Read();
    }

    private void Attack(Traveler attacker, Beast target, string weaponName)
    {
        int damage = DamageCalculator.Calculate(attacker, target);
        target.ReceiveDamage(damage);
        _renderer.ShowTravelerAttack(new AttackOutcome(attacker, target, damage), weaponName);
    }

    private TurnResult BrowseSkills(Traveler traveler)
    {
        _renderer.ShowSkillMenu(traveler);
        _optionReader.Read();
        return TurnResult.Cancelled;
    }

    private TurnResult Flee()
    {
        _renderer.ShowFlee();
        return TurnResult.Fled;
    }

    private List<Beast> LivingBeasts()
        => _team.Beasts.Where(beast => beast.Alive).ToList();

    private static bool IsCancel(int option, int itemCount)
        => option > itemCount;
}
