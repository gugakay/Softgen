using AutoMapper;
using DataAccess;
using DataAccess.Dtos.Student;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Softgen.Infrastructure.Extensions;
using Softgen.Infrastructure.Option.Expressions;

namespace Softgen.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper, Serilog.ILogger logger)
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<StudentViewDto> AddStudentAsync(StudentCreateDto student, CancellationToken cancellationToken)
        {
            var newStudent = _mapper.Map<StudentEntity>(student);
            
            var createdStudent = await _unitOfWork.StudentRepository().AddAsync(newStudent, cancellationToken);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<StudentViewDto>(createdStudent);
        }

        public async Task<IEnumerable<StudentViewDto>> GetStudentsAsync(StudentFilterDto studentFilter, CancellationToken cancellationToken)
        {
            var students =  await _unitOfWork.StudentRepository()
                        .GetByCriteria(cancellationToken, new StudentFilterSpecification(studentFilter.ToFilterQuery()).ToExpression());

            return _mapper.Map<IEnumerable<StudentViewDto>>(students);
        }

        public async Task<StudentViewDto> GetStudentByIdAsync(int studentId, CancellationToken cancellationToken)
        {
            var student = await GetStudentEntityByIdAsync(studentId, cancellationToken);

            return _mapper.Map<StudentViewDto>(student);
        }

        public async Task<StudentEntity> GetStudentEntityByIdAsync(int studentId, CancellationToken cancellationToken) =>
            await _unitOfWork.StudentRepository().GetByIdAsync(studentId, cancellationToken);

        public async Task<bool> UpdateAsync(StudentUpdateDto updateStudent, CancellationToken cancellationToken)
        {            
            var existingStudent = await GetStudentEntityByIdAsync(updateStudent.Id, cancellationToken);

            if (existingStudent != null)
            {
                UpdateUserFields(ref existingStudent, updateStudent);
                await _unitOfWork.StudentRepository().UpdateAsync(existingStudent, cancellationToken);
                
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            else
                _logger.Information("Student with id {studentId} not found", updateStudent.Id);

            return false;
        }

        public async Task<bool> DeleteAsync(int studentId, CancellationToken cancellationToken)
        {
            var existingStudent = _unitOfWork.StudentRepository()
                .GetAllAsNoTrackingQueryable()
                .Where(x => x.Id == studentId)
                .Include(x => x.Groups)
                .FirstOrDefault();

            if (existingStudent != null)
            {
                if (existingStudent.Groups.Any())
                {
                    _logger.Information("Student with id {studentId} has groups", studentId);
                    return false;
                }

                _unitOfWork.StudentRepository().Delete(existingStudent);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            else
                _logger.Information("Student with id {studentId} not found", studentId);

            return false;
        }

        private void UpdateUserFields(ref StudentEntity existingStudent, StudentUpdateDto studentUpdate)
        {
            if (!string.IsNullOrEmpty(studentUpdate.Name))
                existingStudent.Name = studentUpdate.Name;

            if (!string.IsNullOrEmpty(studentUpdate.LastName))
                existingStudent.LastName = studentUpdate.LastName;

            if (!string.IsNullOrEmpty(studentUpdate.PrivateNumber))
                existingStudent.PrivateNumber = studentUpdate.PrivateNumber;
            
            if (studentUpdate.BirthDate.HasValue) existingStudent.BirthDate = (DateTime)studentUpdate.BirthDate;
        }
    }
}
