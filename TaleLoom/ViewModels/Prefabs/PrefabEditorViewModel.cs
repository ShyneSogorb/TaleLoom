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
    
    
    private readonly PrefabRepository _repository;

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
    public ObservableCollection<FieldDefinitionViewModel> Fields { get; }

    public ObservableCollection<FieldDefinitionViewModel> ActiveFields
    {
        get => new (Fields.Where(f => f.IsActive));
    }

    public PrefabEditorViewModel(Prefab prefab, PrefabRepository repository)
    {
        _prefab = prefab;
        Fields = new ObservableCollection<FieldDefinitionViewModel>(
            prefab.Fields
                .OrderBy(field => field.Position)
                .Select(field => new FieldDefinitionViewModel(prefab, field))
        );
        _repository = repository;

        AddFieldCommand = new RelayCommand(_ => AddField());

        DeactivateFieldCommand = new RelayCommand(parameter =>
        {
            if (parameter is FieldDefinitionViewModel field)
            {
                field.IsActive = false;
            }
        });

        MoveFieldUpCommand = new RelayCommand(parameter =>
        {
            if (parameter is FieldDefinitionViewModel field)
            {
                MoveFieldUp(field);
            }
        });
        
        MoveFieldDownCommand = new RelayCommand(parameter =>
        {
            if (parameter is FieldDefinitionViewModel field)
            {
                MoveFieldDown(field);
            }
        });

        SavePrefabCommand = new RelayCommand(_ => SavePrefab());
    }
    
    public void AddField(string name = "New Field", FieldType type = FieldType.Name)
    {
        var field = _prefab.GetField(_prefab.AddField(name, type));

        var viewModel = new FieldDefinitionViewModel(_prefab, field);
        
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

    public void MoveFieldUp(FieldDefinitionViewModel field)
    {
        var index = Fields.IndexOf(field);

        if (index <= 0) return;
        
        Fields.Move(index, index-1);
        UpdateFieldOrder();
        
    }

    public void MoveFieldDown(FieldDefinitionViewModel field)
    {
        var index = Fields.IndexOf(field);
        
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