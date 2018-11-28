using LiteDB;
using Microsoft.AspNetCore.Authorization;

namespace HGGM.Services.Authorization.Tag
{
    public class TagRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// The tag requirement for the given event.
        /// </summary>
        [BsonRef]
        public Models.Tag Tag { get; set; }
    }
}