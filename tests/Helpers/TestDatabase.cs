// tests/Helpers/TestDatabase.cs
using SQLite;
using StoreApp.Models;
using StoreApp.src.Services;

public static class TestDatabase
{
    public static DatabaseService CreateInMemoryDb()
    {
        // Each call gets a unique file — guaranteed isolation
        var tempPath = Path.Combine(
            Path.GetTempPath(),
            $"test_{Guid.NewGuid()}.db");
        return new DatabaseService(tempPath);
    }
}