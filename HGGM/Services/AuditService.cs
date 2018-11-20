using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models;
using LiteDB;

namespace HGGM.Services
{
    public class AuditService
    {
        private readonly LiteRepository db;

        public AuditService(LiteRepository db)
        {
            this.db = db;
        }

        public void Add(AuditEntryBase item)
        {
            db.Insert(item);
        }

        public IList<AuditEntryBase> GetAll()
        {
            return db.Fetch<AuditEntryBase>();
        }

        public LiteQueryable<AuditEntryBase> Query()
        {
            return db.Query<AuditEntryBase>();
        }
    }
}
