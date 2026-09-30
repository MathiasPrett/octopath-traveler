using Octopath_Traveler.Models;

namespace Octopath_Traveler_View;

public abstract class ConsoleCombatView
{
    private const string Separator = "----------------------------------------";

    private static readonly UnitDescriber Describer = new UnitDescriber();

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

    protected static string Describe(Unit unit)
        => unit.Accept(Describer);

    protected static List<string> DescribeAll(IReadOnlyList<Unit> units)
        => units.Select(Describe).ToList();

    private class UnitDescriber : IUnitVisitor<string>
    {
        public string VisitTraveler(Traveler traveler)
            => $"{traveler.Name} - HP:{traveler.CurrentHp}/{traveler.MaxHp}"
               + $" SP:{traveler.CurrentSp}/{traveler.MaxSp} BP:{traveler.Bp}";

        public string VisitBeast(Beast beast)
            => $"{beast.Name} - HP:{beast.CurrentHp}/{beast.MaxHp} Shields:{beast.Shields}";
    }
}
