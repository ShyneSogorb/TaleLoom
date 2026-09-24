using Avalonia.Media.Imaging;

namespace TaleLoom.ViewModels.Images;

public class ImageItemViewModel
{
    public Bitmap Image { get; }
    public ImageItemViewModel(string path)
    {
        Image = new Bitmap(path);
    }
}