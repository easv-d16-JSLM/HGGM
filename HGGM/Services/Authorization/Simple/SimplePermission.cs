﻿using HGGM.Models.Identity;

namespace HGGM.Services.Authorization.Simple
{
    public class SimplePermission : IPermission
    {
        public SimplePermission(SimplePermissionType permission)
        {
            Permission = permission;
        }
        public SimplePermissionType Permission { get; set; }
        public enum SimplePermissionType
        {
            ShowMainMenu,
            Hangfire
        }
    }
}