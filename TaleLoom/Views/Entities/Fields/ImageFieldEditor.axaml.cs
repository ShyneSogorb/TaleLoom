using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using TaleLoom.Infrastructure.Files;
using TaleLoom.ViewModels.Entities;
using TaleLoom.ViewModels.Entities.Fields;
using TaleLoom.ViewModels.Images;

namespace TaleLoom.Views.Entities.Fields;

public partial class ImageFieldEditor : UserControl
{
    
    private ImageFieldEditorViewModel? _viewModel;

    
    public ImageFieldEditor()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }
    
    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (_viewModel != null)
            _viewModel.SelectImageRequested -= AddImage;

        _viewModel = DataContext as ImageFieldEditorViewModel;

        if (_viewModel != null)
            _viewModel.SelectImageRequested += AddImage;
    }

    private async void AddImage()
    {
        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel == null) 
            return;

        
        if (_viewModel == null) return;
        
        var imagesFolder = await topLevel.StorageProvider
            .TryGetFolderFromPathAsync(TaleLoomDataDirectory.Images);

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Select Image",
                AllowMultiple = false,
                SuggestedStartLocation = imagesFolder,
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

            var destination = Path.Combine(TaleLoomDataDirectory.Images, Path.GetFileName(path));

            if (!string.Equals(path, destination, StringComparison.OrdinalIgnoreCase))
            {
                File.Copy(path, destination, overwrite:false);
            }

            
            _viewModel.SetValueAsImage(destination);

        }

    }
        
}