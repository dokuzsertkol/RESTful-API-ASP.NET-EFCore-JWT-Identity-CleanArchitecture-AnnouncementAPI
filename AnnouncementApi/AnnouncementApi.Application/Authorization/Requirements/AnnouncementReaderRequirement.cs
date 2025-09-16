using Microsoft.AspNetCore.Authorization;

namespace AnnouncementApi.Application.Authorization.Requirements
{
    public class AnnouncementReaderRequirement : IAuthorizationRequirement
    {
        public AnnouncementReaderRequirement() { }
    }
}
