using Microsoft.AspNetCore.Authorization;

namespace AnnouncementApi.Application.Authorization.Requirements
{
    public class GroupMemberRequirement : IAuthorizationRequirement
    {
        public GroupMemberRequirement() { }
    }
}
