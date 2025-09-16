using AnnouncementApi.Infrastructure.Data;
using AnnouncementApi.Application.Authorization.Requirements;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AnnouncementApi.Infrastructure.Authorization.Handlers
{
    public class GroupMemberHandler : AuthorizationHandler<GroupMemberRequirement>
    {
        private readonly AppDbContext _context;

        public GroupMemberHandler(AppDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupMemberRequirement requirement)
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
            if (!routeValues.TryGetValue("groupId", out var groupIdObj) || !int.TryParse(groupIdObj.ToString(), out var groupId))
            {
                context.Fail();
                return;
            }

            var isMember = await _context.GroupUsers
                .AnyAsync(x => x.GroupId == groupId && x.UserId == userId);

            if (isMember) context.Succeed(requirement);
            else context.Fail();
        }
    }
}
