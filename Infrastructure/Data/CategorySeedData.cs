using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class CategorySeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "IT", Code = CourseTypeEnum.IT },
                new Category { Id = 2, Name = "Sales", Code = CourseTypeEnum.Sales },
                new Category { Id = 3, Name = "HR", Code = CourseTypeEnum.HR },
                new Category { Id = 4, Name = "Marketing", Code = CourseTypeEnum.Marketing }
            );
        }
    }
}







