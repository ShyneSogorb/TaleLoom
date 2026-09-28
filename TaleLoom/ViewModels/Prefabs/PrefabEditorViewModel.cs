using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Infrastructure.Persistence;

namespace TaleLoom.ViewModels.Prefabs;



public class PrefabEditorViewModel : ViewModelBase
{
   
    private readonly Prefab _prefab;

    public PrefabID Id => _prefab.Id;
    
    
    private readonly TaleLoomRepository _repository;

    public string Name
    {
        get => _prefab.Name;
        set
        {
            if (_prefab.Name == value)
            {
                return;
            }
            
            _prefab.Rename(value);
            OnPropertyChanged();
        }
    } 
    public ObservableCollection<PrefabFieldDefinitionViewModel> Fields { get; }

    public ObservableCollection<PrefabFieldDefinitionViewModel> ActiveFields
    {
        get => new (Fields.Where(f => f.IsActive));
    }

    public PrefabEditorViewModel(Prefab prefab, TaleLoomRepository repository)
    {
        _prefab = prefab;
        _repository = repository;
        Fields = new ObservableCollection<PrefabFieldDefinitionViewModel>(
            prefab.Fields
                .OrderBy(field => field.Position)
                .Select(field => new PrefabFieldDefinitionViewModel(prefab, field, _repository))
        );

        AddFieldCommand = new RelayCommand(_ => AddField());

        DeactivateFieldCommand = new RelayCommand(parameter =>
        {
            if (parameter is PrefabFieldDefinitionViewModel field)
            {
                field.IsActive = false;
            }
        });

        MoveFieldUpCommand = new RelayCommand(parameter =>
        {
            if (parameter is PrefabFieldDefinitionViewModel field)
            {
                MoveFieldUp(field);
            }
        });
        
        MoveFieldDownCommand = new RelayCommand(parameter =>
        {
            if (parameter is PrefabFieldDefinitionViewModel field)
            {
                MoveFieldDown(field);
            }
        });

        SavePrefabCommand = new RelayCommand(_ => SavePrefab());
    }
    
    public void AddField(string name = "New Field", FieldType type = FieldType.Name)
    {
        var field = _prefab.GetField(_prefab.AddField(name, type));

        var viewModel = new PrefabFieldDefinitionViewModel(_prefab, field, _repository);
        
        ActiveFields.Add(viewModel);
        Fields.Add(viewModel);
    }

    public void RemoveField(FieldId fieldId)
    {
        _prefab.RemoveField(fieldId);

        var viewModel = Fields.FirstOrDefault(x => x.Id == fieldId);

        if (viewModel != null)
        {
            ActiveFields.Remove(viewModel);
            //Fields.Remove(viewModel);
        }
    }

    public void MoveFieldUp(PrefabFieldDefinitionViewModel prefabField)
    {
        var index = Fields.IndexOf(prefabField);

        if (index <= 0) return;
        
        Fields.Move(index, index-1);
        UpdateFieldOrder();
        
    }

    public void MoveFieldDown(PrefabFieldDefinitionViewModel prefabField)
    {
        var index = Fields.IndexOf(prefabField);
        
        if (index < 0 || index >= Fields.Count - 1) return;
        
        Fields.Move(index, index+1);

        UpdateFieldOrder();
    }

    private void UpdateFieldOrder()
    {
        for (int i = 0; i < Fields.Count; i++)
        {
            Fields[i].SetOrder(i+1);
        }
    }
    
    private void SavePrefab()
    {
        _repository.Save(_prefab);
    }
    
    public ICommand SavePrefabCommand { get; }
    public ICommand AddFieldCommand { get; }
    public ICommand DeactivateFieldCommand { get; }
    public ICommand MoveFieldUpCommand { get; }
    public ICommand MoveFieldDownCommand { get; }

}