namespace TaleLoom.Core.Model.Common;

public interface IDatabaseEntity
{
    static abstract string Table { get; }
}