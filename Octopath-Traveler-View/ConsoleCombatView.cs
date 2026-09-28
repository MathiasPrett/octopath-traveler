using Octopath_Traveler.Models;

namespace Octopath_Traveler_View;

public abstract class ConsoleCombatView
{
    private const string Separator = "----------------------------------------";

    private readonly View _view;

    protected ConsoleCombatView(View view)
    {
        _view = view;
    }

    protected void WriteLine(string text)
        => _view.WriteLine(text);

    protected void ShowSeparator()
        => WriteLine(Separator);

    protected void ShowBlock(string header)
    {
        ShowSeparator();
        WriteLine(header);
    }

    protected void ShowNumberedList(string header, IReadOnlyList<string> items, string separator)
    {
        ShowBlock(header);
        for (int index = 0; index < items.Count; index++)
            WriteLine($"{index + 1}{separator}{items[index]}");
    }

    protected static string Describe(Traveler traveler)
        => $"{traveler.Name} - HP:{traveler.CurrentHp}/{traveler.MaxHp}"
           + $" SP:{traveler.SpCurrent}/{traveler.SpMax} BP:{traveler.Bp}";

    protected static string Describe(Beast beast)
        => $"{beast.Name} - HP:{beast.CurrentHp}/{beast.MaxHp} Shields:{beast.Shields}";

    protected static List<string> DescribeAll(IReadOnlyList<Traveler> travelers)
        => travelers.Select(Describe).ToList();

    protected static List<string> DescribeAll(IReadOnlyList<Beast> beasts)
        => beasts.Select(Describe).ToList();
}
