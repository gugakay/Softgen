using DataAccess.Entities;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultDbContext _dbContext;

        public UnitOfWork(DefaultDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IRepository<StudentEntity> _studentRepository;
        public IRepository<StudentEntity> StudentRepository() => _studentRepository ??= new Repository<StudentEntity>(_dbContext);

        private IRepository<TeacherEntity> _teacherRepository;
        public IRepository<TeacherEntity> TeacherRepository() => _teacherRepository ??= new Repository<TeacherEntity>(_dbContext);

        private IRepository<GroupEntity> _groupRepository;
        public IRepository<GroupEntity> GroupRepository() => _groupRepository ??= new Repository<GroupEntity>(_dbContext);

        public async Task<int> SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

