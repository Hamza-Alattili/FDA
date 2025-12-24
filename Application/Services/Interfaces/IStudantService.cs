using Application.DTOs.Auth;
using Application.DTOs.Student;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task RegisterStudentAsync(RegisterStudentDto dto);
        Task<List<StudentListDto>> GetStudentsAsync(StudentFilterDto filter);
        Task<StudentListDto> GetStudentByIdAsync(int id);
        Task ResetPasswordAsync(ResetPasswordDto input);
        Task DeleteStudentAsync(int id);
        Task UpdateStudentAsync(UpdateStudentDto dto); 
    }
}

