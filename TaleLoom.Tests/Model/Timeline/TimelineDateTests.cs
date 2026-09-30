
using TaleLoom.ViewModels.Timeline;

namespace TaleLoom.Tests.Model.Timeline;

public class TimelineDateTests
{
    [Fact]
    public void CorrectDays()
    {
        var twoT = new DateOnly(2000, 1, 1);
        var first = new DateOnly(2, 1, 1);
        var cent = new DateOnly(300, 1, 1);

        var mtwoT = new TimelineDate(twoT.DayNumber);
        var mfirst = new TimelineDate(first.DayNumber);
        var mcent = new TimelineDate(cent.DayNumber);
        
        Assert.Equal(twoT.Year, mtwoT.GetYear());
        Assert.Equal(cent.Year, mcent.GetYear());
        Assert.Equal(first.Year, mfirst.GetYear());
    }

}