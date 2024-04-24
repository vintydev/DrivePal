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
                    Street = "West Graham Street",
                    PostCode = "G4 9LH",
                    DOB = new DateOnly(1994, 7, 22),
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(adminUser, "pass123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            if (!context.Users.Any(u => u.UserName == "staff1@example.com"))
            {
                var staffUser = new Staff
                {
                    UserName = "staff1@example.com",
                    Email = "staff1@example.com",
                    FirstName = "Steve",
                    LastName = "Buscemi",
                    City = "Glasgow",
                    Street = "9 Luaf Street",
                    PostCode = "G52 9LH",
                    DOB = new DateOnly(1994, 7, 22),
                    EmailConfirmed = true,
                    WorkType=WorkType.FullTime,
                };
                await userManager.CreateAsync(staffUser, "pass123");
                await userManager.AddToRoleAsync(staffUser, "Staff");
            }
            if (!context.Users.Any(u => u.UserName == "staff2@example.com"))
            {
                var staffUser = new Staff
                {
                    UserName = "staff2@example.com",
                    Email = "staff2@example.com",
                    FirstName = "Rowan",
                    LastName = "Devers",
                    City = "Glasgow",
                    Street = "9 Luaf Street",
                    PostCode = "G52 9LH",
                    DOB = new DateOnly(1998, 11, 07),
                    EmailConfirmed = true,
                    WorkType=WorkType.PartTime,
                };
                await userManager.CreateAsync(staffUser, "pass123");
                await userManager.AddToRoleAsync(staffUser, "Staff");
            }


            // Instructor 1
            if (!context.Users.Any(u => u.UserName == "instructor@example.com"))
            {
                var instructorUser1 = new Instructor
                {
                    UserName = "instructor@example.com",
                    Email = "instructor@example.com",
                    FirstName = "David",
                    LastName = "Jones",
                    City = "Glasgow",
                    Street = "Victoria Street",
                    PostCode = "G73 1DT",
                    DOB = new DateOnly(1996, 5, 10),
                    Gender = Gender.Male,
                    TotalRating = 4,
                    EmailConfirmed = true,
                    LicenceNumber = "JONES960510G1H2F",
                    isApproved = true,
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = new List<string> { "Assertive", "Experienced" },
                    InstructorDrivingGoals = new List<string> { "Pass quickly" },
                    InstructorTeachingType = new List<string> { "Manual", "Automatic" },
                    InstructorAvailableDaysOf = new List<string> { "Monday", "Tuesday", "Saturday" },
                    InstructorTimeOfDay = new List<string> { "Morning", "Afternoon" },
                    InstructorLessonDuration = new int[] { 60 },
                    InstructorLessonAverage = 60
                };
                await userManager.CreateAsync(instructorUser1, "pass123");
                await userManager.AddToRoleAsync(instructorUser1, "Instructor");
            }

            if (!context.Users.Any(u => u.UserName == "instructor2@example.com"))
            {
                var instructorUser2 = new Instructor
                {
                    UserName = "instructor2@example.com",
                    Email = "instructor2@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    City = "Hamilton",
                    Street = "Main Street",
                    PostCode = "ML3 0NA",
                    DOB = new DateOnly(1990, 8, 15),
                    Gender = Gender.Male,
                    TotalRating = 4,
                    EmailConfirmed = true,
                    LicenceNumber = "DOE900815Q3W4E",
                    isApproved = true,
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = new List<string> { "Patient", "Experienced" },
                    InstructorDrivingGoals = new List<string> { "Pass quickly" },
                    InstructorTeachingType = new List<string> { "Manual", "Automatic" },
                    InstructorAvailableDaysOf = new List<string> { "Monday", "Wednesday", "Friday" },
                    InstructorTimeOfDay = new List<string> { "Morning", "Afternoon" },
                    InstructorLessonDuration = new int[] { 60 },
                    InstructorLessonAverage = 60
                };
                await userManager.CreateAsync(instructorUser2, "pass123");
                await userManager.AddToRoleAsync(instructorUser2, "Instructor");
            }

            if (!context.Users.Any(u => u.UserName == "instructor3@example.com"))
            {
                var instructorUser3 = new Instructor
                {
                    UserName = "instructor3@example.com",
                    Email = "instructor3@example.com",
                    FirstName = "Emma",
                    LastName = "Smith",
                    City = "Clarkston",
                    Street = "High Street",
                    PostCode = "G76 8DQ",
                    DOB = new DateOnly(1985, 10, 25),
                    Gender = Gender.Female,
                    TotalRating = 4,
                    EmailConfirmed = true,
                    LicenceNumber = "SMITH851025Q7W8E",
                    isApproved = true,
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = new List<string> { "Patient", "Experienced" },
                    InstructorDrivingGoals = new List<string> { "Gain driving confidence", "Pass quickly" },
                    InstructorTeachingType = new List<string> { "Manual" },
                    InstructorAvailableDaysOf = new List<string> { "Tuesday", "Thursday", "Saturday" },
                    InstructorTimeOfDay = new List<string> { "Afternoon", "Evening" },
                    InstructorLessonDuration = new int[] { 90 },
                    InstructorLessonAverage = 75
                };
                await userManager.CreateAsync(instructorUser3, "pass123");
                await userManager.AddToRoleAsync(instructorUser3, "Instructor");
            }

            if (!context.Users.Any(u => u.UserName == "instructor4@example.com"))
            {
                var instructorUser4 = new Instructor
                {
                    UserName = "instructor4@example.com",
                    Email = "instructor4@example.com",
                    FirstName = "Michael",
                    LastName = "Johnson",
                    City = "Glasgow",
                    Street = "Argyle Street",
                    PostCode = "G3 8TJ",
                    DOB = new DateOnly(1980, 6, 15),
                    Gender = Gender.Male,
                    TotalRating = 3,
                    EmailConfirmed = true,
                    LicenceNumber = "JOHNS800615R6T7Y",
                    isApproved = true,
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = new List<string> { "Patient", "Calm" },
                    InstructorDrivingGoals = new List<string> { "Gain driving confidence" },
                    InstructorTeachingType = new List<string> { "Manual" },
                    InstructorAvailableDaysOf = new List<string> { "Monday", "Wednesday", "Friday" },
                    InstructorTimeOfDay = new List<string> { "Morning", "Afternoon" },
                    InstructorLessonDuration = new int[] { 60, 90 },
                    InstructorLessonAverage = 75
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
                    FirstName = "Sophie",
                    LastName = "Wilson",
                    City = "Glasgow",
                    Street = "Sauchiehall Street",
                    PostCode = "G2 3JW",
                    DOB = new DateOnly(1992, 8, 21),
                    Gender = Gender.Female,
                    TotalRating = 5,
                    EmailConfirmed = true,
                    LicenceNumber = "WILSO920821G3H4J",
                    isApproved = true,
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = new List<string> { "Assertive", "Experienced" },
                    InstructorDrivingGoals = new List<string> { "Pass quickly" },
                    InstructorTeachingType = new List<string> { "Automatic" },
                    InstructorAvailableDaysOf = new List<string> { "Tuesday", "Thursday", "Saturday" },
                    InstructorTimeOfDay = new List<string> { "Afternoon", "Evening" },
                    InstructorLessonDuration = new int[] { 60 },
                    InstructorLessonAverage = 60
                };
                await userManager.CreateAsync(instructorUser5, "pass123");
                await userManager.AddToRoleAsync(instructorUser5, "Instructor");
            }

            if (!context.Users.Any(u => u.UserName == "instructor6@example.com"))
            {
                var instructorUser6 = new Instructor
                {
                    UserName = "instructor6@example.com",
                    Email = "instructor6@example.com",
                    FirstName = "John",
                    LastName = "Brown",
                    City = "East Kilbride",
                    Street = "Main Street",
                    PostCode = "G75 0BX",
                    DOB = new DateOnly(1983, 4, 30),
                    Gender = Gender.Male,
                    TotalRating = 4,
                    EmailConfirmed = true,
                    LicenceNumber = "BROWN830430G7B5X",
                    isApproved = true,
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = new List<string> { "Patient", "Calm" },
                    InstructorDrivingGoals = new List<string> { "Gain driving confidence", "Pass quickly" },
                    InstructorTeachingType = new List<string> { "Manual" },
                    InstructorAvailableDaysOf = new List<string> { "Monday", "Wednesday", "Friday" },
                    InstructorTimeOfDay = new List<string> { "Morning", "Afternoon" },
                    InstructorLessonDuration = new int[] { 60, 90 },
                    InstructorLessonAverage = 75
                };
                await userManager.CreateAsync(instructorUser6, "pass123");
                await userManager.AddToRoleAsync(instructorUser6, "Instructor");
            }

            if (!context.Users.Any(u => u.UserName == "instructor7@example.com"))
            {
                var instructorUser7 = new Instructor
                {
                    UserName = "instructor7@example.com",
                    Email = "instructor7@example.com",
                    FirstName = "Marek",
                    LastName = "Kowalski",
                    City = "Glasgow",
                    Street = "Springhill Gardens",
                    PostCode = "G41 2EX",
                    DOB = new DateOnly(1985, 9, 17),
                    Gender = Gender.Male,
                    TotalRating = 4,
                    EmailConfirmed = true,
                    LicenceNumber = "KOWAL9876543219",
                    isApproved = true,
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = new List<string> { "Patient", "Calm" },
                    InstructorDrivingGoals = new List<string> { "Gain driving confidence", "Pass quickly" },
                    InstructorTeachingType = new List<string> { "Automatic" },
                    InstructorAvailableDaysOf = new List<string> { "Monday", "Thursday", "Saturday" },
                    InstructorTimeOfDay = new List<string> { "Morning", "Afternoon" },
                    InstructorLessonDuration = new int[] { 60, 90 },
                    InstructorLessonAverage = 75
                };
                await userManager.CreateAsync(instructorUser7, "pass123");
                await userManager.AddToRoleAsync(instructorUser7, "Instructor");
            }

            if (!context.Users.Any(u => u.UserName == "instructor8@example.com"))
            {
                var instructorUser8 = new Instructor
                {
                    UserName = "instructor8@example.com",
                    Email = "instructor8@example.com",
                    FirstName = "Kwame",
                    LastName = "Osei",
                    City = "Glasgow",
                    Street = "Great Western Road",
                    PostCode = "G15 6NB",
                    DOB = new DateOnly(1980, 6, 25),
                    Gender = Gender.Male,
                    TotalRating = 4,
                    EmailConfirmed = true,
                    LicenceNumber = "OSEIK8765432198",
                    isApproved = true,
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = new List<string> { "Patient", "Calm" },
                    InstructorDrivingGoals = new List<string> { "Gain driving confidence", "Pass quickly" },
                    InstructorTeachingType = new List<string> { "Manual", "Automatic" },
                    InstructorAvailableDaysOf = new List<string> { "Wednesday", "Friday", "Sunday" },
                    InstructorTimeOfDay = new List<string> { "Afternoon", "Evening" },
                    InstructorLessonDuration = new int[] { 60 },
                    InstructorLessonAverage = 60
                };
                await userManager.CreateAsync(instructorUser8, "pass123");
                await userManager.AddToRoleAsync(instructorUser8, "Instructor");
            }

            if (!context.Users.Any(u => u.UserName == "instructor9@example.com"))
            {
                var instructorUser9 = new Instructor
                {
                    UserName = "instructor9@example.com",
                    Email = "instructor9@example.com",
                    FirstName = "Sophia",
                    LastName = "Papadopoulos",
                    City = "Glasgow",
                    Street = "Gallowgate",
                    PostCode = "G1 5DY",
                    DOB = new DateOnly(1992, 9, 15),
                    Gender = Gender.Female,
                    TotalRating = 4,
                    EmailConfirmed = true,
                    LicenceNumber = "PAPAD1234567890",
                    isApproved = true,
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = new List<string> { "Patient", "Calm" },
                    InstructorDrivingGoals = new List<string> { "Gain driving confidence", "Pass quickly" },
                    InstructorTeachingType = new List<string> { "Manual", "Automatic" },
                    InstructorAvailableDaysOf = new List<string> { "Thursday", "Saturday", "Sunday" },
                    InstructorTimeOfDay = new List<string> { "Morning", "Afternoon" },
                    InstructorLessonDuration = new int[] { 60 },
                    InstructorLessonAverage = 60
                };
                await userManager.CreateAsync(instructorUser9, "pass123");
                await userManager.AddToRoleAsync(instructorUser9, "Instructor");
            }

            if (!context.Users.Any(u => u.UserName == "instructor10@example.com"))
            {
                var instructorUser10 = new Instructor
                {
                    UserName = "instructor10@example.com",
                    Email = "instructor10@example.com",
                    FirstName = "Musa",
                    LastName = "Abdullah",
                    City = "East Kilbride",
                    Street = "Hamilton Road",
                    PostCode = "G75 9EL",
                    DOB = new DateOnly(1985, 6, 23),
                    Gender = Gender.Male,
                    TotalRating = 3,
                    EmailConfirmed = true,
                    LicenceNumber = "ABDUL10987654321",
                    isApproved = true,
                    isBlocked = false,
                    InstructorDrivingStatus = DrivingStatus.Experienced,
                    InstructorTeachingTraits = new List<string> { "Friendly", "Patient" },
                    InstructorDrivingGoals = new List<string> { "Enhance driving skills", "Ensure safety" },
                    InstructorTeachingType = new List<string> { "Manual" },
                    InstructorAvailableDaysOf = new List<string> { "Monday", "Wednesday", "Friday" },
                    InstructorTimeOfDay = new List<string> { "Afternoon", "Evening" },
                    InstructorLessonDuration = new int[] { 90 },
                    InstructorLessonAverage = 75
                };
                await userManager.CreateAsync(instructorUser10, "pass123");
                await userManager.AddToRoleAsync(instructorUser10, "Instructor");
            }

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
                    PostCode = "G2 4HG",
                    DOB = new DateOnly(1997, 3, 13),
                    Gender = Gender.Female,
                    EmailConfirmed = true,
                    LicenceNumber = "WILLI12345678901"
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
                    FirstName = "Jack",
                    LastName = "Smith",
                    City = "Glasgow",
                    Street = "Renfrew Street",
                    PostCode = "G3 6TT",
                    DOB = new DateOnly(1995, 8, 17),
                    Gender = Gender.Male,
                    EmailConfirmed = true,
                    LicenceNumber = "SMITH54321987650"
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
                    PostCode = "G1 3LA",
                    DOB = new DateOnly(1967, 3, 26),
                    Gender = Gender.Male,
                    EmailConfirmed = true,
                    LicenceNumber = "THOMA87654321905"
                };
                await userManager.CreateAsync(learnerUser3, "pass123");
                await userManager.AddToRoleAsync(learnerUser3, "Learner");
            }

            if (!context.Users.Any(u => u.UserName == "learner4@example.com"))
            {
                var learnerUser4 = new Learner
                {
                    UserName = "learner4@example.com",
                    Email = "learner4@example.com",
                    FirstName = "Aisha",
                    LastName = "Khan",
                    City = "Glasgow",
                    Street = "Ruskin Terrace",
                    PostCode = "G12 8DY",
                    DOB = new DateOnly(1998, 2, 25),
                    Gender = Gender.Female,
                    EmailConfirmed = true,
                    LicenceNumber = "KHAN76543219876"
                };
                await userManager.CreateAsync(learnerUser4, "pass123");
                await userManager.AddToRoleAsync(learnerUser4, "Learner");
            }

            if (!context.Users.Any(u => u.UserName == "learner5@example.com"))
            {
                var learnerUser5 = new Learner
                {
                    UserName = "learner5@example.com",
                    Email = "learner5@example.com",
                    FirstName = "Mohammed",
                    LastName = "Al-Farsi",
                    City = "Glasgow",
                    Street = "Shettleston Road",
                    PostCode = "G32 7BW",
                    DOB = new DateOnly(1996, 9, 12),
                    Gender = Gender.Male,
                    EmailConfirmed = true,
                    LicenceNumber = "ALFAR367321D9B5G1X"
                };
                await userManager.CreateAsync(learnerUser5, "pass123");
                await userManager.AddToRoleAsync(learnerUser5, "Learner");
            }

            if (!context.Users.Any(u => u.UserName == "learner6@example.com"))
            {
                var learnerUser6 = new Learner
                {
                    UserName = "learner6@example.com",
                    Email = "learner6@example.com",
                    FirstName = "Leila",
                    LastName = "Garcia",
                    City = "East Kilbride",
                    Street = "Maxwellton Avenue",
                    PostCode = "G74 3DY",
                    DOB = new DateOnly(1997, 5, 18),
                    Gender = Gender.Female,
                    EmailConfirmed = true,
                    LicenceNumber = "LEILA268431X8D2B9X"
                };
                await userManager.CreateAsync(learnerUser6, "pass123");
                await userManager.AddToRoleAsync(learnerUser6, "Learner");
            }

            var instructor2 = context.Instructors.Where(r => r.Email == "instructor2@example.com").FirstOrDefault();
            var learner= context.Learners.Where(r => r.Email == "learner@example.com").FirstOrDefault();

            int numberOfLessons = 5;
       

          
            for (int i = 0; i < numberOfLessons; i++)
            {
                var lesson = new DrivingClass
                {
                    DrivingClassStart = DateTime.UtcNow.AddHours(i),
                    DrivingClassEnd = DateTime.UtcNow.AddHours(i + 1),
                    InstructorId = instructor2.Id,
                    Price = 22,
                    IsReserved = false,
                };

                context.DrivingClasses.Add(lesson);
            }
            context.SaveChanges();

            var instructor3 = context.Instructors.Where(r => r.Email == "instructor3@example.com").FirstOrDefault();

            for (int i = 0; i < numberOfLessons; i++)
            {
                var lesson = new DrivingClass
                {
                    DrivingClassStart = DateTime.UtcNow.AddHours(i),
                    DrivingClassEnd = DateTime.UtcNow.AddHours(i + 1),
                    InstructorId = instructor3.Id,
                    Price = 22,
                    IsReserved = false,
                };

                context.DrivingClasses.Add(lesson);
            }
            context.SaveChanges();

            var instructor4= context.Instructors.Where(r => r.Email == "instructor4@example.com").FirstOrDefault();

            for (int i = 0; i < numberOfLessons; i++)
            {
                var lesson = new DrivingClass
                {
                    DrivingClassStart = DateTime.UtcNow.AddHours(i+25),
                    DrivingClassEnd = DateTime.UtcNow.AddHours(i + 26),
                    InstructorId = instructor4.Id,
                    Price = 22,
                    IsReserved = false,
                };

                context.DrivingClasses.Add(lesson);
            }
            context.SaveChanges();


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
}


