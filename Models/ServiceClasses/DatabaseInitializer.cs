using Google;
using Microsoft.AspNetCore.Identity;
using DrivePal.Data;
using DrivePal.Models;
using System.Linq;
using System.Threading.Tasks;

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
            string[] roles = { "Admin", "Instructor", "Learner", "Staff" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin users if they do not exist
            if (!context.Users.Any(u => u.UserName == "admin@example.com"))
            {
                var adminUser = new Admin
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "John",
                    LastName = "Smith",
                    City = "Glasgow",
                    Street = "Renfield Street",
                    PostCode = "G11AA",
                    DOB = new DateOnly(1994, 7, 22),
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(adminUser, "AdminPassword123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Create instructor users if they do not exist
            if (!context.Users.Any(u => u.UserName == "instructor@example.com"))
            {
                var instructorUser = new Instructor
                {
                    UserName = "instructor@example.com",
                    Email = "instructor@example.com",
                    FirstName = "David",
                    LastName = "Jones",
                    City = "Glasgow",
                    Street = "Buchanan Street",
                    PostCode = "G13 2YH",
                    DOB = new DateOnly(1996, 5, 10),
                    TotalRating = 4,
                    EmailConfirmed = true,
                    LicenceNumber = "1234567812345678",
                    isApproved = true,
                    isBlocked = false,
                };
                await userManager.CreateAsync(instructorUser, "InstructorPassword123!");
                await userManager.AddToRoleAsync(instructorUser, "Instructor");
            }

            if (!context.Users.Any(u => u.UserName == "instructor2@example.com"))
            {
                var instructorUser2 = new Instructor
                {
                    UserName = "instructor2@example.com",
                    Email = "instructor2@example.com",
                    FirstName = "Michael",
                    LastName = "Taylor",
                    City = "Glasgow",
                    Street = "Maryhill Street",
                    PostCode = "G5 8AQ",
                    DOB = new DateOnly(1997, 3, 11),
                    TotalRating = 2,
                    EmailConfirmed = true,
                    LicenceNumber = "8765432187654321",
                    isApproved = true,
                    isBlocked = false,
                };
                await userManager.CreateAsync(instructorUser2, "Instructor2Password123!");
                await userManager.AddToRoleAsync(instructorUser2, "Instructor");
            }

            // Create learner users if they do not exist
            if (!context.Users.Any(u => u.UserName == "learner@example.com"))
            {
                var learnerUser = new Learner
                {
                    UserName = "learner@example.com",
                    Email = "learner@example.com",
                    FirstName = "Emma",
                    LastName = "Williams",
                    City = "Glasgow",
                    Street = "Bath Street",
                    PostCode = "G1 1AA",
                    DOB = new DateOnly(1997, 3, 13),
                    EmailConfirmed = true,
                    LicenceNumber = "12345678"
                };
                await userManager.CreateAsync(learnerUser, "LearnerPassword123!");
                await userManager.AddToRoleAsync(learnerUser, "Learner");
            }

            if (!context.Users.Any(u => u.UserName == "learner2@example.com"))
            {
                var learnerUser2 = new Learner
                {
                    UserName = "learner2@example.com",
                    Email = "learner2@example.com",
                    FirstName = "Sophie",
                    LastName = "Brown",
                    City = "Glasgow",
                    Street = "Hope Street",
                    PostCode = "G15AA",
                    DOB = new DateOnly(1994, 4, 11),
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(learnerUser2, "Learner2Password123!");
                await userManager.AddToRoleAsync(learnerUser2, "Learner");
            }
        }
    }
}
