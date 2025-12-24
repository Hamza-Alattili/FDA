using Application.DTOs.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICoursesService
    {
        Task CreateCourseAsync(CreateCourseDto input);

        Task<List<CoursesListDto>> GetAllCoursesAsync(CourseFilterDto filter);

        Task<CoursesListDto> GetCourseByIdAsync(int id);

        Task UpdateCourseAsync(UpdateCoursesDto input);

        Task DeleteCourseAsync(int id);
    }
}
