namespace Octopath_Traveler.Models.Skills;

public class ActiveSkill : Skill
{
    private readonly SkillTargeting _targeting;

    public ActiveSkill(string name, int spCost, SkillTargeting targeting, List<Effect> effects)
        : base(name, effects)
    {
        SpCost = spCost;
        _targeting = targeting;
    }

    public int SpCost { get; }

    public bool NeedsTargetChoice => _targeting.NeedsChoice;

    public bool NeedsWeaponChoice => NeedsWeapon;

    public List<Unit> FindCandidates(ValidatedTeam team, Unit user)
        => _targeting.FindCandidates(team, user);

    public ActionReport Use(SkillUse use)
        => Apply(use);
}
