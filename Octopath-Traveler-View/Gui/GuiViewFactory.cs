namespace Octopath_Traveler_View.Gui;

public class GuiViewFactory : IViewFactory
{
    private readonly GuiWindow _window = new GuiWindow();

    public void Start(Action program)
        => _window.Start(program);

    public ITeamView CreateTeamView()
        => new GuiTeamView(_window);

    public IRoundView CreateRoundView()
        => new GuiRoundView(_window);

    public IBattleView CreateBattleView()
        => new GuiBattleView(_window);

    public ICombatLogView CreateCombatLogView()
        => new GuiCombatLogView(_window);
}
