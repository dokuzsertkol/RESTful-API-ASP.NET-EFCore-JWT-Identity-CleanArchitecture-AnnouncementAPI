using Microsoft.AspNetCore.Authorization;

namespace AnnouncementApi.Application.Authorization.Requirements
{
    public class GroupAdminRequirement : IAuthorizationRequirement
    {
        public GroupAdminRequirement() { }
    }
}
