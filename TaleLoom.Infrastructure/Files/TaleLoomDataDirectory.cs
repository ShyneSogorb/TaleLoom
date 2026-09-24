namespace TaleLoom.Infrastructure.Files;

public static class TaleLoomDataDirectory
{
    public static string Root { get; } =
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            "TaleLoom");

    public static string Images => Path.Combine(Root, "Images");
    public static string Projects => Path.Combine(Root, "Projects");

    static TaleLoomDataDirectory()
    {
        Directory.CreateDirectory(Root);
        Directory.CreateDirectory(Images);
        Directory.CreateDirectory(Projects);
    }
}