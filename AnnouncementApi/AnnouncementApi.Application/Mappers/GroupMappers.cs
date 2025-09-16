using AnnouncementApi.Application.DTOs.Group;
using AnnouncementApi.Domain.Entities;

namespace AnnouncementApi.Application.Mappers
{
    public static class GroupMappers
    {
        public static Group CreateGroupDtoToGroup(this CreateGroupDto createGroupDto)
        {
            return new Group
            {
                Name = createGroupDto.Name
            };
        }

        public static GroupDto GroupToGroupDto(this Group group)
        {
            return new GroupDto
            {
                Id = group.Id,
                Name = group.Name
            };
        }

        public static GroupDetailDto GroupToGroupDetailDto(this Group group)
        {
            return new GroupDetailDto
            {
                Id = group.Id,
                Name = group.Name,
                Announcements = group.Announcements.Select(x => x.AnnouncementToAnnouncementDto()).ToList(),
                Users = group.GroupUsers.Select(x => x.User.UserToUserDto()).ToList()
            };
        }
    }
}
