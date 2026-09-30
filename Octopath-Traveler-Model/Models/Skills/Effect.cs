namespace Octopath_Traveler.Models.Skills;

public abstract class Effect
{
    public virtual bool NeedsWeapon => false;

    public abstract CombatEvent Apply(SkillUse use, Unit target);
}
