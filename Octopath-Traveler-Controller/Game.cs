using Octopath_Traveler.Combat;
using Octopath_Traveler.Data;
using Octopath_Traveler.Models;
using Octopath_Traveler_View;

namespace Octopath_Traveler;

public class Game
{
    private readonly IViewFactory _viewFactory;
    private readonly ITeamView _teamView;
    private readonly string _dataFolder;

    public Game(View view, string teamsFolder)
        : this(new ConsoleViewFactory(view, teamsFolder),
               Path.GetDirectoryName(teamsFolder) ?? teamsFolder) { }

    public Game(IViewFactory viewFactory, string dataFolder)
    {
        _viewFactory = viewFactory;
        _teamView = viewFactory.CreateTeamView();
        _dataFolder = dataFolder;
    }

    public void Play()
    {
        ParsedTeamFile parsedTeam = TeamParser.Parse(_teamView.AskForTeamFileLines());
        GameCatalog catalog = CatalogLoader.LoadCatalog(_dataFolder);

        if (!TeamValidator.IsValid(parsedTeam, catalog))
        {
            _teamView.AnnounceInvalidTeamFile();
            return;
        }

        StartCombat(TeamBuilder.Build(parsedTeam, catalog));
    }

    private void StartCombat(ValidatedTeam team)
        => new CombatEngine(_viewFactory, team).Run();
}
