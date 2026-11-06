using Octopath_Traveler.Models;
using OctopathTravelerGUI.Models.Interfaces;

namespace Octopath_Traveler_View.Gui;

public class BeastGUI : IBeast
{
    private readonly Beast _beast;

    public BeastGUI(Beast beast)
    {
        _beast = beast;
    }

    public string Name => _beast.Name;
    public int HP => _beast.CurrentHp;
    public int MaxHP => _beast.MaxHp;
    public int Shields => _beast.Shields;
}
