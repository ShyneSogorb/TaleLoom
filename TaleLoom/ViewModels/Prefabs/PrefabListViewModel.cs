using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.Services;

namespace TaleLoom.ViewModels.Prefabs;

public class PrefabListViewModel : ViewModelBase
{

    private readonly INavigationService _navigationService;
    private readonly PrefabRepository _repository;
    public ObservableCollection<PrefabListItemViewModel> Prefabs { get; }
    
    private readonly Action<PrefabID> _editPrefab;
    private readonly Action _createPrefab;
    
    public ICommand EditPrefabCommand { get; }
    public ICommand CreatePrefabCommand { get; }
    public ICommand DeletePrefabCommand { get; }
    
    public PrefabListViewModel(
        INavigationService navigationService,
        PrefabRepository repository
        )
    {
        _repository = repository;
        _navigationService = navigationService;
        
        Prefabs = new ObservableCollection<PrefabListItemViewModel>(
            repository.GetAllPrefabs().Select(prefab => 
                new PrefabListItemViewModel(prefab.Id, prefab.Name))
        );

        EditPrefabCommand = new RelayCommand(EditPrefab);
        DeletePrefabCommand = new RelayCommand(DeletePrefab);

        CreatePrefabCommand = new RelayCommand(_ => CreatePrefab());
        
    }
    
    private void EditPrefab(object? parameter)
    {
        if (parameter is not PrefabListItemViewModel prefabItem)
            return;

        var prefab = _repository.GetPrefabById(prefabItem.ID);
        //var prefab = Prefab.CreateTransientPrefab("Any");

        _navigationService.Navigate(
            new PrefabEditorViewModel(
                prefab,
                _repository));
    }
    
    private void DeletePrefab(object? parameter)
    {
        if (parameter is not PrefabListItemViewModel prefabItem)
            return;

        _repository.DeletePrefab(prefabItem.ID);
    }

    private void CreatePrefab()
    {
        var prefab = Prefab.CreateTransientPrefab("New Prefab");

        _navigationService.Navigate(
            new PrefabEditorViewModel(
                prefab,
                _repository));
    }
}