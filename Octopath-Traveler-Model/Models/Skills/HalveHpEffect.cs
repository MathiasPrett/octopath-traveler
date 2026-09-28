namespace Octopath_Traveler.Models.Skills;

// Vortal Claw: el objetivo queda con la mitad de su HP, sin importar defensas ni tipos.
public class HalveHpEffect : Effect
{
    private const int Half = 2;

    public override CombatEvent Apply(SkillUse use, Unit target)
        => target.TakeDirectDamage(target.CurrentHp - target.CurrentHp / Half);
}
