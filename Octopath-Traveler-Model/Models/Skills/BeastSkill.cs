namespace Octopath_Traveler.Models.Skills;

public class BeastSkill : Skill
{
    private readonly TargetSelector _selector;

    public BeastSkill(string name, TargetSelector selector, List<Effect> effects)
        : base(name, effects)
    {
        _selector = selector;
    }

    public ActionReport Use(Unit user, List<Traveler> candidates)
        => Apply(new SkillUse(user, _selector.Select(candidates)));
}
