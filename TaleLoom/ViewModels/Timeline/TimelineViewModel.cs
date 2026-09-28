namespace TaleLoom.ViewModels.Timeline;

using TaleLoom.Core.Timeline;

public class TimelineViewModel : ViewModelBase
{

    private Timeline _timeline;

    public int StartYear { get; } = 1100;
    public int EndYear { get; } = 1200;

    public TimelineViewModel()
    {
        _timeline = Timeline.LoadProjectTimeline();
    }
}