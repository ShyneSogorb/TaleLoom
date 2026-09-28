namespace TaleLoom.Infrastructure.Persistence;

public partial class TaleLoomRepository
{
    private readonly SqliteDatabase _database;

    public TaleLoomRepository(SqliteDatabase database)
    {
        _database = database;
    }
}