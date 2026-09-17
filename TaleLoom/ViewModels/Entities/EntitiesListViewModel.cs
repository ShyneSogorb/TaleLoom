using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.Services;
using TaleLoom.ViewModels.Prefabs.Prefabs;

namespace TaleLoom.ViewModels.Prefabs.Entities;

public class PrefabCategoryContainer
{
    public Prefab _prefab { get; }
    public List<EntitiesListItemViewModel> _items { get; }

    public PrefabCategoryContainer(Prefab prefab, IEnumerable<EntitiesListItemViewModel> items)
    {
        _prefab = prefab;
        _items = items.ToList();
    }
}

public class EntitiesListViewModel
{
    private readonly INavigationService _navigationService;
    private readonly EntityRepository _entityRepository;
    private readonly PrefabRepository _prefabRepository;
    public ObservableCollection<PrefabCategoryContainer> Entities { get; }
    
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
                    .Select(entity => new EntitiesListItemViewModel(entity.Id, entity.Name))
                )
            )
        );
    }
    
}