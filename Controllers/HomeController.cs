using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using DrivePal.Data;
using DrivePal.Extensions;
using DrivePal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace DrivePal.Controllers
{
    public class HomeController : Controller
    {
        private readonly DrivePalDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, DrivePalDbContext context, UserManager<User> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult QuestionnaireIndex(string? id)
        {
            var model = new Questionnaire();

            var experienceType = Enum.GetValues(typeof(DrivingStatus))
                .Cast<Models.DrivingStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.GetDisplayName()
                })
                .ToList();

            var drivingType = Enum.GetValues(typeof(DriveType))
                .Cast<DriveType>()
                .Select(e => new SelectListItem()
                {
                    Value = ((int)e).ToString(),
                    Text = e.GetDisplayName()
                }).ToList();

            ViewBag.DriveSelectList = new SelectList(drivingType, "Value", "Text");
            ViewBag.ExperienceSelectList = new SelectList(experienceType, "Value", "Text");

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestionnaire([Bind(
                "QuestionnaireId,MinPrice,MaxPrice,DrivingStatus,TeachingTraits,DrivingGoals,TeachingType," +
                "AvailableDaysOf,TimeOfDay,LessonDuration,IsFinished,DateCompleted,LearnerId,Learner")]
            Questionnaire questionnaire, List<string> teachingTraits, List<string> selectedGoals,
            List<string> selectedDays)
        {
            // Get user
            var userName = User.Identity?.Name;

            var learner = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName) as Learner;

            // Assign learner
            questionnaire.Learner = learner;

            questionnaire.LearnerId = learner?.Id;

            // Collect error messages list initialisation
            List<string> errorMessages = new List<string>(3);
            int errorIndex = 0;
            int? dayIndex = null;
            int? goalIndex = null;
            int? traitIndex = null;

            // Add the error messages to the list
            if (selectedDays.Count == 0)
            {
                dayIndex = errorIndex;


                errorMessages.Add("You must select at least one day");
                errorIndex++;
            }

            if (selectedGoals.Count == 0)
            {
                goalIndex = errorIndex;


                errorMessages.Add("You must select at least one goal.");
                errorIndex++;
            }

            if (teachingTraits.Count == 0)
            {
                traitIndex = errorIndex;

                errorMessages.Add("You must select at least one trait.");
                errorIndex++;
            }


            // Redirects with validation errors
            if (!ModelState.IsValid || errorMessages.Count != 0)
            {
                // Create new questionnaire with default values
                Questionnaire questionnaire2 = new Questionnaire
                {
                    TraitIndex = traitIndex,
                    GoalIndex = goalIndex,
                    DayIndex = dayIndex
                };

                ViewBag.ErrorMessages = errorMessages;

                return View("QuestionnaireIndex", questionnaire2);
            }

            // Add properties
            questionnaire.IsFinished = true;

            questionnaire.DateCompleted = DateTime.Now;

            questionnaire.AvailableDaysOf = selectedDays;

            questionnaire.DrivingGoals = selectedGoals;

            questionnaire.TeachingTraits = teachingTraits;

            // Add questionnaire to db, save changes, redirect to Index action
            await _context.AddAsync(questionnaire);

            await _context.SaveChangesAsync();

            // Querying instructors based on filtering criteria
            var matchingInstructors =
                await FilterInstructors(questionnaire, teachingTraits, selectedGoals, selectedDays);

            // Return the list of matching instructors to the view
            return View("SeeAllInstructors", matchingInstructors);
        }

        private async Task<List<InstructorsDetailsViewModel>> FilterInstructors(Questionnaire questionnaire,
            List<string> teachingTraits, List<string> selectedGoals, List<string> selectedDays)
        {
            // Load all instructors into memory
            var allInstructors = await _context.Instructors.ToListAsync();

            // Filter instructors that contain any match
            var matchingInstructors = allInstructors.Where(instructor =>
                GetAveragePricePerLesson(instructor.Id) >= questionnaire.MinPrice &&
                GetAveragePricePerLesson(instructor.Id) <= questionnaire.MaxPrice &&
                // instructor.InstructorDrivingStatus == questionnaire.DrivingStatus && (fix later, redundant rn)
                instructor.isApproved == true && instructor.isBlocked == false &&
                (
                    instructor.InstructorTeachingTraits.Any(trait => teachingTraits.Contains(trait)) ||
                    instructor.InstructorDrivingGoals.Any(goal => selectedGoals.Contains(goal)) ||
                    instructor.InstructorTeachingType.Any(type => questionnaire.TeachingType.Contains(type)) ||
                    instructor.InstructorAvailableDaysOf.Any(day => selectedDays.Contains(day)) ||
                    instructor.InstructorTimeOfDay.Any(time => questionnaire.TimeOfDay.Contains(time)) ||
                    instructor.InstructorLessonDuration.Any(duration =>
                        questionnaire.LessonDuration.Contains(duration))
                )).ToList();

            // Create InstructorsDetailsViewModel objects for matching instructors
            var matchingInstructorsDetailsViewModel = matchingInstructors.Select(instructor =>
                new InstructorsDetailsViewModel()
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    PostCode = instructor.PostCode,
                    TotalRating = instructor.TotalRating,
                    InstructorDaysAvailable = instructor.InstructorAvailableDaysOf,
                    InstructorTimeAvaiable = instructor.InstructorTimeOfDay,
                    AveragePrice = GetAveragePricePerLesson(instructor.Id)
                }).ToList();

            return matchingInstructorsDetailsViewModel;
        }


        public async Task<IActionResult> SeeAllInstructors()
        {
             var allInstructors = await _context.Instructors.ToListAsync();
            
            var instructors = allInstructors
                .Where(i => i.isApproved == true && i.isBlocked == false)
                .Select(i => new InstructorsDetailsViewModel
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    PostCode = i.PostCode,
                    TotalRating = i.TotalRating,
                    ProfilePicture = i.ProfilePicture,
                    AveragePrice = GetAveragePricePerLesson(i.Id)
                })
                .ToList();

            return View(instructors);
        }

        public IActionResult Instructors()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Resources()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        public IActionResult EditProfile()
        {
            var user = _userManager.GetUserAsync(User).Result;
            ViewBag.ProfilePicture = user.ProfilePicture;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfileChanges(User updatedProfile, IFormFile profilePicture)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (profilePicture != null)
                {
                    var fileName = $"{updatedProfile.FirstName}{updatedProfile.LastName}{Path.GetExtension(profilePicture.FileName)}";
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img/profilepictures", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilePicture.CopyToAsync(stream);
                    }

                    user.ProfilePicture = $"/img/profilepictures/{fileName}";
                }

                // Update the mutable fields
                user.Street = updatedProfile.Street;
                user.City = updatedProfile.City;
                user.PostCode = updatedProfile.PostCode;
                user.PhoneNumber = updatedProfile.PhoneNumber;  // Assuming User class has a PhoneNumber property

                // Update only specific user types
                if (user is Learner updatedLearner)
                {
                    var learner = user as Learner;
                    learner.isExperienced = updatedLearner.isExperienced;
                    await _userManager.UpdateAsync(learner);
                }
                else if (user is Instructor updatedInstructor)
                {
                    var instructor = user as Instructor;
                    instructor.isApproved = updatedInstructor.isApproved;
                    instructor.LicenceNumber = updatedInstructor.LicenceNumber;
                    await _userManager.UpdateAsync(instructor);
                }

                return RedirectToAction("Profile");
            }

            return View("EditProfile", updatedProfile);
        }



        [Authorize] // Ensure the user is logged in
        public async Task<IActionResult> GetUserPostcode()
        {
            // Get the currently logged in user
            var user = await _userManager.GetUserAsync(User);

            // Check if the user and the postcode exist
            if (user != null && !string.IsNullOrEmpty(user.PostCode))
            {
                // Return the user's postcode
                return Ok(user.PostCode);
            }

            // If the user or the postcode doesn't exist, return an error
            return NotFound("User or postcode not found");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Calendar()
        {
            return View();
        }

        // Method to get average price per lesson for a given instructor ID
        private decimal GetAveragePricePerLesson(string instructorId)
        {
            // Get all bookings for the instructor
            var instructorsLessons = _context.DrivingClasses.Where(b => b.InstructorId == instructorId).ToList();

            if (instructorsLessons.Count == 0)
            {
                // Return the default value if there are no bookings
                return _context.Instructors.FirstOrDefault(i => i.Id == instructorId)?.InstructorLessonAverage ?? 0;
            }

            // Use LINQ to calculate the average price
            var totalPrice = instructorsLessons.Sum(lesson => lesson.Price);

            // Return the average price
            return totalPrice / instructorsLessons.Count;
        }
    }
}