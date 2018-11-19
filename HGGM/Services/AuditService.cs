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

        public void Add(AuditBase item)
        {
            db.Insert(item);
        }

        public IList<AuditBase> GetAll()
        {
            return db.Fetch<AuditBase>();
        }

        public LiteQueryable<AuditBase> Query()
        {
            return db.Query<AuditBase>();
        }
    }
}
