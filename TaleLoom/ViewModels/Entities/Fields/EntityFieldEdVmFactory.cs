using System;
using System.Runtime.CompilerServices;
using TaleLoom.Core.Model.Common;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Infrastructure.Files;
using TaleLoom.Infrastructure.Persistence;

namespace TaleLoom.ViewModels.Entities.Fields;

public static class EntityFieldEdVmFactory
{
    public static EntityFieldEditorViewModel Create(Entity.FieldEntity field, TaleLoomRepository repo, bool modifyingPrefab)
    {
        if (field.Definition.Type == FieldType.Image)
        {
            return new ImageFieldEditorViewModel(field, modifyingPrefab);
        }
        
        if (field.Definition.Type == FieldType.Reference)
        {
            if (modifyingPrefab || field.Definition.DefaultValue is null)
            {
                return new ReferenceFieldEditorViewModel(field, modifyingPrefab, repo.GetAllPrefabsSimple());
            }
            
            return new ReferenceFieldEditorViewModel(field, modifyingPrefab, repo.GetAllEntitiesSimple(
                new PrefabID(Guid.Parse(field.Definition.DefaultValue))
            ));
        }

        return new EntityFieldEditorViewModel(field, modifyingPrefab);
    }
}