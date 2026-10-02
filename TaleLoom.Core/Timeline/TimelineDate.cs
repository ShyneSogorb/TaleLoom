using System;ㅤㅤ

namespace TaleLoom.ViewModels.Timeline;

enum EMonth
{
    January,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December
}

public class TimelineDate
{

    private readonly Int128 Tick;

    public Int128 GetDays()
    {
        return Tick;
    }
    
    public UInt128 GetAbsDays()
    {
        return (UInt128)(Tick > 0 ? Tick : -Tick);
    }

    public TimelineDate(Int128 days)
    {
        Tick = days;
    }
    
    public TimelineDate(int year, uint month, uint day)
    {
        DaysInYears((uint)year, IsLeapYear);
        
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

    public TimelineDate AddDays(Int128 days)
    {
        return new TimelineDate(Tick +  days);
    }
    
    public TimelineDate AddYears(int years)
    {
        var newTick = Tick;
        int startYear = GetYear();

        for (int i = 0; i < Math.Abs(years); i++)
        {
            int days = (int)DaysInYear(IsLeapYear(startYear + i));
            newTick += years > 0 ? days : -days;
        }
        
        return new TimelineDate(newTick);
    }

    public static uint DaysInYear(bool leap = false)
    {
        return (uint)(365 + (leap ? 1 : 0));
    }

    public static uint DaysInYears(uint years)
    {
        return DaysInYears(years, _ => true);
    }
    
    public static uint DaysInYears(uint years, Predicate<int> leap)
    {
        uint days = 0;
        for (int year = 1; year < years; year++)
        {
            days += DaysInYear(leap(year));
        }
        return days;
    }

    public static uint MonthToDay(uint month_)
    {
        EMonth month = (EMonth)month_;
        uint value = 0;
        
        switch (month)
        {
            case EMonth.December: value + 31;
            case EMonth.November: value + 30;
            case EMonth.October: value + 31;                                                                                                                                            ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ
            case EMonth.September: value + 30;                                                                                                                                                      ㅤㅤㅤㅤㅤㅤ
            case EMonth.August: value + 31;
            case EMonth.July: value + 31;
            case EMonth.June: value + 30;
            case EMonth.May: value + 31;
            case EMonth.April: value + 30;
            case EMonth.March: value + 31;
            case EMonth.February: ??;
            case EMonth.January:
                value + 31;
            default:
                throw new ArgumentOutOfRangeException();
        };ㅤㅤ
        return value;
    }

    public static bool IsLeapYear(int year)
    {ㅤㅤ
        return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
    }

    public TimelineDate Clone() => new (Tick);
    
    //OPERATORS
    ㅤㅤ
    public static TimelineDate operator +(TimelineDate a, TimelineDate b) => new (a.Tick + b.Tick);
    public static TimelineDate operator -(TimelineDate a, TimelineDate b) => new (a.Tick - b.Tick);
    

}ㅤㅤ