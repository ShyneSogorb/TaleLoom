using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Controls.Presenters;
using TaleLoom.ViewModels.Timeline;
using Avalonia.Media;

namespace TaleLoom.Views.Timeline;

public class TimelineCanvas : Control
{

    private const double ZoomFactor = 1.15;
    private const double MinPixelsPerDay = 0.0001;
    private const double MaxPixelsPerDay = 100.0;
    private readonly record struct TickInterval(int Years);
    private const double TargetTickSpacing = 100.0;
    private static readonly DateTime OriginDate = new(1, 1, 1);

    private TickInterval CalculateTickInterval()
    {
        var targetDays = TargetTickSpacing / PixelsPerDay;

        var targetYears = targetDays / 365.2425;

        int intervalYears = 1;

        while (intervalYears < targetYears)
        {
            intervalYears *= 2;

            if (intervalYears >= targetYears)
                break;

            intervalYears = (int)(intervalYears / 2.0 * 5.0);
        }

        return new TickInterval(intervalYears);
    }

    private DateTime FindFirstVisibleTick(TickInterval interval)
    {
        var days =
            -OffsetX / PixelsPerDay;

        var firstDate =
            OriginDate.AddDays(days);

        var intervalYears =
            interval.Years;

        var year =
            firstDate.Year;

        var remainder =
            year % intervalYears;

        if (remainder != 0)
            year += intervalYears - remainder;

        return new DateTime(
            year,
            1,
            1);
    }

    private bool IsVisible(DateTime date)
    {
        var days = (date - OriginDate).TotalDays;

        var x = days * PixelsPerDay + OffsetX;

        return x <= Bounds.Width;
    }

    private static DateTime AddInterval(DateTime date, TickInterval interval)
    {
        return date.AddYears(interval.Years);
    }
    
    public TimelineCanvas()
    {
        PointerWheelChanged += OnPointerWheelChanged;
    }

    static TimelineCanvas()
    {
        PixelsPerDayProperty.Changed.AddClassHandler<TimelineCanvas>(
            static (control, _) => control.InvalidateVisual());

        OffsetXProperty.Changed.AddClassHandler<TimelineCanvas>(
            static (control, _) => control.InvalidateVisual());
    }

    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        var pointer = e.GetPosition(this);
        var prevPixelsPerDay = PixelsPerDay;

        var daysAtPointer = (pointer.X - OffsetX) / prevPixelsPerDay;

        var zoom = e.Delta.Y > 0 ? ZoomFactor : 1.0 / ZoomFactor;

        var newPixelsPerDay = prevPixelsPerDay * zoom;

        newPixelsPerDay = Math.Clamp(
            newPixelsPerDay,
            MinPixelsPerDay,
            MaxPixelsPerDay);
        
        OffsetX = pointer.X - daysAtPointer * newPixelsPerDay;

        PixelsPerDay = newPixelsPerDay;

        e.Handled = true;

    }


    // public static readonly StyledProperty<DateTime> StartDateProperty =
    //     AvaloniaProperty.Register<TimelineCanvas, DateTime>(nameof(StartDate));
    //
    // public DateTime StartDate
    // {
    //     get => GetValue(StartDateProperty);
    //     set => SetValue(StartDateProperty, value);
    // }

    public static readonly StyledProperty<double> PixelsPerDayProperty =
        AvaloniaProperty.Register<TimelineCanvas, double>(nameof(PixelsPerDay), 100.0);

    public double PixelsPerDay
    {
        get => GetValue(PixelsPerDayProperty);
        set => SetValue(PixelsPerDayProperty, value);
    }
    
    public static readonly StyledProperty<double> OffsetXProperty =
        AvaloniaProperty.Register<TimelineCanvas, double>(nameof(OffsetX), 100.0);

    public double OffsetX
    {
        get => GetValue(OffsetXProperty);
        set => SetValue(OffsetXProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return availableSize;
    }

    private void DrawTick(DrawingContext context, DateTime date, TickInterval interval)
    {
        var days = (date - OriginDate).TotalDays;
        var x = days * PixelsPerDay + OffsetX;

        const double axisY = 50.0;
        const double tickHeight = 10.0;

        var pen = new Pen(Brushes.Black, 2);
        
        //Tick
        context.DrawLine(
            pen,
            new Point(x, axisY - tickHeight / 2),
            new Point(x, axisY + tickHeight / 2)
        );
            
        //Label
        var text = new FormattedText(
            date.Year.ToString(),
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface("Inter"),
            14,
            Brushes.Black
        );
            
        context.DrawText(
            text,
            new Point(
                x - text.Width / 2,
                axisY - tickHeight / 2 - text.Height
            )
        );
    }
    
    public override void Render(DrawingContext context)
    {
        base.Render(context);
        
        context.FillRectangle(
            Brushes.Transparent,
            new Rect(Bounds.Size));

        const double axisY = 50.0;

        var pen = new Pen(Brushes.Black, 2);
        
        //Axis
        context.DrawLine(
            pen, 
            new Point(0, axisY), 
            new Point(Bounds.Width, axisY)
        );
        
        var interval = CalculateTickInterval();

        var firstTick = FindFirstVisibleTick(interval);
        
        //Draw years that fit inside the viewport
        for (var date = firstTick;
             IsVisible(date);
             date = AddInterval(date, interval))
        {
            DrawTick(context, date, interval);
        }

    }
}