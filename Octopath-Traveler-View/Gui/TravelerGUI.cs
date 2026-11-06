using Octopath_Traveler.Models;
using OctopathTravelerGUI.Models.Interfaces;

namespace Octopath_Traveler_View.Gui;

public class TravelerGUI : ITraveler
{
    private readonly Traveler _traveler;

    public TravelerGUI(Traveler traveler)
    {
        _traveler = traveler;
    }

    public string Name => _traveler.Name;
    public int HP => _traveler.CurrentHp;
    public int MaxHP => _traveler.MaxHp;
    public int SP => _traveler.CurrentSp;
    public int MaxSP => _traveler.MaxSp;
    public int BoostPoints => _traveler.Bp;
}
