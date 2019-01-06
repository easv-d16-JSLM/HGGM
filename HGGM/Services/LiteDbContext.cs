using LiteDB;
using Serilog;

namespace HGGM.Services
{
    public class LiteDbContext : AspNetCore.Identity.LiteDB.Data.ILiteDbContext
    {
        public LiteDbContext(LiteRepository repo)
        {
            LiteDatabase = repo.Database;
            LiteDatabase.Log.Level = Logger.FULL;
            LiteDatabase.Log.Logging += msg => Log.ForContext<LiteDatabase>().Debug("{dbMessage}", msg);
        }

        public LiteDatabase LiteDatabase { get; }
    }
}