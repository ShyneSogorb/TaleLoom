using Avalonia.Controls;
using Avalonia.Controls.Templates;
using TaleLoom.Core.Model.Fields;

namespace TaleLoom.ViewModels.Entities;

public class EntityFieldTemplateSelector : IDataTemplate
{
    public IDataTemplate NameTemplate { get; set; } = null;
    public IDataTemplate IntegerTemplate { get; set; } = null;
    public IDataTemplate FloatTemplate { get; set; } = null;
    public IDataTemplate TextTemplate {get; set; } = null;
    public IDataTemplate ImageTemplate {get; set; } = null;
    public IDataTemplate UrlTemplate {get; set; } = null;
    public IDataTemplate ReferenceTemplate {get; set; } = null;

    public Control? Build(object? param)
    {
        if (param is not EntityFieldEditorViewModel field)
            return null;

        IDataTemplate? template = field.Field.Type switch
        {
            FieldType.Name      => NameTemplate,
            FieldType.Integer   => IntegerTemplate,
            FieldType.Float     => FloatTemplate,
            FieldType.Text      => TextTemplate,
            FieldType.Image     => ImageTemplate,
            FieldType.Url       => UrlTemplate,
            FieldType.Reference => ReferenceTemplate,
            _ => null
        };
        
        return template?.Build(param);
    }

    public bool Match(object? data)
    {
        return data is EntityFieldEditorViewModel;
    }
}