using DrivePal.Data;
using DrivePal.Models.ViewModels;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DrivePal.Controllers
{
    public class InstructorDataController : Controller
    {
        // Database context for interacting with the application database
        private DrivePalDbContext _context;
        // User manager for managing user-related operations
        private UserManager<User> _userManager;
        // Sign-in manager for managing user authentication
        private SignInManager<User> _signInManager;
        // Role manager for managing user roles
        private RoleManager<IdentityRole> _roleManager;

        private ILogger<InstructorDataController> _logger;

        public InstructorDataController(ILogger<InstructorDataController> logger, DrivePalDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> InstructorSeeOwnDetails()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);

            // Checks if the user has the "Customer" role
            if (user is Instructor instructor && await _userManager.IsInRoleAsync(user, "Instructor"))
            {
                //loads the data
                var viewModel = new InstructorSeeOwnDetails
                {
                    Email = instructor.Email,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    City = instructor.City,
                    Street = instructor.Street,
                    PostCode = instructor.PostCode,
                    DOB = instructor.DOB,
                    LicenceNumber = instructor.LicenceNumber,
                    Gender = instructor.Gender,
                    isBlocked = instructor.isBlocked,
                    isApproved = instructor.isApproved,
                };
                //returns the view
                return View(viewModel);
            }

            //if it does not work, display an error page
            return RedirectToAction("Error", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> InstructorEditOwnDetails()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user is Instructor instructor)
            {
                var viewModel = new InstructorEditOwnDetails
                {
                    Email = instructor.Email,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    City = instructor.City,
                    Street = instructor.Street,
                    PostCode = instructor.PostCode,
                    DOB = instructor.DOB,
                    LicenceNumber = instructor.LicenceNumber,
                    Gender = instructor.Gender,
                    isBlocked = instructor.isBlocked,
                    isApproved = instructor.isApproved,
                };

                return View(viewModel);
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> InstructorEditOwnDetails([Bind("Email, FirstName, LastName, Street, PostCode, DOB, City, LicenceNumber, isBlocked, isApproved, TotalRating")] InstructorEditOwnDetails model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is Instructor instructor)
                {
                    // Map the properties from the ViewModel to the Learner entity
                    instructor.FirstName = model.FirstName;
                    instructor.LastName = model.LastName;
                    instructor.Street = model.Street;
                    instructor.PostCode = model.PostCode;
                    instructor.DOB = model.DOB;
                    instructor.City = model.City;
                    instructor.LicenceNumber = model.LicenceNumber;
                    instructor.Gender = model.Gender;
                    instructor.isBlocked = model.isBlocked;
                    instructor.isApproved = model.isApproved;
                    instructor.TotalRating = model.TotalRating;
                    
                    IdentityResult result = await _userManager.UpdateAsync(instructor);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("InstructorSeeOwnDetails");
                    }
                    // Handle the errors if update is not successful
                }
            }

            // If model state is not valid or update failed, show the form again with validation messages
            return View(model);
        }




    }
}
