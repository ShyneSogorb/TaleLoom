using System.Collections.Generic;
using System.Linq;

namespace TaleLoom.ViewModels.Timeline;

using TaleLoom.Core.Timeline;

public class TimelineViewModel : ViewModelBase
{

    //private Timeline _timeline;

    public int StartYear { get; } = 1100;
    public int EndYear { get; } = 1200;

    public IReadOnlyList<TimelineTick> Ticks { get; }
    
    public TimelineViewModel()
    {
        Ticks = Enumerable.Range(StartYear, EndYear - StartYear + 1)
            .Select(year => new TimelineTick(year))
            .ToList();
        
        //_timeline = Timeline.LoadProjectTimeline();
    }
}