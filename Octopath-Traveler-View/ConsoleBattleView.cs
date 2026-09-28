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
    private const string PhysicalDamage = "físico";
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

    public void AnnounceTravelerAttack(AttackOutcome outcome, string weaponName)
    {
        ShowBlock($"{outcome.Attacker.Name} {AttacksMessage}");
        ShowDamage(outcome, $"{WeaponDamage} {weaponName}");
    }

    public void AnnounceBeastAttack(AttackOutcome outcome, string skillName)
    {
        ShowBlock($"{outcome.Attacker.Name} {UsesMessage} {skillName}");
        ShowDamage(outcome, PhysicalDamage);
    }

    public void AnnounceFlee()
        => ShowBlock(FleeMessage);

    private void ShowMenu(string header, List<string> options)
        => ShowNumberedList(header, options, MenuSeparator);

    private void ShowDamage(AttackOutcome outcome, string damageDescription)
    {
        WriteLine($"{outcome.Target.Name} recibe {outcome.Damage} de daño {damageDescription}");
        WriteLine($"{outcome.Target.Name} termina con HP:{outcome.Target.Stats.HpCurrent}");
    }

    private T? ChooseFrom<T>(List<T> items) where T : class
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

    private static List<string> WithCancel(List<string> options)
        => options.Append(CancelOption).ToList();
}
