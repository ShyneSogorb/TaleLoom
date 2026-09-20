using TaleLoom.Core.Model.Common;

namespace TaleLoom.ViewModels.Entities;

public class EntitiesListItemViewModel
{
    public EntityId ID { get; }
    public PrefabID ParentID { get; }
    public string Name { get; }

    
    public EntitiesListItemViewModel(EntityId id, string name, PrefabID parentId)
    {
        ID = id;
        Name = name;
        ParentID = parentId;
    }
}