using Octopath_Traveler.Models;

namespace Octopath_Traveler_View;

public static class BattleMenu
{
    public const string Cancel = "Cancelar";

    private static readonly (TravelerAction Action, string Label)[] Actions =
    {
        (TravelerAction.BasicAttack, "Ataque básico"),
        (TravelerAction.Skill, "Usar habilidad"),
        (TravelerAction.Defend, "Defender"),
        (TravelerAction.Flee, "Huir")
    };

    public static List<string> ActionLabels
        => Actions.Select(item => item.Label).ToList();

    public static TravelerAction GetActionAt(int index)
        => Actions[index].Action;

    public static List<string> WithCancel(IEnumerable<string> options)
        => options.Append(Cancel).ToList();
}
