using Octopath_Traveler.Models;

namespace Octopath_Traveler_View.Gui;

public class GuiCombatLogView : ICombatLogView
{
    private readonly GuiWindow _window;

    public GuiCombatLogView(GuiWindow window)
    {
        _window = window;
    }

    public void AnnounceBasicAttack(ActionReport report)
    {
        // La ventana no tiene registro de combate: el resultado se ve al redibujar el tablero.
    }

    public void AnnounceSkillUse(ActionReport report, string skillName)
    {
        // Igual que el ataque básico: el cambio se ve en el siguiente Update.
    }

    public void AnnounceFlee()
        => _window.MarkFled();
}
