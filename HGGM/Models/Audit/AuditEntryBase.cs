using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;

namespace HGGM.Models
{
    public abstract class AuditEntryBase
    {
        public Guid Id { get; set; }
        public virtual DateTimeOffset Time { get; } = DateTimeOffset.Now;
        public abstract string Message { get; }
    }
}
