using AnnouncementApi.Infrastructure.Data;
using AnnouncementApi.Application.Authorization.Requirements;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AnnouncementApi.Infrastructure.Authorization.Handlers
{
    public class AnnouncementCreatorHandler : AuthorizationHandler<AnnouncementCreatorRequirement>
    {
        private readonly AppDbContext _context;

        public AnnouncementCreatorHandler(AppDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AnnouncementCreatorRequirement requirement)
        {
            if (context.Resource is not HttpContext httpContext)
            {
                context.Fail();
                return;
            }

            var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                context.Fail();
                return;
            }

            var routeValues = httpContext.GetRouteData().Values;
            if (!routeValues.TryGetValue("announcementId", out var announcementIdObj) || !int.TryParse(announcementIdObj?.ToString(), out var announcementId))
            {
                context.Fail();
                return;
            }

            var isCreator = await _context.Announcements
                .AnyAsync(x => x.Id == announcementId && x.UserId == userId);

            if (isCreator) context.Succeed(requirement);
            else context.Fail();
        }
    }
}
