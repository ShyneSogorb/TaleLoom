using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Core.Model.Prefabs;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.ViewModels.Prefabs;

namespace TaleLoom.ViewModels.Entities;

public class EntityEditorViewModel : ViewModelBase
{
    private readonly Prefab _prefab;

    private readonly Entity _entity;

    private readonly EntityRepository _entityRepository;

    public IReadOnlyList<EntityFieldEditorViewModel> Fields { get; }
    
    public ICommand SaveEntityCommand { get; }

    public string Name
    {
        get => _entity.Name;
        set {
            if (Name == value) return;
            
            _entity.Rename(value);
            OnPropertyChanged();
        }
    }

    public EntityEditorViewModel(Prefab prefab, Entity entity, EntityRepository entityRepository)
    {
        _prefab = prefab;
        _entity = entity;
        _entityRepository = entityRepository;

        Fields = entity.FieldsData
            .Select(f => new EntityFieldEditorViewModel(f))
            .ToList();

        SaveEntityCommand = new RelayCommand(_ => SaveEntity());
    }

    private void SaveEntity()
    {
        _entityRepository.SaveEntity(_entity);
    }
    
}