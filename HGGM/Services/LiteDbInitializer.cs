using System.Collections.Generic;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using LiteDB;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace HGGM.Services
{
    public class LiteDbInitializer
    {
        private readonly ILogger log = Log.ForContext<LiteDbInitializer>();

        public LiteDbInitializer(LiteDatabase db, UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            db.Log.Level = Logger.FULL;
            db.Log.Logging += msg => Log.ForContext<LiteDatabase>().Verbose("{msg}", msg);
            db.Engine.DropCollection("users");
            Initialize(new LiteRepository(db), userManager, roleManager).GetAwaiter().GetResult();
            
        }

        private async Task CreateUsers(UserManager<User> userManager)
        {
            var results = new List<IdentityResult>
            {
                await userManager.CreateAsync(new User {Email = "root@localhost", UserName = "root"}, "ASDfghjkl123.")
            };
            results.ForEach(r =>
            {
                if (r.Succeeded) return;
                foreach (var error in r.Errors)
                    log.Error("User Creation failed with code {code}: {message}", error.Code, error.Description);
            });
        }

        private async Task Initialize(LiteRepository repository, UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            await CreateUsers(userManager);
        }
    }
}