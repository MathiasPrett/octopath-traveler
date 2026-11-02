namespace Octopath_Traveler.Models.Skills;

public abstract class Effect
{
    public virtual bool NeedsWeapon => false;

    public abstract CombatEvent Apply(SkillParameters parameters, Unit target);
}
