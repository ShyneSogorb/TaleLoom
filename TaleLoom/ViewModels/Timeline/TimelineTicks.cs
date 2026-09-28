using System;

namespace TaleLoom.ViewModels.Timeline;

public class TimelineTick
{
    public int Year { get; }
    public DateTime Date { get; }

    public TimelineTick(int year)
    {
        Year = year;
        Date = new DateTime(Year, 1, 1);
    }
}
