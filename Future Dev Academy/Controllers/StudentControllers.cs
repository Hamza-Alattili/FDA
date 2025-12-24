using Application.DTOs.Student;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // ================= Register Student
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterStudentDto dto)
        {
            await _studentService.RegisterStudentAsync(dto);
            return Ok("Student registered successfully");
        }

        // ================= Get All Students (Admin)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] StudentFilterDto filter)
        {
            var result = await _studentService.GetStudentsAsync(filter);
            return Ok(result);
        }

        // ================= Get Student By Id (Admin)
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _studentService.GetStudentByIdAsync(id);
            return Ok(result);
        }

        // ================= Update Student
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateStudentDto dto)
        {
            await _studentService.UpdateStudentAsync(dto);
            return Ok("Student updated successfully");
        }

        // ================= Delete Student (Admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return Ok("Student deleted successfully");
        }
    }
}
