using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Course
{
    public class UpdateCoursesDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; }
    }
}

