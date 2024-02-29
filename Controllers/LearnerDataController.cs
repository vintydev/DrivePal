using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DrivePal.Models.ViewModels;

namespace DrivePal.Controllers
{
    public class LearnerDataController : Controller
    {
        // Database context for interacting with the application database
        private DrivePalDbContext _context;
        // User manager for managing user-related operations
        private UserManager<User> _userManager;
        // Sign-in manager for managing user authentication
        private SignInManager<User> _signInManager;
        // Role manager for managing user roles
        private RoleManager<IdentityRole> _roleManager;

        private ILogger<LearnerDataController> _logger;

        public LearnerDataController(ILogger<LearnerDataController> logger, DrivePalDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> LearnerSeeOwnDetails()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);

            // Checks if the user has the "Customer" role
            if (user is Learner learner && await _userManager.IsInRoleAsync(user, "Learner"))
            {
                //loads the data
                var viewModel = new LearnerSeeOwnDetails
                {
                    Email = learner.Email,
                    FirstName = learner.FirstName,
                    LastName = learner.LastName,
                    City = learner.City,
                    Street = learner.Street,
                    PostCode = learner.PostCode,
                    DOB = learner.DOB,
                    LicenceNumber = learner.LicenceNumber,
                    Gender = learner.Gender,
                    isBlocked = learner.isBlocked,
                    isExperienced = learner.isExperienced,
                    lessonCount = learner.lessonCount
                };
                //returns the view
                return View(viewModel);
            }

            //if it does not work, display an error page
            return RedirectToAction("Error", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> LearnerEditOwnDetails()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user is Learner learner)
            {
                var viewModel = new LearnerEditOwnDetails
                {
                    Email = learner.Email,
                    FirstName = learner.FirstName,
                    LastName = learner.LastName,
                    Street = learner.Street,
                    PostCode = learner.PostCode,
                    DOB = learner.DOB,
                    City = learner.City,
                    LicenceNumber = learner.LicenceNumber
                    // Populate other properties as needed
                };

                return View(viewModel);
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LearnerEditOwnDetails([Bind("Email, FirstName, LastName, Street, PostCode, DOB, City, LicenceNumber")] LearnerEditOwnDetails model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is Learner learner)
                {
                    // Map the properties from the ViewModel to the Learner entity
                    learner.FirstName = model.FirstName;
                    learner.LastName = model.LastName;
                    learner.Street = model.Street;
                    learner.PostCode = model.PostCode;
                    learner.DOB = model.DOB;
                    learner.City = model.City;
                    learner.LicenceNumber = model.LicenceNumber;
                    // Map other properties as needed

                    // Update the Learner entity
                    IdentityResult result = await _userManager.UpdateAsync(learner);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("LearnerSeeOwnDetails");
                    }
                    // Handle the errors if update is not successful
                }
            }

            // If model state is not valid or update failed, show the form again with validation messages
            return View(model);
        }




    }
}
