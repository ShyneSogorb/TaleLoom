using System;

namespace TaleLoom.ViewModels.Timeline;

public class TimelineDate
{
    public readonly int Year;
    public readonly uint Month;
    public readonly uint Day;
    private readonly Int128 Tick;

    public Int128 GetDays()
    {
        return Tick;
    }
    
    public UInt128 GetAbsDays()
    {
        return (UInt128)Tick;
    }

    public TimelineDate(Int128 days)
    {
        Tick = days;
    }
    
    public TimelineDate(int year, uint month, uint day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public int GetYear()
    {
        int years = 0;
        for (UInt128 i = 0; i < GetAbsDays(); i+=365*4)
        {
            years += 4;
            
            if (IsLeapYear(years))
                ++i;
        }

        return years;
    }

    public static bool IsLeapYear(int year)
    {
        return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
    }
    
}