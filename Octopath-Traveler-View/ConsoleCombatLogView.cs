using Octopath_Traveler.Models;

namespace Octopath_Traveler_View;

public class ConsoleCombatLogView : ConsoleCombatView, ICombatLogView, ICombatEventVisitor
{
    private const string FleeMessage = "El equipo de viajeros ha huido!";
    private const string AttacksMessage = "ataca";
    private const string UsesMessage = "usa";
    private const string DefendsMessage = "se defiende";
    private const string BreakingPointMessage = "entra en Breaking Point";
    private const string WeaknessSuffix = " con debilidad";
    private const string NoSuffix = "";
    private const string PhysicalDamage = "físico";
    private const string ElementalDamage = "elemental";
    private const string WeaponDamage = "de tipo";

    public ConsoleCombatLogView(View view) : base(view) { }

    public void AnnounceBasicAttack(ActionReport report)
    {
        ShowBlock($"{report.Actor.Name} {AttacksMessage}");
        ShowReport(report);
    }

    public void AnnounceSkillUse(ActionReport report, string skillName)
    {
        ShowBlock($"{report.Actor.Name} {UsesMessage} {skillName}");
        ShowReport(report);
    }

    public void AnnounceFlee()
        => ShowBlock(FleeMessage);

    void ICombatEventVisitor.Visit(DamageEvent damage)
    {
        if (damage.TargetWasDefending) WriteLine($"{damage.Target.Name} {DefendsMessage}");
        WriteLine($"{damage.Target.Name} recibe {damage.Damage} de daño{Describe(damage)}");
        if (damage.CausedBreak) WriteLine($"{damage.Target.Name} {BreakingPointMessage}");
    }

    private void ShowReport(ActionReport report)
    {
        foreach (CombatEvent combatEvent in report.Events)
            combatEvent.Accept(this);
        foreach (Unit unit in report.UnitsWithHpChanges)
            WriteLine($"{unit.Name} termina con HP:{unit.CurrentHp}");
    }

    private static string Describe(DamageEvent damage)
        => FormatDamageKind(damage.Type) + (damage.ExploitedWeakness ? WeaknessSuffix : NoSuffix);

    private static string FormatDamageKind(AttackType? type)
    {
        if (type == null) return NoSuffix;
        if (type.HasName) return $" {WeaponDamage} {type.Name}";
        return type.Category == AttackCategory.Physical ? $" {PhysicalDamage}" : $" {ElementalDamage}";
    }
}
