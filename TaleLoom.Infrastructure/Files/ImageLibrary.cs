namespace TaleLoom.Infrastructure.Files;

public sealed class ImageLibrary
{
    



    public IEnumerable<string> GetImages()
    {
        return Directory.EnumerateFiles(
                TaleLoomDataDirectory.Images,
                "*.*",
                SearchOption.TopDirectoryOnly)
            .Where(IsImage);
    }

    private static bool IsImage(string path)
    {
        var format = Path.GetExtension(path);

        var imageFormats = new []{"png", "jpg", "jpeg", "webp"};
        
        return imageFormats.Any(f => $".{f}".Equals(format, StringComparison.OrdinalIgnoreCase));

    }
}