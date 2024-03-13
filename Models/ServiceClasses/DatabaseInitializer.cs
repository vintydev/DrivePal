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
            // Dont drop database, messes with migrations
            // Ensure create instead
            await context.Database.EnsureCreatedAsync();

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
                await userManager.CreateAsync(adminUser, "pass123");
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
                await userManager.CreateAsync(instructorUser, "pass123");
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
                await userManager.CreateAsync(instructorUser2, "pass123");
                await userManager.AddToRoleAsync(instructorUser2, "Instructor");
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
                    isBlocked = false
                };
                await userManager.CreateAsync(adminUser, "pass123");
                await userManager.AddToRoleAsync(adminUser, "Instructor");

            }
            if (!context.Users.Any(u => u.UserName == "instructor5@example.com"))
            {
                var adminUser = new Instructor
                {
                    UserName = "instructor4@example.com",
                    Email = "instructor4@example.com",
                    FirstName = "Lewis",
                    LastName = "Shamilton",
                    City = "Glasgow",
                    Street = "2 Maryhill Street",
                    PostCode = "G203AA",
                    DOB = new DateOnly(1997, 3, 11),
                    TotalRating = 0,
                    EmailConfirmed = true,
                    LicenceNumber = "8765412187654323",
                    isApproved = false,
                    isBlocked = false
                };
                await userManager.CreateAsync(adminUser, "pass123");
                await userManager.AddToRoleAsync(adminUser, "Instructor");

            }

            // Create an admin user if it does not exist
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
                    await userManager.CreateAsync(learnerUser, "pass123");
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
                    await userManager.CreateAsync(learnerUser2, "pass123");
                    await userManager.AddToRoleAsync(learnerUser2, "Learner");
                }
            }
        }
    }

