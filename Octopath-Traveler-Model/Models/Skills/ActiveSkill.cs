namespace Octopath_Traveler.Models.Skills;

public class ActiveSkill
{
    private readonly EffectSet _effects;

    public ActiveSkill(string name, int spCost, SkillTarget target, List<Effect> effects)
    {
        Name = name;
        SpCost = spCost;
        Target = target;
        _effects = new EffectSet(effects);
    }

    public string Name { get; }
    public int SpCost { get; }
    public SkillTarget Target { get; }

    public bool NeedsTargetChoice
        => Target is SkillTarget.Single or SkillTarget.Ally;

    public bool NeedsWeaponChoice
        => _effects.NeedsWeapon;

    public List<Unit> Candidates(ValidatedTeam team)
        => Target is SkillTarget.Single or SkillTarget.Enemies
            ? team.LivingBeasts().Cast<Unit>().ToList()
            : team.LivingTravelers().Cast<Unit>().ToList();

    public ActionReport Use(SkillUse use)
        => _effects.ApplyTo(use);
}
