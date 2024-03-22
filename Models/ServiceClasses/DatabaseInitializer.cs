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
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = ["Assertive", "Experienced"],
                    InstructorDrivingGoals = ["Pass quickly"],
                    InstructorTeachingType = ["Manual", "Automatic"],
                    InstructorAvailableDaysOf = ["Monday", "Tuesday", "Saturday"],
                    InstructorTimeOfDay = ["Morning", "Afternoon"],
                    InstructorLessonDuration = [60],
                    InstructorLessonAverage = 60
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
                    PostCode = "G71 8AW",
                    DOB = new DateOnly(1997, 3, 11),
                    TotalRating = 2,
                    EmailConfirmed = true,
                    LicenceNumber = "8765432187654321",
                    isApproved = true,
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Inexperienced,
                    InstructorTeachingTraits = ["Patient", "Calm"],
                    InstructorDrivingGoals = ["Gain driving confidence"],
                    InstructorTeachingType = ["Manual"],
                    InstructorAvailableDaysOf = ["Wednesday", "Thursday", "Friday"],
                    InstructorTimeOfDay = ["Afternoon", "Evening"],
                    InstructorLessonDuration = [90],
                    InstructorLessonAverage = 75
                };
                await userManager.CreateAsync(instructorUser2, "pass123");
                await userManager.AddToRoleAsync(instructorUser2, "Instructor");
            }

            //seeding instructor non approved
            if (!context.Users.Any(u => u.UserName == "instructor4@example.com"))
            {
                var instructorUser4 = new Instructor
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
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = ["Assertive", "Empathetic"],
                    InstructorDrivingGoals = ["Gain driving confidence", "Pass quickly"],
                    InstructorTeachingType = ["Manual", "Automatic"],
                    InstructorAvailableDaysOf = ["Friday", "Saturday", "Sunday"],
                    InstructorTimeOfDay = ["Morning", "Afternoon", "Evening"],
                    InstructorLessonDuration = [60],
                    InstructorLessonAverage = 70
                };
                await userManager.CreateAsync(instructorUser4, "pass123");
                await userManager.AddToRoleAsync(instructorUser4, "Instructor");

            }

            if (!context.Users.Any(u => u.UserName == "instructor5@example.com"))
            {
                var instructorUser5 = new Instructor
                {
                    UserName = "instructor5@example.com",
                    Email = "instructor5@example.com",
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
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = ["Assertive", "Empathetic"],
                    InstructorDrivingGoals = ["Gain driving confidence", "Pass quickly"],
                    InstructorTeachingType = ["Manual", "Automatic"],
                    InstructorAvailableDaysOf = ["Friday", "Saturday", "Sunday"],
                    InstructorTimeOfDay = ["Morning", "Afternoon", "Evening"],
                    InstructorLessonDuration = [60],
                    InstructorLessonAverage = 70
                };
                await userManager.CreateAsync(instructorUser5, "pass123");
                await userManager.AddToRoleAsync(instructorUser5, "Instructor");

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
                    Gender = Gender.Female,
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

            if (!context.Users.Any(u => u.UserName == "learner3@example.com"))
            {
                var learnerUser3 = new Learner
                {
                    UserName = "learner3@example.com",
                    Email = "learner3@example.com",
                    FirstName = "James",
                    LastName = "Thomas",
                    City = "Glasgow",
                    Street = "Mitchell Street",
                    PostCode = "G1 3HL",
                    DOB = new DateOnly(1967, 3, 26),
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(learnerUser3, "pass123");
                await userManager.AddToRoleAsync(learnerUser3, "Learner");
            }
        }

    

    //private static async Task SeedChatMessages(DrivePalDbContext context, UserManager<User> userManager)
        //{
        //    var learner = await userManager.FindByNameAsync("learner@example.com");
        //    var instructor = await userManager.FindByNameAsync("instructor@example.com");

        //    if (learner != null && instructor != null)
        //    {
        //        if (!context.ChatMessages.Any(m => m.SenderId == learner.Id && m.RecipientId == instructor.Id))
        //        {
        //            // Seed initial chat message
        //            var chatMessage = new ChatMessage
        //            {
        //                SenderId = learner.Id,
        //                RecipientId = instructor.Id,
        //                Content = "Hi instructor!",
        //                SentAt = DateTime.Now
        //            };

        //            context.ChatMessages.Add(chatMessage);
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //}
}


