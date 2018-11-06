using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using LiteDB;
using Serilog;

namespace HGGM.Services
{
    public class DbShrinker
    {
        private readonly LiteDatabase Database;
        private readonly ILogger log = Log.ForContext<DbShrinker>();

        public DbShrinker(LiteDatabase database)
        {
            Database = database;
        }

        public void Shrink()
        {
            var savedBytes = Database.Shrink().Bytes();
            log.Verbose("Database reduced by {size}", savedBytes);
        }
    }
}
