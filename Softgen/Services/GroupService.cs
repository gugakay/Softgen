using AutoMapper;
using DataAccess;
using DataAccess.Dtos.Group;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Softgen.Infrastructure.Extensions;
using Softgen.Infrastructure.Option.Expressions;

namespace Softgen.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly Serilog.ILogger _logger;

        public GroupService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IStudentService studentService, 
            ITeacherService teacherService,
            Serilog.ILogger logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _studentService = studentService;
            _teacherService = teacherService;
            _logger = logger;
        }

        public async Task<GroupViewDto> AddGroupAsync(GroupCreateDto group, CancellationToken cancellationToken)
        {
            var newGroup = _mapper.Map<GroupEntity>(group);

            var createdGroup = await _unitOfWork.GroupRepository().AddAsync(newGroup, cancellationToken);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GroupViewDto>(createdGroup);
        }



        public async Task<IEnumerable<GroupViewDto>> GetGroupsAsync(GroupFilterDto groupFilter, CancellationToken cancellationToken)
        {
            var groups = await _unitOfWork.GroupRepository()
                        .GetByCriteria(cancellationToken,new GroupFilterSpecification(groupFilter.ToFilterQuery()).ToExpression());

            return _mapper.Map<IEnumerable<GroupViewDto>>(groups);
        }

        public async Task<GroupViewDto> GetGroupByIdAsync(int groupId, CancellationToken cancellationToken)
        {
            var group = await GetGroupEntityByIdAsync(groupId, cancellationToken);

            return _mapper.Map<GroupViewDto>(group);
        }

        public async Task<GroupEntity> GetGroupEntityByIdAsync(int groupId, CancellationToken cancellationToken) =>
            await _unitOfWork.GroupRepository().GetByIdAsync(groupId, cancellationToken);

        public async Task<bool> UpdateAsync(GroupUpdateDto updateGroup, CancellationToken cancellationToken)
        {
            var existingGroup = await GetGroupEntityByIdAsync(updateGroup.Id, cancellationToken);

            if (existingGroup != null)
            {
                UpdateUserFields(ref existingGroup, updateGroup);
                await _unitOfWork.GroupRepository().UpdateAsync(existingGroup, cancellationToken);

                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            else
                _logger.Information("Group with id {groupId} not found", updateGroup.Id);
            
            return false;
        }

        public async Task<bool> DeleteAsync(int groupId, CancellationToken cancellationToken)
        {
            var existingGroup = _unitOfWork.GroupRepository()
                    .GetAllAsNoTrackingQueryable()
                    .Where(x => x.Id == groupId)
                    .Include(x => x.Students)
                    .Include(x => x.Teacher)
                    .FirstOrDefault();

            if (existingGroup != null)
            {
                if (existingGroup.Students.Any() || existingGroup.Teacher != null)
                {
                    _logger.Information("Group with id {groupId} has students or teacher", groupId);
                    return false;
                }

                _unitOfWork.GroupRepository().Delete(existingGroup);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            else
                _logger.Information("Group with id {groupId} not found", groupId);
            
            return false;
        }

        public async Task<bool> AddStudentToGroupAsync(int groupId, int studentId, CancellationToken cancellationToken)
        {
            var existingGroup = await GetGroupEntityByIdAsync(groupId, cancellationToken);
            var existingStudent = await _studentService.GetStudentEntityByIdAsync(studentId, cancellationToken);

            if (existingGroup != null && existingStudent != null)
            {
                if (existingGroup.Students?.Any() == true && existingGroup.Students.Any(s => s.Id == studentId))
                {
                    _logger.Information("Student with id {studentId} already exists in group with id {groupId}", studentId, groupId);
                    return false;
                }

                existingGroup.Students.Add(existingStudent);

                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AddTeacherToGroupAsync(int groupId, int teacherId, CancellationToken cancellationToken)
        {
            var existingGroup = await GetGroupEntityByIdAsync(groupId, cancellationToken);
            var existingTeacher = await _teacherService.GetTeacherEntityByIdAsync(teacherId, cancellationToken);

            if (existingGroup != null && existingTeacher != null)
            {
                if (existingGroup.Teacher != null)
                {
                    _logger.Information("Group already has a teacher");
                    return false;
                }

                existingGroup.Teacher = existingTeacher;
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveStudentFromGroupAsync(int groupId, int studentId, CancellationToken cancellationToken)
        {
            var existingGroup = GetGroups()
                .Where(x => x.Id == groupId)
                .FirstOrDefault();
            
            var existingStudent = await _studentService.GetStudentEntityByIdAsync(studentId, cancellationToken);

            if (existingGroup != null && existingStudent != null)
            {
                if (!existingGroup.Students.Any(s => s.Id == studentId))
                {
                    _logger.Information("Student with id {studentId} is not in group with id {groupId}", studentId, groupId);
                    return false;
                }

                existingGroup.Students.Remove(existingStudent);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveTeacherFromGroupAsync(int groupId, int teacherId, CancellationToken cancellationToken)
        {
            var existingGroup = GetGroups()
                    .Where(x => x.Id == groupId)
                    .FirstOrDefault();
            
            var existingTeacher = await _teacherService.GetTeacherEntityByIdAsync(teacherId, cancellationToken);

            if (existingGroup != null && existingTeacher != null)
            {
                if (existingGroup.Teacher.Id != teacherId)
                {
                    _logger.Information("Teacher with id {teacherId} is not in group with id {groupId}", teacherId, groupId);
                    return false;
                }

                existingGroup.Teacher = null;
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private static void UpdateUserFields(ref GroupEntity existingGroup, GroupUpdateDto groupUpdate)
        {
            if (!string.IsNullOrEmpty(groupUpdate.Name))
                existingGroup.Name = groupUpdate.Name;

            if (groupUpdate.Number != null || groupUpdate.Number != 0)
                existingGroup.Number = (int)groupUpdate.Number;
        }

        public IQueryable<GroupEntity> GetGroups() =>
             _unitOfWork.GroupRepository()
                    .GetAllAsNoTrackingQueryable()
                    .Include(x => x.Students)
                    .Include(x => x.Teacher);
    }
}
