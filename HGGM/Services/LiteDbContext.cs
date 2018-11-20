using LiteDB;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace HGGM.Services
{
    public class LiteDbContext : AspNetCore.Identity.LiteDB.Data.LiteDbContext
    {
        public LiteDbContext(IHostingEnvironment environment, LiteRepository repo) : base(environment)
        {
            LiteDatabase = repo.Database;
            LiteDatabase.Log.Level = Logger.FULL;
            LiteDatabase.Log.Logging += msg => Log.ForContext<LiteDatabase>().Debug("{dbMessage}", msg);
            
        }
    }
}