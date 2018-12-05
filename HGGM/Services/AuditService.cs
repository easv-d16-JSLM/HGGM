using System;
using System.Collections.Generic;
using System.Linq;
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

        public AuditEntryBase Get(Guid id)
        {
            return db.SingleById<AuditEntryBase>(id);
        }

        public List<AuditEntryBase> GetAll()
        {
            return db.Fetch<AuditEntryBase>().OrderByDescending(a => a.Time).ToList();
        }

        public List<T> GetAll<T>() where T : AuditEntryBase
        {
            return db.Fetch<AuditEntryBase>().OfType<T>().OrderByDescending(a => a.Time).ToList();
        }

        public LiteQueryable<AuditEntryBase> Query()
        {
            return db.Query<AuditEntryBase>();
        }
    }
}