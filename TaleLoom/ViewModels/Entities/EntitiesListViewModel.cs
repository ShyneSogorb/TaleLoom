using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.Services;
using TaleLoom.ViewModels.Prefabs;

namespace TaleLoom.ViewModels.Entities;

public class PrefabCategoryContainer
{
    public PrefabID PrefabId { get; }
    
    public List<EntitiesListItemViewModel> _items { get; }

    public string Name { get; }
    public IReadOnlyList<EntitiesListItemViewModel> Items { get => _items; }

    public PrefabCategoryContainer(PrefabID prefab, string name, IEnumerable<EntitiesListItemViewModel> items)
    {
        PrefabId = prefab;
        Name = name;
        _items = items.ToList();
    }
}

public class EntitiesListViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly EntityRepository _entityRepository;
    private readonly PrefabRepository _prefabRepository;
    public ObservableCollection<PrefabCategoryContainer> Entities { get; }
    
    public ICommand EditEntityCommand { get; }
    public ICommand CreateEntityCommand { get; }
    
    public EntitiesListViewModel(
        INavigationService navigationService,
        EntityRepository entityRepository,
        PrefabRepository prefabRepository)
    {
        _navigationService = navigationService;
        _entityRepository = entityRepository;
        _prefabRepository = prefabRepository;
        
        Entities = new ObservableCollection<PrefabCategoryContainer>(
            _prefabRepository.GetAllPrefabs()
            .Select(
                prefab => new PrefabCategoryContainer(
                    prefab.Id,
                    prefab.Name,
                    _entityRepository.GetAllEntities(prefab)
                    .Select(entity => new EntitiesListItemViewModel(entity.Id, entity.Name, prefab.Id))
                )
            )
        );

        EditEntityCommand = new RelayCommand(EditEntity);
        CreateEntityCommand = new RelayCommand(NewEntity);
    }

    void EditEntity(object? param)
    {
        if (param is not EntitiesListItemViewModel entityItem) return;

        var prefab = _prefabRepository.GetPrefabById(entityItem.ParentID);
        var entity = _entityRepository.GetEntityById(prefab, entityItem.ID);
        
        _navigationService.Navigate(
            new EntityEditorViewModel(prefab, entity, _entityRepository)
            );
        
    }

    void NewEntity(object? param)
    {
        if (param is not PrefabCategoryContainer prefabCategoryContainer) return;

        var prefab = _prefabRepository.GetPrefabById(prefabCategoryContainer.PrefabId);
        
        _navigationService.Navigate(
            new EntityEditorViewModel(prefab, Entity.Instantiate(prefab), _entityRepository)
        );

    }
    
}