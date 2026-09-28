using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler_View;

public class ConsoleBattleView : ConsoleCombatView, IBattleView
{
    private const string TurnHeader = "Turno de";
    private const string WeaponMenuHeader = "Seleccione un arma";
    private const string SkillMenuHeader = "Seleccione una habilidad para";
    private const string TargetMenuHeader = "Seleccione un objetivo para";
    private const string BoostPointPrompt = "Seleccione cuantos BP utilizar";
    private const string CancelOption = "Cancelar";
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
    private const string MenuSeparator = ": ";
    private const int BasicAttackOption = 1;
    private const int SkillOption = 2;
    private const int FleeOption = 4;

    private static readonly List<string> ActionOptions =
        new() { "Ataque básico", "Usar habilidad", "Defender", "Huir" };

    private readonly OptionReader _optionReader;

    public ConsoleBattleView(View view) : base(view)
    {
        _optionReader = new OptionReader(view);
    }

    public TravelerAction AskForAction(Traveler traveler)
    {
        ShowMenu($"{TurnHeader} {traveler.Name}", ActionOptions);
        return ToAction(_optionReader.Read());
    }

    public string? AskForWeapon(Traveler traveler)
    {
        ShowMenu(WeaponMenuHeader, WithCancel(traveler.Weapons));
        return ChooseFrom(traveler.Weapons);
    }

    public string? AskForAnyWeapon()
    {
        ShowMenu(WeaponMenuHeader, WithCancel(AttackType.WeaponNames));
        return ChooseFrom(AttackType.WeaponNames);
    }

    public ActiveSkill? AskForSkill(Traveler traveler)
    {
        IReadOnlyList<ActiveSkill> skills = traveler.AffordableSkills();
        ShowMenu($"{SkillMenuHeader} {traveler.Name}", WithCancel(NamesOf(skills)));
        return ChooseFrom(skills);
    }

    public Unit? AskForTarget(Traveler traveler, IReadOnlyList<Unit> targets)
    {
        ShowMenu($"{TargetMenuHeader} {traveler.Name}", WithCancel(DescribeAll(targets)));
        return ChooseFrom(targets);
    }

    public int AskForBoostPoints()
    {
        ShowBlock(BoostPointPrompt);
        return _optionReader.Read();
    }

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

    private void ShowMenu(string header, IReadOnlyList<string> options)
        => ShowNumberedList(header, options, MenuSeparator);

    private void ShowReport(ActionReport report)
    {
        foreach (CombatEvent combatEvent in report.Events)
            ShowEvent(combatEvent);
        foreach (Unit unit in report.UnitsWithHpChanges)
            WriteLine($"{unit.Name} termina con HP:{unit.CurrentHp}");
    }

    private void ShowEvent(CombatEvent combatEvent)
    {
        switch (combatEvent)
        {
            case DamageEvent damage: ShowDamage(damage); break;
        }
    }

    private void ShowDamage(DamageEvent damage)
    {
        if (damage.TargetWasDefending) WriteLine($"{damage.Target.Name} {DefendsMessage}");
        WriteLine($"{damage.Target.Name} recibe {damage.Damage} de daño{Describe(damage)}");
        if (damage.CausedBreak) WriteLine($"{damage.Target.Name} {BreakingPointMessage}");
    }

    private static string Describe(DamageEvent damage)
        => DamageKind(damage.Type) + (damage.ExploitedWeakness ? WeaknessSuffix : NoSuffix);

    private static string DamageKind(AttackType? type)
    {
        if (type == null) return NoSuffix;
        if (type.HasName) return $" {WeaponDamage} {type.Name}";
        return type.Category == AttackCategory.Physical ? $" {PhysicalDamage}" : $" {ElementalDamage}";
    }

    private T? ChooseFrom<T>(IReadOnlyList<T> items) where T : class
    {
        int option = _optionReader.Read();
        return IsCancel(option, items.Count) ? null : items[option - 1];
    }

    private static bool IsCancel(int option, int itemCount)
        => option > itemCount;

    private static TravelerAction ToAction(int option)
    {
        if (option == BasicAttackOption) return TravelerAction.BasicAttack;
        if (option == SkillOption) return TravelerAction.Skill;
        if (option == FleeOption) return TravelerAction.Flee;
        return TravelerAction.Defend;
    }

    private static List<string> WithCancel(IReadOnlyList<string> options)
        => options.Append(CancelOption).ToList();

    private static List<string> NamesOf(IReadOnlyList<ActiveSkill> skills)
        => skills.Select(skill => skill.Name).ToList();
}
