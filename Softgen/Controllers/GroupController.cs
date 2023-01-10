using DataAccess.Dtos.Group;
using Microsoft.AspNetCore.Mvc;
using Softgen.Services;

namespace Softgen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("groups")]
        public async Task<IActionResult> Groups(GroupFilterDto groupFilter, CancellationToken cancellationToken)
        {
            var result = await _groupService.GetGroupsAsync(groupFilter, cancellationToken);

            if (result == null)
                return NoContent();

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _groupService.GetGroupByIdAsync(id, cancellationToken);
            
            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Add(GroupCreateDto group, CancellationToken cancellationToken)
        {
            var result = await _groupService.AddGroupAsync(group, cancellationToken);
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update(GroupUpdateDto group, CancellationToken cancellationToken)
        {
            var result = await _groupService.UpdateAsync(group, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _groupService.DeleteAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost("add-student")]
        public async Task<IActionResult> Student(int groupId, int studentId, CancellationToken cancellationToken)
        {
            var result = await _groupService.AddStudentToGroupAsync(groupId, studentId, cancellationToken);
            return Ok(result);
        }

        [HttpPost("add-teacher")]
        public async Task<IActionResult> Teacher(int groupId, int teacherId, CancellationToken cancellationToken)
        {
            var result = await _groupService.AddTeacherToGroupAsync(groupId, teacherId, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("remove-student")]
        public async Task<IActionResult> RemoveStudent(int groupId, int studentId, CancellationToken cancellationToken)
        {
            var result = await _groupService.RemoveStudentFromGroupAsync(groupId, studentId, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("remove-teacher")]
        public async Task<IActionResult> RemoveTeacher(int groupId, int teacherId, CancellationToken cancellationToken)
        {
            var result = await _groupService.RemoveTeacherFromGroupAsync(groupId, teacherId, cancellationToken);
            return Ok(result);
        }
    }
}
