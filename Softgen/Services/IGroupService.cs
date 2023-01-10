using DataAccess.Dtos.Group;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Softgen.Services
{
    public interface IGroupService
    {
        Task<GroupViewDto> AddGroupAsync(GroupCreateDto group, CancellationToken cancellationToken);
        Task<GroupViewDto> GetGroupByIdAsync(int groupId, CancellationToken cancellationToken);
        Task<GroupEntity> GetGroupEntityByIdAsync(int groupId, CancellationToken cancellationToken);
        Task<IEnumerable<GroupViewDto>> GetGroupsAsync(GroupFilterDto groupFilter, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(GroupUpdateDto group, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int groupId, CancellationToken cancellationToken);
        Task<bool> AddStudentToGroupAsync(int groupId, int studentId, CancellationToken cancellationToken);
        Task<bool> AddTeacherToGroupAsync(int groupId, int teacherId, CancellationToken cancellationToken);
        Task<bool> RemoveStudentFromGroupAsync(int groupId, int studentId, CancellationToken cancellationToken);
        Task<bool> RemoveTeacherFromGroupAsync(int groupId, int teacherId, CancellationToken cancellationToken);
    }
}