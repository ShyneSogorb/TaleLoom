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
        
        for (UInt128 i = 0; i < GetAbsDays(); i+= DaysInYear())
        {
            years ++;
            
            if (IsLeapYear(years))
                ++i;
        }
        
        if (Tick < 0)
            return -years-1;
        else
            return years+1;
    }

    public static uint DaysInYear(bool leap = false)
    {
        return (uint)(365 + (leap ? 1 : 0));
    }
    
    public static uint DaysInYears(uint years)
    {
        return DaysInYear() * years;
    }

    public static bool IsLeapYear(int year)
    {
        return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
    }
    
}