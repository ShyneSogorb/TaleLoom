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
    private Entity.FieldEntity _fieldEntity;

    public FieldDefinition Field
    {
        get => _fieldEntity.Definition;
    }

    public ValueBase ValueContainer
    {
        get => _fieldEntity.Value;
    }
    
    public object? Value
    {
        get => _fieldEntity.Value.GetData();
        set
        {
            if (_fieldEntity.Value.GetData() == value) return;
            _fieldEntity.Value.SetData(value);
            OnPropertyChanged();
        }
    }

    public double? ValueAsDouble
    {
        get
        {
            if (_fieldEntity.Value.TryGet(out double value))
            {
                return value;
            }
            return null;
        }
        set => _fieldEntity.Value.SetData(value);
    }

    public Bitmap? ValueAsImage
    {
        get
        {
            if (_fieldEntity.Value.TryGet(out string value))
            {
                return new Bitmap(value);
            }
            return null;
        }
    }

    public void SetValueAsImage(string value)
    {
        if (_fieldEntity.Value.IsValid && _fieldEntity.Value.Get<string>() == value) return;
        _fieldEntity.Value.SetData(value);
        OnPropertyChanged(nameof(ValueAsImage));
    }

    public EntityFieldEditorViewModel(Entity.FieldEntity fieldEntity)
    {
        _fieldEntity = fieldEntity;
    }
}