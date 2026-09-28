using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Core.Model.Values;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.ViewModels.Entities;
using TaleLoom.ViewModels.Entities.Fields;

namespace TaleLoom.ViewModels.Prefabs;

public class PrefabFieldDefinitionViewModel : ViewModelBase
{
    private readonly Prefab _prefab;
    private readonly FieldDefinition _field;
    private readonly Entity.FieldEntity _entityField;
    private readonly EntityFieldEditorViewModel _defaultValue;
    
    public IEnumerable<EntityFieldEditorViewModel> DefaultValue
    {
        get => [_defaultValue];
    }
    
    
    
    public FieldId Id => _field.Id;
    
    public string Name
    {
        get => _field.Name;
        set
        {
            if (_field.Name == value) 
                return;
            
            _prefab.RenameField(_field.Id, value);
            OnPropertyChanged();
        }
    }
    
    public FieldType Type
    {
        get => _field.Type;

        set
        {
            if (_field.Type == value)
            {
                return;
            }
            
            _field.ChangeType(value);
            OnPropertyChanged();
        }

    }

    public bool IsActive
    {
        get => _field.IsActive;
        set
        {
            if (_field.IsActive == value) 
                return;

            if (value)
            {
                _field.Restore();
            }
            else
            {
                _field.Deactivate();
            }
            OnPropertyChanged();
        }
    }
    
    public bool IsRequired
    {
        get => _field.IsRequired;
        set
        {
            if (_field.IsRequired == value) 
                return;

            if (value)
            {
                _field.MarkAsRequired();
            }
            else
            {
                _field.MarkAsOptional();
            }
            OnPropertyChanged();
        }
    }

    public int Order => _field.Position;
    
    public void SetOrder(int order)
    {
        _field.SetOrder(order);

        OnPropertyChanged(
            nameof(Order));
    }
    
    public ObservableCollection<FieldType> AvailableTypes { get; }

    public PrefabFieldDefinitionViewModel(Prefab prefab, FieldDefinition field, TaleLoomRepository repo)
    {
        _prefab = prefab;
        _field = field;

        _entityField = new Entity.FieldEntity(field, ValueFactory.Create(field.Type, null));

        var defaultValue = _entityField.Definition.DefaultValue;
        _defaultValue = EntityFieldEdVmFactory.Create(_entityField, repo, true);
            

        _defaultValue.PropertyChanged += (_, _) => _field.SetDefaultValue(_entityField.Value.Get<string>());
        
        AvailableTypes = new ObservableCollection<FieldType>(Enum.GetValues<FieldType>());
    }

}