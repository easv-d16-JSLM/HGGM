using System;
using System.Collections.Generic;
using System.Linq;
using HGGM.Models.Identity;

namespace HGGM.Services.Authorization.Simple
{
    public class SimplePermission : IPermission, IEquatable<SimplePermission>
    {
        public enum SimplePermissionType
        {
            ShowMainMenu,
            Hangfire
        }

        /// <summary>
        ///     Used by LiteDb
        /// </summary>
        [Obsolete]
        public SimplePermission()
        {
        }

        public SimplePermission(SimplePermissionType permission)
        {
            Permission = permission;
        }

        public static List<SimplePermission> GetAllSimplePermissions =>
            Enum.GetValues(typeof(SimplePermissionType)).Cast<SimplePermissionType>()
                .Select(p => new SimplePermission(p)).ToList();

        public SimplePermissionType Permission { get; set; }

        public bool Equals(SimplePermission other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Permission == other.Permission;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SimplePermission) obj);
        }

        public override int GetHashCode()
        {
            return Permission.GetHashCode();
        }
    }
}