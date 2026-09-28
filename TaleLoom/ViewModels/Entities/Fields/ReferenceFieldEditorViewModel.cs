using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Infrastructure.Files;
using TaleLoom.ViewModels.Prefabs;

namespace TaleLoom.ViewModels.Entities.Fields;

public class ReferenceFieldEditorViewModel : EntityFieldEditorViewModel
{

    private readonly List<ObjectIdentifier> _options;

    public IReadOnlyList<ObjectIdentifier> Options => _options;

    private ObjectIdentifier _selectedIdentifier;
    
    public IEnumerable<string> Ids => _options.Select(obj => obj.Id);
    public IEnumerable<string> Names => _options.Select(obj => obj.Name);


    public ObjectIdentifier SelectedIdentifier
    {
        get => _selectedIdentifier;
        set
        {
            if (_selectedIdentifier.Id.Equals(value.Id)) return;

            _selectedIdentifier = value;
            FieldEntity.Value.SetData(value.Id);
            OnPropertyChanged();
        }
    }
    
    public ReferenceFieldEditorViewModel(Entity.FieldEntity fieldEntity, bool isPrefabEditor, List<ObjectIdentifier> options) : base(fieldEntity, isPrefabEditor)
    {
        _options = options;
        _options.Insert(0, new ObjectIdentifier("", "None"));

        ObjectIdentifier? identifier = null; 
        if (isPrefabEditor)
        {
            identifier = options.FirstOrDefault(elem =>
                fieldEntity.Definition.DefaultValue is not null && fieldEntity.Definition.DefaultValue.Equals(elem.Id)
            );
        }
        else
        {
            identifier = options.FirstOrDefault(elem =>
                fieldEntity.Value.IsValid && fieldEntity.Value.Get<string>().Equals(elem.Id)
            );
        }

        _selectedIdentifier = identifier ?? options[0];
    }
}