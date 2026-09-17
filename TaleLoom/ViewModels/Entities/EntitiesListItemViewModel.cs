using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TaleLoom.Core.Model.Common;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.Services;
using TaleLoom.ViewModels.Prefabs.Prefabs;

namespace TaleLoom.ViewModels.Prefabs.Entities;

public class EntitiesListItemViewModel
{
    public EntityId ID { get; }
    public string Name { get; }

    
    public EntitiesListItemViewModel(EntityId id, string name)
    {
        ID = id;
        Name = name;
    }
}