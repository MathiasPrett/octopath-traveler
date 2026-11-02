using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;
using Octopath_Traveler_View;

namespace Octopath_Traveler.Combat;

public class TravelerTurn
{
    private const string? NoWeapon = null;

    private readonly IBattleView _battleView;
    private readonly ICombatLogView _combatLog;
    private readonly ValidatedTeam _team;

    public TravelerTurn(IBattleView battleView, ICombatLogView combatLog, ValidatedTeam team)
    {
        _battleView = battleView;
        _combatLog = combatLog;
        _team = team;
    }

    public TurnResult Play(Traveler traveler)
    {
        TurnResult result = TurnResult.Cancelled;
        while (result == TurnResult.Cancelled)
            result = ChooseAndExecuteAction(traveler);
        return result;
    }

    private TurnResult ChooseAndExecuteAction(Traveler traveler)
        => ExecuteAction(traveler, _battleView.AskForAction(traveler));

    private TurnResult ExecuteAction(Traveler traveler, TravelerAction action)
        => action switch
        {
            TravelerAction.BasicAttack => TryBasicAttack(traveler),
            TravelerAction.Skill => TryUseSkill(traveler),
            TravelerAction.Defend => Defend(traveler),
            TravelerAction.Flee => Flee(),
            _ => throw new ArgumentOutOfRangeException()
        };

    private TurnResult TryBasicAttack(Traveler traveler)
    {
        BasicAttackChoice? choice = AskForBasicAttackChoice(traveler);
        if (choice == null) return TurnResult.Cancelled;
        _battleView.AskForBoostPoints();
        return BasicAttack(traveler, choice);
    }

    private BasicAttackChoice? AskForBasicAttackChoice(Traveler traveler)
    {
        string? weapon = _battleView.AskForWeapon(traveler);
        if (weapon == null) return null;
        Unit? target = _battleView.AskForTarget(traveler, _team.LivingBeasts);
        return target == null ? null : new BasicAttackChoice(weapon, target);
    }

    private TurnResult BasicAttack(Traveler traveler, BasicAttackChoice choice)
    {
        _combatLog.AnnounceBasicAttack(traveler.BasicAttack(choice.Target, choice.WeaponName));
        return TurnResult.Completed;
    }

    private TurnResult TryUseSkill(Traveler traveler)
    {
        SkillChoice? choice = AskForSkillChoice(traveler);
        if (choice == null) return TurnResult.Cancelled;
        return UseSkill(traveler, choice);
    }

    private SkillChoice? AskForSkillChoice(Traveler traveler)
    {
        ActiveSkill? skill = _battleView.AskForSkill(traveler);
        if (skill == null) return null;
        SkillParameters? parameters = AskForSkillParameters(traveler, skill);
        return parameters == null ? null : new SkillChoice(skill, parameters);
    }

    private SkillParameters? AskForSkillParameters(Traveler traveler, ActiveSkill skill)
    {
        if (!skill.NeedsWeaponChoice) return AskForSkillTargets(traveler, skill, NoWeapon);
        string? weapon = _battleView.AskForAnyWeapon();
        return weapon == null ? null : AskForSkillTargets(traveler, skill, weapon);
    }

    private SkillParameters? AskForSkillTargets(Traveler traveler, ActiveSkill skill, string? weaponName)
    {
        List<Unit> candidates = skill.FindCandidates(_team, traveler);
        if (!skill.NeedsTargetChoice) return new SkillParameters(traveler, candidates, weaponName);
        Unit? target = _battleView.AskForTarget(traveler, candidates);
        return target == null ? null : new SkillParameters(traveler, new List<Unit> { target }, weaponName);
    }

    private TurnResult UseSkill(Traveler traveler, SkillChoice choice)
    {
        _battleView.AskForBoostPoints();
        _combatLog.AnnounceSkillUse(traveler.Use(choice.Skill, choice.Parameters), choice.Skill.Name);
        return TurnResult.Completed;
    }

    private TurnResult Defend(Traveler traveler)
    {
        traveler.Defend();
        return TurnResult.Completed;
    }

    private TurnResult Flee()
    {
        _combatLog.AnnounceFlee();
        return TurnResult.Fled;
    }
}
