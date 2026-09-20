using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.Services;
using TaleLoom.ViewModels.Prefabs;

namespace TaleLoom.ViewModels.Entities;

public class PrefabCategoryContainer
{
    public Prefab _prefab { get; }
    public List<EntitiesListItemViewModel> _items { get; }

    public string Name { get => _prefab.Name; }
    public IReadOnlyList<EntitiesListItemViewModel> Items { get => _items; }

    public PrefabCategoryContainer(Prefab prefab, IEnumerable<EntitiesListItemViewModel> items)
    {
        _prefab = prefab;
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
                    prefab,
                    _entityRepository.GetAllEntities(prefab)
                    .Select(entity => new EntitiesListItemViewModel(entity.Id, entity.Name, prefab.ID))
                )
            )
        );

        EditEntityCommand = new RelayCommand(EditEntity);

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
    
}