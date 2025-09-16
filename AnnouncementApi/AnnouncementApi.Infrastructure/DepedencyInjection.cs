using AnnouncementApi.Application.Interfaces;
using AnnouncementApi.Infrastructure.Data;
using AnnouncementApi.Repositories;
using AnnouncementApi.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AnnouncementApi.Infrastructure.Authorization.Handlers;
using Microsoft.AspNetCore.Authorization;
using AnnouncementApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using AnnouncementApi.Application.Authorization.Requirements;

namespace AnnouncementApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // for database
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            // for identity
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 9;
            }).AddEntityFrameworkStores<AppDbContext>();

            // for authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("GroupAdmin", policy =>
                    policy.Requirements.Add(new GroupAdminRequirement()));
                options.AddPolicy("GroupMember", policy =>
                    policy.Requirements.Add(new GroupMemberRequirement()));
                options.AddPolicy("AnnouncementReader", policy =>
                    policy.Requirements.Add(new AnnouncementReaderRequirement()));
                options.AddPolicy("AnnouncementCreator", policy =>
                    policy.Requirements.Add(new AnnouncementCreatorRequirement()));
            });

            // adding scopes
            services.AddScoped<IAuthorizationHandler, GroupAdminHandler>();
            services.AddScoped<IAuthorizationHandler, GroupMemberHandler>();
            services.AddScoped<IAuthorizationHandler, AnnouncementReaderHandler>();
            services.AddScoped<IAuthorizationHandler, AnnouncementCreatorHandler>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAnnouncementService, AnnouncementService>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<IGroupUserRepository, GroupUserRepository>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
