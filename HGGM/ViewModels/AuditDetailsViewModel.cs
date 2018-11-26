using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models;

namespace HGGM.ViewModels
{
    public class AuditDetailsViewModel : AuditEntryBase
    {
        public Dictionary<string,string> Properties { get; set; }
        public string message;
        public override string Message => message;
    }
}
