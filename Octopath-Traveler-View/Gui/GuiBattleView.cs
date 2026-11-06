using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;
using OctopathTravelerGUI.Models.Enums;

namespace Octopath_Traveler_View.Gui;

public class GuiBattleView : IBattleView
{
    private const int NotFound = -1;
    private const int NoBoostPoints = 0;

    private readonly GuiWindow _window;

    public GuiBattleView(GuiWindow window)
    {
        _window = window;
    }

    public TravelerAction AskForAction(Traveler traveler)
    {
        List<string> labels = BattleMenu.ActionLabels;
        string chosen = _window.AskForOption(Option.Action, labels);
        return BattleMenu.GetActionAt(labels.IndexOf(chosen));
    }

    public string? AskForWeapon(Traveler traveler)
        => ChooseFrom(Option.Weapon, traveler.Weapons, weapon => weapon);

    public string? AskForAnyWeapon()
        => ChooseFrom(Option.Weapon, AttackTypeNames.WeaponNames, weapon => weapon);

    public ActiveSkill? AskForSkill(Traveler traveler)
        => ChooseFrom(Option.Skill, traveler.AffordableSkills, skill => skill.Name);

    public Unit? AskForTarget(Traveler traveler, IReadOnlyList<Unit> targets)
        => ChooseFrom(Option.Target, targets, target => target.Name);

    public int AskForBoostPoints(Traveler traveler)
    {
        IEnumerable<int> amounts = Enumerable.Range(NoBoostPoints, traveler.Bp + 1);
        List<string> labels = amounts.Select(amount => amount.ToString()).ToList();
        return int.Parse(_window.AskForOption(Option.BoostPoints, labels));
    }

    public void AnnounceNotEnoughBoostPoints(Traveler traveler, int amount)
    {
        // Los botones solo ofrecen de 0 a los BP que tiene el viajero: nunca se pide de más.
    }

    private T? ChooseFrom<T>(Option option, IReadOnlyList<T> items, Func<T, string> describe) where T : class
    {
        List<string> labels = items.Select(describe).ToList();
        string chosen = _window.AskForOption(option, BattleMenu.WithCancel(labels));
        int index = labels.IndexOf(chosen);
        return index == NotFound ? null : items[index];
    }
}
