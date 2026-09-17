using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;

namespace TaleLoom.ViewModels.Prefabs;

public class FieldDefinitionViewModel : ViewModelBase
{
    private readonly Prefab _prefab;
    private readonly FieldDefinition _field;

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
            OnPropertyChanged(nameof(IsActive));
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

    public FieldDefinitionViewModel(Prefab prefab, FieldDefinition field)
    {
        _prefab = prefab;
        _field = field;

        AvailableTypes = new ObservableCollection<FieldType>(Enum.GetValues<FieldType>());
    }

}