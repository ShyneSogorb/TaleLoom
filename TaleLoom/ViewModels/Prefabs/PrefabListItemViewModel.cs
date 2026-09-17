using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Prefabs;

namespace TaleLoom.ViewModels.Prefabs.Prefabs;

public class PrefabListItemViewModel
{
    public PrefabID ID { get; }
    public string Name { get; }

    
    public PrefabListItemViewModel(PrefabID id, string name)
    {
        ID = id;
        Name = name;
    }
}