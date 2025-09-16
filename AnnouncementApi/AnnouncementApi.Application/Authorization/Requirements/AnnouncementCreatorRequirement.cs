using Microsoft.AspNetCore.Authorization;

namespace AnnouncementApi.Application.Authorization.Requirements
{
    public class AnnouncementCreatorRequirement : IAuthorizationRequirement
    {
        public AnnouncementCreatorRequirement() { }
    }
}
