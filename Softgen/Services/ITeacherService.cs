using DataAccess.Dtos.Teacher;
using DataAccess.Entities;

namespace Softgen.Services
{
    public interface ITeacherService
    {
        Task<TeacherViewDto> AddTeacherAsync(TeacherCreateDto teacher, CancellationToken cancellationToken);
        Task<TeacherViewDto> GetTeacherByIdAsync(int teacherId, CancellationToken cancellationToken);
        Task<TeacherEntity> GetTeacherEntityByIdAsync(int teacherId, CancellationToken cancellationToken);
        Task<IEnumerable<TeacherViewDto>> GetTeachersAsync(TeacherFilterDto teacherFilter, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(TeacherUpdateDto teacher, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int teacherId, CancellationToken cancellationToken);
    }
}
