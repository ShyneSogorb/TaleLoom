using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using TaleLoom.Infrastructure.Files;
using TaleLoom.ViewModels.Images;

namespace TaleLoom.Views.Images;

public partial class ImageGalleryView : UserControl
{
    
    private ImageGalleryViewModel? _viewModel;
    
    public ImageGalleryView()
    {
        InitializeComponent();

        DataContextChanged += OnDataContextChanged;
        
    }
    
    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (_viewModel != null)
            _viewModel.AddImageRequested -= AddImage;

        _viewModel = DataContext as ImageGalleryViewModel;

        if (_viewModel != null)
            _viewModel.AddImageRequested += AddImage;
    }

    private async void AddImage()
    {
        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel == null) 
            return;

        
        if (_viewModel == null) return;
        
        var imagesFolder = await topLevel.StorageProvider
            .TryGetFolderFromPathAsync(_viewModel.ImagesDirectory);

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Select Image",
                AllowMultiple = false,
                FileTypeFilter = 
                [
                    new FilePickerFileType("Images")
                    {
                        Patterns = [
                            "*.png",
                            "*.jpg",
                            "*.jpeg",
                            "*.webp"
                        ]
                    }
                ]
            }
        );

        if (files.Count == 0) return;

        
        foreach (var file in files)
        {
            var path = file.Path.LocalPath;

            var destination = Path.Combine(_viewModel.ImagesDirectory, Path.GetFileName(path));

            if (!string.Equals(path, destination, StringComparison.OrdinalIgnoreCase))
            {
                File.Copy(path, destination, overwrite:false);
            }

            _viewModel.AddImage(destination);

        }

    }
}