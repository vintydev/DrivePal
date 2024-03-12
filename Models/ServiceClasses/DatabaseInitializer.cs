using Google;
using Microsoft.AspNetCore.Identity;
using DrivePal.Data;
using DrivePal.Models;

namespace DrivePal.Models.ServiceClasses
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeRolesAndUsers(
            DrivePalDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();



            // Create roles if they do not exist
            string[] roles = { "Admin", "Instructor", "Learner","Staff" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create an admin user if it does not exist
            if (!context.Users.Any(u => u.UserName == "admin@example.com"))
            {
                var adminUser = new Admin
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Lebron",
                    LastName = "James",
                    City = "Glasgow",
                    Street = "Renfield Street",
                    PostCode = "G11AA",
                    DOB = new DateOnly(1994, 7, 22),
                    EmailConfirmed = true,

                };
                await userManager.CreateAsync(adminUser, "pass123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Create an admin user if it does not exist
            if (!context.Users.Any(u => u.UserName == "instructor@example.com"))
            {
                var adminUser = new Instructor
                {
                    UserName = "instructor@example.com",
                    Email = "instructor@example.com",
                    FirstName = "Luka",
                    LastName = "Doncic",
                    City = "Glasgow",
                    Street = "Buchanan Street",
                    PostCode = "G12AA",
                    DOB = new DateOnly(1996, 5, 10),
                    TotalRating = 0,
                    EmailConfirmed = true,
                    LicenceNumber = "1234567812345678",
                    isApproved = true,
                    isBlocked = false,

                };
                await userManager.CreateAsync(adminUser, "pass123");
                await userManager.AddToRoleAsync(adminUser, "Instructor");
            }


            if (!context.Users.Any(u => u.UserName == "instructor2@example.com"))
            {
                var adminUser = new Instructor
                {
                    UserName = "instructor2@example.com",
                    Email = "instructor2@example.com",
                    FirstName = "Dwayne",
                    LastName = "Wade",
                    City = "Glasgow",
                    Street = "Maryhill Street",
                    PostCode = "G203AA",
                    DOB = new DateOnly(1997, 3, 11),
                    TotalRating = 0,
                    EmailConfirmed = true,
                    LicenceNumber = "8765432187654321",
                    isApproved = true,
                    isBlocked = false,

                };
                await userManager.CreateAsync(adminUser, "pass123");
                await userManager.AddToRoleAsync(adminUser, "Instructor");
            }
          
            //seeding instructor non approved
            if (!context.Users.Any(u => u.UserName == "instructor3@example.com"))
            {
                var adminUser = new Instructor
                {
                    UserName = "instructor3@example.com",
                    Email = "instructor3@example.com",
                    FirstName = "Lewis",
                    LastName = "Shamilton",
                    City = "Glasgow",
                    Street = "4 Maryhill Street",
                    PostCode = "G203AA",
                    DOB = new DateOnly(1997, 3, 11),
                    TotalRating = 0,
                    EmailConfirmed = true,
                    LicenceNumber = "8165432187654321",
                    isApproved = false,
                    isBlocked = false,

                };
                await userManager.CreateAsync(adminUser, "pass123");
                await userManager.AddToRoleAsync(adminUser, "Instructor");
            }
            //seeding instructor non approved
            if (!context.Users.Any(u => u.UserName == "instructor4@example.com"))
            {
                var adminUser = new Instructor
                {
                    UserName = "instructor4@example.com",
                    Email = "instructor4@example.com",
                    FirstName = "Lando",
                    LastName = "Norris",
                    City = "Glasgow",
                    Street = "2 Maryhill Street",
                    PostCode = "G203AA",
                    DOB = new DateOnly(1997, 3, 11),
                    TotalRating = 0,
                    EmailConfirmed = true,
                    LicenceNumber = "8765412187654321",
                    isApproved = false,
                    isBlocked = false,

                };
                await userManager.CreateAsync(adminUser, "pass123");
                await userManager.AddToRoleAsync(adminUser, "Instructor");
            }



            // Create an admin user if it does not exist
            if (!context.Users.Any(u => u.UserName == "learner@example.com"))
            {
                var adminUser = new Learner
                {
                    UserName = "learner@example.com",
                    Email = "learner@example.com",
                    FirstName = "Giannis",
                    LastName = "Antetokounmpo",
                    City = "Glasgow",
                    Street = "Bath Street",
                    PostCode = "G32AA",
                    DOB = new DateOnly(1997, 3, 13),
                    EmailConfirmed = true,
                    LicenceNumber = "12345678"

                };
                await userManager.CreateAsync(adminUser, "pass123");
                await userManager.AddToRoleAsync(adminUser, "Learner");
            }

            // Create an admin user if it does not exist
            if (!context.Users.Any(u => u.UserName == "learner2@example.com"))
            {
                var adminUser = new Learner
                {
                    UserName = "learner2@example.com",
                    Email = "learner2@example.com",
                    FirstName = "Nikola",
                    LastName = "Jokic",
                    City = "Glasgow",
                    Street = "Hope Street",
                    PostCode = "G15AA",
                    DOB = new DateOnly(1994, 4, 11),
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(adminUser, "pass123");
                await userManager.AddToRoleAsync(adminUser, "Learner");
            }
        }
    }
}
