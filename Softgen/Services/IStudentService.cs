using DataAccess.Dtos.Student;
using DataAccess.Entities;

namespace Softgen.Services
{
    public interface IStudentService
    {
        Task<StudentViewDto> AddStudentAsync(StudentCreateDto student, CancellationToken cancellationToken);
        Task<StudentViewDto> GetStudentByIdAsync(int studentId, CancellationToken cancellationToken);
        Task<StudentEntity> GetStudentEntityByIdAsync(int studentId, CancellationToken cancellationToken);
        Task<IEnumerable<StudentViewDto>> GetStudentsAsync(StudentFilterDto studentFilter, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(StudentUpdateDto student, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int studentId, CancellationToken cancellationToken);
    }
}
