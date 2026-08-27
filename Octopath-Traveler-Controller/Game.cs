using Octopath_Traveler.Combat;
using Octopath_Traveler.Data;
using Octopath_Traveler.Models;
using Octopath_Traveler.Utils;
using Octopath_Traveler_View;

namespace Octopath_Traveler;

public class Game
{
    private const string SelectTeamFileMessage = "Elige un archivo para cargar los equipos";
    private const string InvalidTeamFileMessage = "Archivo de equipos no válido";

    private readonly View _view;
    private readonly string _teamsFolder;
    private readonly OptionReader _optionReader;

    public Game(View view, string teamsFolder)
    {
        _view = view;
        _teamsFolder = teamsFolder;
        _optionReader = new OptionReader(view);
    }

    public void Play()
    {
        string teamFilePath = AskUserToSelectTeamFile();
        GameCatalog catalog = UnitDataLoader.LoadCatalog(GetDataFolder());
        ParsedTeamFile parsedTeam = TeamLoader.Load(teamFilePath);

        if (!TeamValidator.IsValid(parsedTeam, catalog))
        {
            _view.WriteLine(InvalidTeamFileMessage);
            return;
        }

        ValidatedTeam team = TeamBuilder.Build(parsedTeam, catalog);
        StartCombat(team);
    }

    private void StartCombat(ValidatedTeam team)
        => new CombatEngine(_view, team).Run();

    private string AskUserToSelectTeamFile()
    {
        string[] teamFiles = GetSortedTeamFiles();
        ShowTeamFileOptions(teamFiles);
        return teamFiles[_optionReader.Read()];
    }

    private string[] GetSortedTeamFiles()
    {
        string[] teamFiles = Directory.GetFiles(_teamsFolder, "*.txt");
        Array.Sort(teamFiles);
        return teamFiles;
    }

    private void ShowTeamFileOptions(string[] teamFiles)
    {
        _view.WriteLine(SelectTeamFileMessage);
        for (int i = 0; i < teamFiles.Length; i++)
            _view.WriteLine($"{i}: {Path.GetFileName(teamFiles[i])}");
    }

    private string GetDataFolder()
        => Path.GetDirectoryName(_teamsFolder) ?? _teamsFolder;
}
