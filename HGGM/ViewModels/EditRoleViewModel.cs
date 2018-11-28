using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.ViewModels
{
    public class EditRoleViewModel
    {
        public string Name { get; set; }
        public Dictionary<string, bool> SimplePermissions { get; set; }
    }
}
