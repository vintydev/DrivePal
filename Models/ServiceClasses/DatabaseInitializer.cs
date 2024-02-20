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
            string[] roles = { "Admin", "Instructor", "Learner" };
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
                await userManager.CreateAsync(adminUser, "AdminPassword123!");
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
                    EmailConfirmed = true,

                };
                await userManager.CreateAsync(adminUser, "InstructorPassword123!");
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

                };
                await userManager.CreateAsync(adminUser, "LearnerPassword123!");
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
                await userManager.CreateAsync(adminUser, "Learner2Password123!");
                await userManager.AddToRoleAsync(adminUser, "Learner");
            }



        }
    }

}
