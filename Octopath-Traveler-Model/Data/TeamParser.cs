namespace Octopath_Traveler.Data;

public static class TeamParser
{
    private const string TravelerTeamHeader = "Player Team";
    private const string BeastTeamHeader = "Enemy Team";
    private const int NotFound = -1;

    public static ParsedTeamFile Parse(string[] rawLines)
    {
        string[] lines = TrimAllLines(rawLines);
        int travelerHeaderIndex = Array.IndexOf(lines, TravelerTeamHeader);
        int beastHeaderIndex = Array.IndexOf(lines, BeastTeamHeader);
        return AreBothFound(travelerHeaderIndex, beastHeaderIndex)
            ? ParseSections(lines, travelerHeaderIndex, beastHeaderIndex)
            : CreateEmptyTeam();
    }

    private static ParsedTeamFile ParseSections(string[] lines, int travelerHeaderIndex, int beastHeaderIndex)
        => new ParsedTeamFile(
            ParseTravelerLines(lines, travelerHeaderIndex + 1, beastHeaderIndex),
            ParseBeastLines(lines, beastHeaderIndex + 1, lines.Length));

    private static ParsedTeamFile CreateEmptyTeam()
        => new ParsedTeamFile(new List<ParsedTraveler>(), new List<string>());

    private static bool AreBothFound(int firstIndex, int secondIndex)
        => firstIndex != NotFound && secondIndex != NotFound;

    private static string[] TrimAllLines(string[] lines)
        => lines.Select(line => line.Trim()).ToArray();

    private static List<ParsedTraveler> ParseTravelerLines(string[] lines, int start, int end)
        => SelectNonBlankLines(lines, start, end).Select(ParseTravelerLine).ToList();

    private static List<string> ParseBeastLines(string[] lines, int start, int end)
        => SelectNonBlankLines(lines, start, end).ToList();

    private static IEnumerable<string> SelectNonBlankLines(string[] lines, int start, int end)
    {
        IEnumerable<string> section = SelectSection(lines, start, end);
        return section.Where(line => !IsBlank(line));
    }

    private static IEnumerable<string> SelectSection(string[] lines, int start, int end)
    {
        IEnumerable<string> fromStart = lines.Skip(start);
        return fromStart.Take(end - start);
    }

    private static bool IsBlank(string line)
        => line.Length == 0;

    private static ParsedTraveler ParseTravelerLine(string line)
    {
        string name = ParseName(line);
        List<string> activeSkills = ParseBracketedList(line, '(', ')');
        List<string> passiveSkills = ParseBracketedList(line, '[', ']');
        return new ParsedTraveler(name, activeSkills, passiveSkills);
    }

    private static string ParseName(string line)
    {
        int cutIndex = FindFirstBracketIndex(line);
        string name = cutIndex == NotFound ? line : line.Substring(0, cutIndex);
        return name.Trim();
    }

    private static int FindFirstBracketIndex(string line)
    {
        int parenIndex = line.IndexOf('(');
        int bracketIndex = line.IndexOf('[');
        if (parenIndex == NotFound) return bracketIndex;
        if (bracketIndex == NotFound) return parenIndex;
        return Math.Min(parenIndex, bracketIndex);
    }

    private static List<string> ParseBracketedList(string line, char openChar, char closeChar)
    {
        int openIndex = line.IndexOf(openChar);
        int closeIndex = line.IndexOf(closeChar);
        if (!AreBothFound(openIndex, closeIndex)) return new List<string>();
        string inside = line.Substring(openIndex + 1, closeIndex - openIndex - 1).Trim();
        return IsBlank(inside) ? new List<string>() : SplitAndTrim(inside);
    }

    private static List<string> SplitAndTrim(string commaSeparated)
    {
        string[] parts = commaSeparated.Split(',');
        return parts.Select(part => part.Trim()).ToList();
    }
}
