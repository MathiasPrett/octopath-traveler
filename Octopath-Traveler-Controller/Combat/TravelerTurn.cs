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
        string? weapon = _battleView.AskForWeapon(traveler);
        if (weapon == null) return TurnResult.Cancelled;
        Unit? target = _battleView.AskForTarget(traveler, _team.LivingBeasts);
        if (target == null) return TurnResult.Cancelled;
        _battleView.AskForBoostPoints();
        return BasicAttack(traveler, target, weapon);
    }

    private TurnResult BasicAttack(Traveler traveler, Unit target, string weaponName)
    {
        _combatLog.AnnounceBasicAttack(traveler.BasicAttack(target, weaponName));
        return TurnResult.Completed;
    }

    private TurnResult TryUseSkill(Traveler traveler)
    {
        ActiveSkill? skill = _battleView.AskForSkill(traveler);
        if (skill == null) return TurnResult.Cancelled;
        SkillUse? use = AskForSkillUse(traveler, skill);
        if (use == null) return TurnResult.Cancelled;
        return UseSkill(traveler, skill, use);
    }

    private SkillUse? AskForSkillUse(Traveler traveler, ActiveSkill skill)
    {
        if (!skill.NeedsWeaponChoice) return AskForSkillTargets(traveler, skill, NoWeapon);
        string? weapon = _battleView.AskForAnyWeapon();
        return weapon == null ? null : AskForSkillTargets(traveler, skill, weapon);
    }

    private SkillUse? AskForSkillTargets(Traveler traveler, ActiveSkill skill, string? weaponName)
    {
        List<Unit> candidates = skill.FindCandidates(_team, traveler);
        if (!skill.NeedsTargetChoice) return new SkillUse(traveler, candidates, weaponName);
        Unit? target = _battleView.AskForTarget(traveler, candidates);
        return target == null ? null : new SkillUse(traveler, new List<Unit> { target }, weaponName);
    }

    private TurnResult UseSkill(Traveler traveler, ActiveSkill skill, SkillUse use)
    {
        _battleView.AskForBoostPoints();
        _combatLog.AnnounceSkillUse(traveler.Use(skill, use), skill.Name);
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
