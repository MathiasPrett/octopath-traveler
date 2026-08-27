using Octopath_Traveler_View;

namespace Octopath_Traveler.Utils;

public class OptionReader
{
    private readonly View _view;

    public OptionReader(View view)
    {
        _view = view;
    }

    public int Read()
        => int.Parse(_view.ReadLine());
}
