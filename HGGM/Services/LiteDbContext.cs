using LiteDB;
using Microsoft.AspNetCore.Hosting;

namespace HGGM.Services
{
    public class LiteDbContext : AspNetCore.Identity.LiteDB.Data.LiteDbContext
    {
        public LiteDbContext(IHostingEnvironment environment, LiteRepository repo) : base(environment)
        {
            LiteDatabase = repo.Database;
        }
    }
}