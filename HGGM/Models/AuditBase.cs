using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;

namespace HGGM.Models
{
    public abstract class AuditBase
    {
        public ObjectId Id { get; set; }
        public abstract DateTimeOffset Time { get; }
        public abstract string Message { get; }
    }
}
