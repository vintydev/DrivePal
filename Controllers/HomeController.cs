using DrivePal.Models;
using Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DrivePal.Data;
using DrivePal.Extensions;
using DrivePal.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DrivePal.Controllers
{
    public class HomeController : Controller
    {

        // Database context for interacting with the application database
        private DrivePalDbContext _context;
        // User manager for managing user-related operations
        private UserManager<User> _userManager;
        // Sign-in manager for managing user authentication
        private SignInManager<User> _signInManager;
        // Role manager for managing user roles
        private RoleManager<IdentityRole> _roleManager;

        private ILogger<HomeController> _logger;
        
        // Constructor
        public HomeController(ILogger<HomeController> logger, DrivePalDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        
        // Only for learners
        [Authorize(Roles="Learner")]
        public ActionResult QuestionnaireIndex(string? id)
        {

            var model = new Questionnaire();
            
            // Retrieve all values of the DrivingStatus enum and convert them into an enumerable of DrivingStatus.
            var experienceType = Enum.GetValues(typeof(DrivingStatus))

                // Cast the enum values to the DrivingStatus enum type.
                .Cast<Models.DrivingStatus>()

                // For each enum value, create a SelectListItem object.
                .Select(e => new SelectListItem
                {
                    // Set the Value property of SelectListItem to the integer representation of the enum value converted to a string.
                    Value = ((int)e).ToString(),

                    // Set the Text property of SelectListItem to the display name of the enum value obtained using GetDisplayName() extension method.
                    Text = e.GetDisplayName()  // Assumes GetDisplayName() is an extension method for retrieving display names.
                })

                // Collect all SelectListItem objects into a list.
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
        public async Task<IActionResult> CreateQuestionnaire([Bind("QuestionnaireId,MinPrice,MaxPrice,DrivingStatus,TeachingTraits,DrivingGoals,TeachingType," +
                                                                   "AvailableDaysOf,TimeOfDay,LessonDuration,IsFinished,DateCompleted,LearnerId,Learner")]
            Questionnaire questionnaire, List<string> teachingTraits, List <string>selectedGoals, List<string> selectedDays)
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
            int? dayIndex;
            int? goalIndex;
            int? traitIndex;
            
            // Add the error messages to the list
            if (selectedDays.Count == 0)
            {
                dayIndex = errorIndex;
                questionnaire.DayIndex = dayIndex;
                
                errorMessages.Add("You must select at least one day");
                errorIndex++;
            }
            
            if (selectedGoals.Count == 0)
            {
                goalIndex = errorIndex;
                
                questionnaire.GoalIndex = goalIndex;
                
                errorMessages.Add("You must select at least one goal.");
                errorIndex ++;

            }

            if (teachingTraits.Count == 0)
            {
                traitIndex = errorIndex;
                questionnaire.TraitIndex = traitIndex;
                
                errorMessages.Add("You must select at least one trait.");
                errorIndex ++;

            }

            // If there are any error messages, set ViewBag and return the view with model
            if (errorMessages.Count != 0)
            {
                ViewBag.ErrorMessages = errorMessages;
                return View("QuestionnaireIndex", questionnaire);
            }
            
            // Redirects with validation errors
            if (!ModelState.IsValid) return View("QuestionnaireIndex",questionnaire);
            
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
            var matchingInstructors = await FilterInstructors(questionnaire, teachingTraits, selectedGoals, selectedDays);

            // Return the list of matching instructors to the view
            return View("SeeAllInstructors",matchingInstructors);
            
        }

        private async Task <List<InstructorsDetailsViewModel>> FilterInstructors(Questionnaire questionnaire,
            List<string> teachingTraits, List<string> selectedGoals, List<string> selectedDays)
        {
            // Filter instructors that contain any match
            var matchingInstructorsDetailsViewModel = _context.Instructors
                .Where(instructor =>
                    instructor.InstructorLessonAverage >= questionnaire.MinPrice &&
                    instructor.InstructorLessonAverage <= questionnaire.MaxPrice &&
                    instructor.InstructorDrivingStatus == questionnaire.DrivingStatus &&
                    instructor.isApproved == true && instructor.isBlocked == false &&
                    (
                        instructor.InstructorTeachingTraits.Any(trait => teachingTraits.Contains(trait)) ||
                        instructor.InstructorDrivingGoals.Any(goal => selectedGoals.Contains(goal)) ||
                        instructor.InstructorTeachingType.Any(type => questionnaire.TeachingType.Contains(type)) ||
                        instructor.InstructorAvailableDaysOf.Any(day => selectedDays.Contains(day)) ||
                        instructor.InstructorTimeOfDay.Any(time => questionnaire.TimeOfDay.Contains(time)) ||
                        instructor.InstructorLessonDuration.Any(duration => questionnaire.LessonDuration.Contains(duration))
                    ))
                .Select(instructor => new InstructorsDetailsViewModel()
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    PostCode = instructor.PostCode,
                    TotalRating = instructor.TotalRating,
                    InstructorDaysAvailable = instructor.InstructorAvailableDaysOf,
                    InstructorTimeAvaiable = instructor.InstructorTimeOfDay,
                    AveragePrice = instructor.InstructorLessonAverage
                })
                .ToListAsync();

            return await matchingInstructorsDetailsViewModel;
        }

        public async Task<IActionResult> SeeAllInstructors()
        {
            var instructors = await _context.Instructors
                .Where(i => i.isApproved == true && i.isBlocked == false) // Assuming you want to filter by these
                .Select(i => new InstructorsDetailsViewModel
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    PostCode = i.PostCode,
                    TotalRating = i.TotalRating
                })
                .ToListAsync();

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
            // Pass the user object to the view
            return View(user);
        }

        public IActionResult EditProfile()
        {
            // Retrieve the current user's details, including the date of birth
            var user = _userManager.GetUserAsync(User).Result;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfileChanges(Learner updatedProfile)
        {
            if (ModelState.IsValid)
            {
                // Get the current logged-in user
                var user = await _userManager.GetUserAsync(User);

                // Update the user's profile details using model binding
                user.FirstName = updatedProfile.FirstName;
                user.LastName = updatedProfile.LastName;
                user.Street = updatedProfile.Street;
                user.City = updatedProfile.City;
                user.PostCode = updatedProfile.PostCode;
                user.DOB = updatedProfile.DOB;
                user.Gender = updatedProfile.Gender;
                user.isBlocked = updatedProfile.isBlocked;
                user.isExperienced = updatedProfile.isExperienced;

                // Save changes to the database
                await _userManager.UpdateAsync(user);

                // Redirect the user to their profile page
                return RedirectToAction("Profile");
            }

            // If the model state is not valid, return to the edit profile page
            return View("EditProfile", updatedProfile);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
