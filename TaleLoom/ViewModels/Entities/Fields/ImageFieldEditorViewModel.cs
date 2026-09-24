using System;
using System.Windows.Input;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Infrastructure.Files;
using TaleLoom.ViewModels.Prefabs;

namespace TaleLoom.ViewModels.Entities.Fields;

public class ImageFieldEditorViewModel : EntityFieldEditorViewModel
{
    public ICommand SelectImageCommand { get; }
    public event Action? SelectImageRequested;
    

    public ImageFieldEditorViewModel(Entity.FieldEntity fieldEntity) : base(fieldEntity)
    {
        SelectImageCommand = new RelayCommand(
            _ => SelectImageRequested?.Invoke());
    }
}