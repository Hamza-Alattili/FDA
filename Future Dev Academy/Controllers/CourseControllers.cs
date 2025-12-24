using Application.DTOs.Course;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICoursesService _courseService;

        public CourseController(ICoursesService courseService)
        {
            _courseService = courseService;
        }

        // ================= Create Course (Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseDto dto)
        {
            await _courseService.CreateCourseAsync(dto);
            return Ok("Course created successfully");
        }

        // ================= Get All Courses + Filter
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CourseFilterDto filter)
        {
            var result = await _courseService.GetAllCoursesAsync(filter);
            return Ok(result);
        }

        // ================= Get Course By Id
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _courseService.GetCourseByIdAsync(id);
            return Ok(result);
        }

        // ================= Update Course (Admin)
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCoursesDto dto)
        {
            await _courseService.UpdateCourseAsync(dto);
            return Ok("Course updated successfully");
        }

        // ================= Delete Course (Admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteCourseAsync(id);
            return Ok("Course deleted successfully");
        }
    }
}
