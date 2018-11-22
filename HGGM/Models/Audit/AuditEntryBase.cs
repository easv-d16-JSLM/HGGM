using System;

namespace HGGM.Models
{
    public abstract class AuditEntryBase
    {
        public Guid Id { get; set; }
        public abstract string Message { get; }
        public virtual DateTimeOffset Time { get; set; } = DateTimeOffset.Now;
    }
}