using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HGGM.Services.Authorization.Tag
{
    public class TagHandler : AuthorizationHandler<TagRequirement>
    {
        private readonly LiteRepository _db;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public TagHandler(LiteRepository db, UserManager<User> userManager, ILogger logger)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TagRequirement requirement)
        {
            var user = await _userManager.GetUserAsync(context.User);


            // write logic for checking if tag-requirement is forfilled
        }
    }
}
