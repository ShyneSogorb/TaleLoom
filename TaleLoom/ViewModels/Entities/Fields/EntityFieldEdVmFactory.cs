using TaleLoom.Core.Model.Entities;
using TaleLoom.Core.Model.Fields;
using TaleLoom.Infrastructure.Files;

namespace TaleLoom.ViewModels.Entities.Fields;

public static class EntityFieldEdVmFactory
{
    public static EntityFieldEditorViewModel Create(Entity.FieldEntity field)
    {
        if (field.Definition.Type == FieldType.Image)
        {
            return new ImageFieldEditorViewModel(field);
        }

        return new EntityFieldEditorViewModel(field);
    }
}