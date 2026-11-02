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
    private const string RecoversMessage = "recupera";
    private const string HealthSuffix = "de vida";
    private const string RevivesMessage = "revive";
    private const string TrapMessage = "tendrá menor prioridad de turno durante";
    private const string RoundsSuffix = "rondas";
    private const string SpSuffix = "SP";

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

    void ICombatEventVisitor.Visit(HealEvent heal)
        => WriteLine($"{heal.Target.Name} {RecoversMessage} {heal.Amount} {HealthSuffix}");

    void ICombatEventVisitor.Visit(ReviveEvent revive)
        => WriteLine($"{revive.Target.Name} {RevivesMessage}");

    void ICombatEventVisitor.Visit(SpRecoveryEvent spRecovery)
        => WriteLine($"{spRecovery.Target.Name} {RecoversMessage} {spRecovery.Amount} {SpSuffix}");

    void ICombatEventVisitor.Visit(TrapEvent trap)
        => WriteLine($"{trap.Target.Name} {TrapMessage} {trap.Rounds} {RoundsSuffix}");

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
