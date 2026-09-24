using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TaleLoom.Infrastructure.Files;
using TaleLoom.ViewModels.Prefabs;

namespace TaleLoom.ViewModels.Images;

public class ImageGalleryViewModel : ViewModelBase
{
    public ObservableCollection<ImageItemViewModel> Images { get; }

    public string ImagesDirectory => TaleLoomDataDirectory.Images;
    public ICommand AddImageCommand { get; }
    public event Action? AddImageRequested;
    
    public ImageGalleryViewModel(ImageLibrary imageLibrary)
    {
        Images = new ObservableCollection<ImageItemViewModel>(
            imageLibrary.GetImages()
            .Select(img => new ImageItemViewModel(img))
        );
        
        AddImageCommand = new RelayCommand(
            _ => AddImageRequested?.Invoke());
    }

    public void AddImage(string path)
    {
        Images.Add(new ImageItemViewModel(path));
    }
}