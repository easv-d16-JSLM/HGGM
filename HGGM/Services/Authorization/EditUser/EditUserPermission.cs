using System.Collections.Generic;
using HGGM.Models.Identity;

namespace HGGM.Services.Authorization.EditUser
{
    public class EditUserPermission : IPermission
    {
        public string PropertyName { get; set; }
        public IList<Role> Roles { get; set; }
    }
}