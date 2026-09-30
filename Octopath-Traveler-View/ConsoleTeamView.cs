namespace Octopath_Traveler_View;

public class ConsoleTeamView : ITeamView
{
    private const string SelectTeamFileMessage = "Elige un archivo para cargar los equipos";
    private const string InvalidTeamFileMessage = "Archivo de equipos no válido";
    private const string TeamFilePattern = "*.txt";

    private readonly View _view;
    private readonly string _teamsFolder;
    private readonly OptionReader _optionReader;

    public ConsoleTeamView(View view, string teamsFolder)
    {
        _view = view;
        _teamsFolder = teamsFolder;
        _optionReader = new OptionReader(view);
    }

    public string[] AskForTeamFileLines()
        => File.ReadAllLines(AskForTeamFilePath());

    public void AnnounceInvalidTeamFile()
        => _view.WriteLine(InvalidTeamFileMessage);

    private string AskForTeamFilePath()
    {
        string[] teamFiles = GetSortedTeamFiles();
        ShowTeamFileOptions(teamFiles);
        return teamFiles[_optionReader.Read()];
    }

    private string[] GetSortedTeamFiles()
    {
        string[] teamFiles = Directory.GetFiles(_teamsFolder, TeamFilePattern);
        Array.Sort(teamFiles);
        return teamFiles;
    }

    private void ShowTeamFileOptions(string[] teamFiles)
    {
        _view.WriteLine(SelectTeamFileMessage);
        for (int index = 0; index < teamFiles.Length; index++)
            _view.WriteLine($"{index}: {Path.GetFileName(teamFiles[index])}");
    }
}
