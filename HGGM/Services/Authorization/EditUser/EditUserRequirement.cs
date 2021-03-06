﻿using Microsoft.AspNetCore.Authorization;

namespace HGGM.Services.Authorization.EditUser
{
    public class EditUserRequirement : IAuthorizationRequirement
    {
        public EditUserRequirement(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}