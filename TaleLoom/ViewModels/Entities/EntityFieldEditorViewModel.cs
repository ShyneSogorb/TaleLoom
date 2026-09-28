using System.Collections.Generic;
using System.Linq;
using Avalonia.Media.Imaging;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Core.Model.Values;
using TaleLoom.Infrastructure.Persistence;

namespace TaleLoom.ViewModels.Entities;

public class EntityFieldEditorViewModel : ViewModelBase
{
    protected Entity.FieldEntity FieldEntity;
    protected bool IsPrefabEditor;

    public FieldDefinition Field
    {
        get => FieldEntity.Definition;
    }

    public ValueBase ValueContainer
    {
        get => FieldEntity.Value;
    }
    
    public object? Value
    {
        get => FieldEntity.Value.GetData();
        set
        {
            if (FieldEntity.Value.GetData() == value) return;
            FieldEntity.Value.SetData(value);
            OnPropertyChanged();
        }
    }

    public double? ValueAsDouble
    {
        get
        {
            if (FieldEntity.Value.TryGet(out double value))
            {
                return value;
            }
            return null;
        }
        set => FieldEntity.Value.SetData(value);
    }

    public Bitmap? ValueAsImage
    {
        get
        {
            if (FieldEntity.Value.TryGet(out string value))
            {
                return new Bitmap(value);
            }
            return null;
        }
    }

    public void SetValueAsImage(string value)
    {
        if (FieldEntity.Value.IsValid && FieldEntity.Value.Get<string>() == value) return;
        FieldEntity.Value.SetData(value);
        OnPropertyChanged(nameof(ValueAsImage));
    }

    public EntityFieldEditorViewModel(Entity.FieldEntity fieldEntity, bool isPrefabEditor)
    {
        FieldEntity = fieldEntity;
        IsPrefabEditor = isPrefabEditor;
    }
}