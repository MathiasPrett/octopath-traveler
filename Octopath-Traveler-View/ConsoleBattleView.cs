using Octopath_Traveler.Models;

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

    public string? AskForSkill(Traveler traveler)
    {
        ShowMenu($"{SkillMenuHeader} {traveler.Name}", WithCancel(traveler.ActiveSkills));
        return ChooseFrom(traveler.ActiveSkills);
    }

    public Beast? AskForTarget(Traveler traveler, List<Beast> targets)
    {
        ShowMenu($"{TargetMenuHeader} {traveler.Name}", WithCancel(DescribeAll(targets)));
        return ChooseFrom(targets);
    }

    public int AskForBoostPoints()
    {
        ShowBlock(BoostPointPrompt);
        return _optionReader.Read();
    }

    public void AnnounceTravelerAttack(Traveler attacker, HitResult hit)
    {
        ShowBlock($"{attacker.Name} {AttacksMessage}");
        ShowHit(hit, $"{WeaponDamage} {hit.Type.Name}");
    }

    public void AnnounceBeastAttack(Beast attacker, HitResult hit)
    {
        ShowBlock($"{attacker.Name} {UsesMessage} {attacker.Skill}");
        ShowHit(hit, DamageKind(hit.Type));
    }

    public void AnnounceFlee()
        => ShowBlock(FleeMessage);

    private void ShowMenu(string header, IReadOnlyList<string> options)
        => ShowNumberedList(header, options, MenuSeparator);

    private void ShowHit(HitResult hit, string damageDescription)
    {
        if (hit.TargetWasDefending) WriteLine($"{hit.Target.Name} {DefendsMessage}");
        WriteLine($"{hit.Target.Name} recibe {hit.Damage} de daño {damageDescription}{Weakness(hit)}");
        if (hit.CausedBreak) WriteLine($"{hit.Target.Name} {BreakingPointMessage}");
        WriteLine($"{hit.Target.Name} termina con HP:{hit.Target.CurrentHp}");
    }

    private static string Weakness(HitResult hit)
        => hit.ExploitedWeakness ? WeaknessSuffix : NoSuffix;

    private static string DamageKind(AttackType type)
        => type.Category == AttackCategory.Physical ? PhysicalDamage : ElementalDamage;

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
}
