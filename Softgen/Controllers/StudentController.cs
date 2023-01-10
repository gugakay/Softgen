using DataAccess.Dtos.Student;
using Microsoft.AspNetCore.Mvc;
using Softgen.Services;

namespace Softgen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("students")]
        public async Task<IActionResult> Students(StudentFilterDto studentFilter, CancellationToken cancellationToken)
        {
            var result = await _studentService.GetStudentsAsync(studentFilter, cancellationToken);

            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _studentService.GetStudentByIdAsync(id, cancellationToken);

            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(StudentCreateDto student, CancellationToken cancellationToken)
        {
            var result = await _studentService.AddStudentAsync(student, cancellationToken);
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update(StudentUpdateDto student, CancellationToken cancellationToken)
        {
            var result = await _studentService.UpdateAsync(student, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _studentService.DeleteAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
