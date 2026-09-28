namespace TaleLoom.Core.Timeline;

public class Timeline
{
    public DateTime InitialOffset { get; private set; }

    private Timeline()
    {
        InitialOffset = DateTime.Now;
    }
    
    public static Timeline LoadProjectTimeline()
    {
        return new Timeline();
    }

}