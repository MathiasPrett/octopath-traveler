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
    private const string NotEnoughBoostPointsMessage = "no tiene";
    private const string BoostPointsSuffix = "BP para utilizar";
    private const string MenuSeparator = ": ";

    private readonly OptionReader _optionReader;

    public ConsoleBattleView(View view) : base(view)
    {
        _optionReader = new OptionReader(view);
    }

    public TravelerAction AskForAction(Traveler traveler)
    {
        ShowMenu($"{TurnHeader} {traveler.Name}", BattleMenu.ActionLabels);
        return BattleMenu.GetActionAt(_optionReader.Read() - 1);
    }

    public string? AskForWeapon(Traveler traveler)
    {
        ShowMenu(WeaponMenuHeader, BattleMenu.WithCancel(traveler.Weapons));
        return ChooseFrom(traveler.Weapons);
    }

    public string? AskForAnyWeapon()
    {
        ShowMenu(WeaponMenuHeader, BattleMenu.WithCancel(AttackTypeNames.WeaponNames));
        return ChooseFrom(AttackTypeNames.WeaponNames);
    }

    public ActiveSkill? AskForSkill(Traveler traveler)
    {
        IReadOnlyList<ActiveSkill> skills = traveler.AffordableSkills;
        ShowMenu($"{SkillMenuHeader} {traveler.Name}", BattleMenu.WithCancel(GetNames(skills)));
        return ChooseFrom(skills);
    }

    public Unit? AskForTarget(Traveler traveler, IReadOnlyList<Unit> targets)
    {
        ShowMenu($"{TargetMenuHeader} {traveler.Name}", BattleMenu.WithCancel(DescribeAll(targets)));
        return ChooseFrom(targets);
    }

    public int AskForBoostPoints(Traveler traveler)
    {
        ShowBlock(BoostPointPrompt);
        return _optionReader.Read();
    }

    public void AnnounceNotEnoughBoostPoints(Traveler traveler, int amount)
        => ShowBlock($"{traveler.Name} {NotEnoughBoostPointsMessage} {amount} {BoostPointsSuffix}");

    private void ShowMenu(string header, IReadOnlyList<string> options)
        => ShowNumberedList(header, options, MenuSeparator);

    private T? ChooseFrom<T>(IReadOnlyList<T> items) where T : class
    {
        int option = _optionReader.Read();
        return IsCancel(option, items.Count) ? null : items[option - 1];
    }

    private static bool IsCancel(int option, int itemCount)
        => option > itemCount;

    private static List<string> GetNames(IReadOnlyList<ActiveSkill> skills)
        => skills.Select(skill => skill.Name).ToList();
}
