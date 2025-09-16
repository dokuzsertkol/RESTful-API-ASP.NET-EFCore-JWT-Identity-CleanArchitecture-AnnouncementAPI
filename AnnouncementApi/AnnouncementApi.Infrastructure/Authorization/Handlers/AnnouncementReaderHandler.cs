using AnnouncementApi.Infrastructure.Data;
using AnnouncementApi.Application.Authorization.Requirements;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AnnouncementApi.Infrastructure.Authorization.Handlers
{
    public class AnnouncementReaderHandler : AuthorizationHandler<AnnouncementReaderRequirement>
    {
        private readonly AppDbContext _context;

        public AnnouncementReaderHandler(AppDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AnnouncementReaderRequirement requirement)
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

            var isMember = await _context.Announcements
                .Where(x => x.Id == announcementId)
                .Join(_context.GroupUsers,
                        x => x.GroupId,
                        y => y.GroupId,
                        (x, y) => new { x, y })
                .AnyAsync(x => x.y.UserId == userId);

            if (isMember) context.Succeed(requirement);
            else context.Fail();
        }
    }
}
