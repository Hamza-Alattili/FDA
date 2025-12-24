using Application.DTOs.Course;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CourseService : ICoursesService
    {
        private readonly IGenericRepository<Course> _courseRepo;

        public CourseService(IGenericRepository<Course> courseRepo)
        {
            _courseRepo = courseRepo;
        }

  
        public async Task CreateCourseAsync(CreateCourseDto input)
        {
            bool exists = await _courseRepo
                .GetAll()
                .AnyAsync(c => c.Title == input.Title);

            if (exists)
                throw new Exception("Course title already exists");

            if (input.StartDate >= input.EndDate)
                throw new Exception("Start date must be before end date");

            var course = new Course
            {
                Title = input.Title,
                Description = input.Description,
                Price = (int)input.Price,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                CategoryId = input.CategoryId
            };

           await _courseRepo.Insert(course); 
        }

        
        public async Task<List<CoursesListDto>> GetAllCoursesAsync(CourseFilterDto filter)
        {
            var query = _courseRepo.GetAll()
                .Include(c => c.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(c => c.Title.Contains(filter.Title));

            if (filter.CategoryId.HasValue)
                query = query.Where(c => c.CategoryId == filter.CategoryId);

            return await query.Select(c => new CoursesListDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                CategoryName = c.Category.Name
            }).ToListAsync();
        }

        public async Task<CoursesListDto> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepo
                .GetAll()
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                throw new Exception("Course not found");

            return new CoursesListDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                CategoryName = course.Category.Name
            };
        }

        public async Task UpdateCourseAsync(UpdateCoursesDto input)
        {
            var course = await _courseRepo.GetById(input.Id);

            if (course == null)
                throw new Exception("Course not found");

            course.Title = input.Title;
            course.Description = input.Description;
            course.Price = (int)input.Price;
            course.EndDate = input.EndDate;
            course.CategoryId = input.CategoryId;

            _courseRepo.Update(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _courseRepo.GetById(id);

            if (course == null)
                throw new Exception("Course not found");

           await  _courseRepo.Delete(course);
        }
    }
}
