using DataAccess.Entities;

namespace DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<StudentEntity> StudentRepository();
        IRepository<TeacherEntity> TeacherRepository();
        IRepository<GroupEntity> GroupRepository();

        Task<int> SaveChangesAsync();
    }
}
