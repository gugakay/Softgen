using AutoMapper;
using DataAccess;
using DataAccess.Dtos.Teacher;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Softgen.Infrastructure.Extensions;
using Softgen.Infrastructure.Option.Expressions;

namespace Softgen.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;

        public TeacherService(IUnitOfWork unitOfWork, IMapper mapper, Serilog.ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TeacherViewDto> AddTeacherAsync(TeacherCreateDto teacher, CancellationToken cancellationToken)
        {
            var newTeacher = _mapper.Map<TeacherEntity>(teacher);

            var createdTeacher = await _unitOfWork.TeacherRepository().AddAsync(newTeacher, cancellationToken);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TeacherViewDto>(createdTeacher);
        }


        public async Task<IEnumerable<TeacherViewDto>> GetTeachersAsync(TeacherFilterDto teacherFilter, CancellationToken cancellationToken)
        {
            var teachers = await _unitOfWork.TeacherRepository()
                        .GetByCriteria(cancellationToken, new TeacherFilterSpecification(teacherFilter.ToFilterQuery()).ToExpression());

            return _mapper.Map<IEnumerable<TeacherViewDto>>(teachers);
        }

        public async Task<TeacherViewDto> GetTeacherByIdAsync(int teacherId, CancellationToken cancellationToken)
        {
            var teacher = await GetTeacherEntityByIdAsync(teacherId, cancellationToken);

            return _mapper.Map<TeacherViewDto>(teacher);
        }

        public async Task<TeacherEntity> GetTeacherEntityByIdAsync(int teacherId, CancellationToken cancellationToken) =>
            await _unitOfWork.TeacherRepository().GetByIdAsync(teacherId, cancellationToken);

        public async Task<bool> UpdateAsync(TeacherUpdateDto updateTeacher, CancellationToken cancellationToken)
        {            
            var existingTeacher = await GetTeacherEntityByIdAsync(updateTeacher.Id, cancellationToken);

            if (existingTeacher != null)
            {
                UpdateUserFields(ref existingTeacher, updateTeacher);
                await _unitOfWork.TeacherRepository().UpdateAsync(existingTeacher, cancellationToken);
                
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            else
                _logger.Information("Teacher with id {teacherId} not found", updateTeacher.Id);

            return false;
        }

        public async Task<bool> DeleteAsync(int teacherId, CancellationToken cancellationToken)
        {
            var existingTeacher = _unitOfWork.TeacherRepository()
                .GetAllAsNoTrackingQueryable()
                .Where(x => x.Id == teacherId)
                .Include(x => x.Groups)
                .FirstOrDefault();

            if (existingTeacher != null)
            {
                if(existingTeacher.Groups.Any())
                {
                    _logger.Information("Teacher with id {teacherId} has groups", teacherId);
                    return false;
                }

                _unitOfWork.TeacherRepository().Delete(existingTeacher);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            else
                _logger.Information("Teacher with id {teacherId} not found", teacherId);
            return false;
        }

        private static void UpdateUserFields(ref TeacherEntity existingTeacher, TeacherUpdateDto teacherUpdate)
        {
            if (!string.IsNullOrEmpty(teacherUpdate.Name))
                existingTeacher.Name = teacherUpdate.Name;

            if (!string.IsNullOrEmpty(teacherUpdate.LastName))
                existingTeacher.LastName = teacherUpdate.LastName;

            if (!string.IsNullOrEmpty(teacherUpdate.PrivateNumber))
                existingTeacher.PrivateNumber = teacherUpdate.PrivateNumber;
            
            if (teacherUpdate.BirthDate.HasValue) existingTeacher.BirthDate = (DateTime)teacherUpdate.BirthDate;
        }
    }
}
