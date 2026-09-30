namespace Octopath_Traveler_View;

public class ConsoleViewFactory : IViewFactory
{
    private readonly View _view;
    private readonly string _teamsFolder;

    public ConsoleViewFactory(View view, string teamsFolder)
    {
        _view = view;
        _teamsFolder = teamsFolder;
    }

    public ITeamView CreateTeamView()
        => new ConsoleTeamView(_view, _teamsFolder);

    public IRoundView CreateRoundView()
        => new ConsoleRoundView(_view);

    public IBattleView CreateBattleView()
        => new ConsoleBattleView(_view);

    public ICombatLogView CreateCombatLogView()
        => new ConsoleCombatLogView(_view);
}
