using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Birthdate { get; set; }
        public string UniversityName { get; set; }
        [Required] 
        public string FullName { get; set; }
    }
}
