using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;

namespace HGGM.Services.Authorization.EditUser
{
    public class EditUserPermission : IPermission
    {
        public User User { get; set; }
    }
}
