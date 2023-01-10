using DataAccess.Dtos.Teacher;
using Microsoft.AspNetCore.Mvc;
using Softgen.Services;

namespace Softgen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost("teachers")]
        public async Task<IActionResult> TeachersAsync(TeacherFilterDto teacherFilter, CancellationToken cancellationToken)
        {
            var result = await _teacherService.GetTeachersAsync(teacherFilter, cancellationToken);

            if (result == null)
                return NoContent();
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _teacherService.GetTeacherByIdAsync(id, cancellationToken);
            
            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TeacherCreateDto teacher, CancellationToken cancellationToken)
        {
            var result = await _teacherService.AddTeacherAsync(teacher, cancellationToken);
            return Ok(result);
        }
        
        [HttpPatch]
        public async Task<IActionResult> Update(TeacherUpdateDto teacher, CancellationToken cancellationToken)
        {
            var result = await _teacherService.UpdateAsync(teacher, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _teacherService.DeleteAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}

