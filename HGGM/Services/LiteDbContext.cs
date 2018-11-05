using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using LiteDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace HGGM.Services
{
    public class LiteDbContext : AspNetCore.Identity.LiteDB.Data.LiteDbContext
    {

        public LiteDbContext(IHostingEnvironment environment, LiteDatabase db) : base(environment)
        {
            LiteDatabase = db;
        }
    }
}
