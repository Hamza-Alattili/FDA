using Application.DTOs.Auth;
using Application.DTOs.Student;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Student> _studentRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentService(
            IGenericRepository<Student> studentRepo,
            IGenericRepository<User> userRepo,
            IGenericRepository<Role> roleRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _studentRepo = studentRepo;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        // ================= Register =================
        public async Task RegisterStudentAsync(RegisterStudentDto input)
        {
            string passwordPattern =
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$";

            if (!Regex.IsMatch(input.Password, passwordPattern))
                throw new Exception("Password is weak");

            if (await _userRepo.GetAll().AnyAsync(u => u.Email == input.Email))
                throw new Exception("Email already exists");

            if (await _userRepo.GetAll().AnyAsync(u => u.PhoneNumber == input.PhoneNumber))
                throw new Exception("Phone number already exists");

            var studentRoleId = await _roleRepo.GetAll()
                .Where(r => r.Code == SystemRole.Student)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (studentRoleId == 0)
                throw new Exception("Student role not found");

            var user = new User
            {
                Name = input.Name,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,
                RoleId = studentRoleId
            };

            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, input.Password);

            await _userRepo.Insert(user);
            await _userRepo.SaveChanges();

            var student = new Student
            {
                UserId = user.Id,
                BirthDate = input.BirthDate,
                UniversityName = input.UniversityName
            };

            await _studentRepo.Insert(student);
            await _studentRepo.SaveChanges();
        }

        // ================= Get All =================
        public async Task<List<StudentListDto>> GetStudentsAsync(StudentFilterDto filter)
        {
            var query = _studentRepo.GetAll()
                .Include(s => s.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(s => s.User.Name.Contains(filter.Name));

            if (!string.IsNullOrEmpty(filter.Email))
                query = query.Where(s => s.User.Email.Contains(filter.Email));

            if (!string.IsNullOrEmpty(filter.UniversityName))
                query = query.Where(s => s.UniversityName.Contains(filter.UniversityName));

            return await query.Select(s => new StudentListDto
            {
                Id = s.Id,
                Name = s.User.Name,
                Email = s.User.Email,
                PhoneNumber = s.User.PhoneNumber,
                BirthDate = s.BirthDate,
                UniversityName = s.UniversityName
            }).ToListAsync();
        }

        // ================= Get By Id =================
        public async Task<StudentListDto> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepo.GetAll()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                throw new Exception("Student not found");

            return new StudentListDto
            {
                Id = student.Id,
                Name = student.User.Name,
                Email = student.User.Email,
                PhoneNumber = student.User.PhoneNumber,
                BirthDate = student.BirthDate,
                UniversityName = student.UniversityName
            };
        }

        // ================= Update =================
        public async Task UpdateStudentAsync(UpdateStudentDto dto)
        {
            var student = await _studentRepo.GetAll()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.User.Email == dto.Email);

            if (student == null)
                throw new Exception("Student not found");

            student.User.Name = dto.Name;
            student.User.Email = dto.Email;
            student.User.PhoneNumber = dto.PhoneNumber;
            student.BirthDate = dto.BirthDate;
            student.UniversityName = dto.UniversityName;

            _userRepo.Update(student.User);
            _studentRepo.Update(student);
            await _studentRepo.SaveChanges();
        }

        // ================= Reset Password =================
        public async Task ResetPasswordAsync(ResetPasswordDto input)
        {
            var userIdClaim = _httpContextAccessor.HttpContext!
                .User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException();

            var userId = int.Parse(userIdClaim.Value);

            var user = await _userRepo.GetById(userId);

            if (user == null)
                throw new Exception("User not found");

            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, input.NewPassword);

            _userRepo.Update(user);
            await _userRepo.SaveChanges();
        }

        // ================= Delete =================
        public async Task DeleteStudentAsync(int id)
        {
            var student = await _studentRepo.GetAll()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                throw new Exception("Student not found");

            _studentRepo.Delete(student);
            await _studentRepo.SaveChanges();
        }
    }
}
        
   

      
        
    

    

    