using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public static class UserSeedData
{

    public static async Task InitializeAsync(FDADbContext context)
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = FDAConst.ADMAIN_ROLE, Code = SystemRole.Admin },
                new Role { Name = FDAConst.STUDENT_ROLE, Code = SystemRole.Student }
            );
            await context.SaveChangesAsync();

        }
        if (!context.Users.Any())
        {
            var PasswordHasher = new PasswordHasher<User>();
            var adminRoleId = context.Roles.First(r => r.Code == SystemRole.Admin).Id;

            var admin = new User
            {
                Name = "System Admin",
                Email = "admain@fda.com",
                PhoneNumber = "00962781505664",
                RoleId = adminRoleId,
            };
            admin.Password = PasswordHasher.HashPassword(admin, "Admin@123");
            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
    

    


    