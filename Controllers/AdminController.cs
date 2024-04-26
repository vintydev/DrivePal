using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using DrivePal.Models;
using DrivePal.Data;
using DrivePal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using DrivePal.Extensions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DrivePal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    { 
        //Global variables
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterStaffViewModel> _logger;
        private readonly DrivePalDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly IEmailSender _emailSender;
        

        // Admin Constructor with dependency injections
        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DrivePalDbContext context, IUserStore<User> userStore, IEmailSender emailSender, ILogger<RegisterStaffViewModel> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _userStore = userStore;
            _logger = logger;

            _emailSender = emailSender;
        }

      


          public async Task<IActionResult> Index()
        {
            var drivePalDbContext = _context.Staffs;
            return View( drivePalDbContext.ToList());
        }
        public async Task<IActionResult> Learners()
        {
            var drivePalDbContext = _context.Learners;
            return View(drivePalDbContext.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> EditLearner(string id)
        {
            var user = _context.Learners.Where(r => r.Id == id).FirstOrDefault();
            ViewBag.id = id;

            var genders = Enum.GetValues(typeof(DrivePal.Models.Gender))
                                  .Cast<Models.Gender>()
                                  .Select(e => new SelectListItem
                                  {
                                      Value = ((int)e).ToString(), // Storing the enum integer value as the value (optional)
                                      Text = e.GetDisplayName() // Assuming you use the same extension method to get the display name
                                  })
                                  .ToList();

            ViewBag.Gender = new SelectList(genders, "Value", "Text");

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
        public async Task<IActionResult> EditLearner([Bind("Email, FirstName, LastName, Street, PostCode, DOB, City, LicenceNumber")] LearnerEditOwnDetails model)
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
                        return RedirectToAction("Learners");
                    }
                    // Handle the errors if update is not successful
                }
            }

            // If model state is not valid or update failed, show the form again with validation messages
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            {
                var user = await _context.Users
                                         .Where(u => u.Id == id)
                                         .FirstOrDefaultAsync();

                if (user == null)
                {
                   
                    return RedirectToAction("ErrorPage", "Home"); 
                }

                // Remove user from any roles they might be in
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    var result = await _userManager.RemoveFromRolesAsync(user, roles);
                    if (!result.Succeeded)
                    {
                        // Handle the case where removing the roles fails
                        // Log the errors or display to the user
                        return RedirectToAction("Index", "Home");
                    }
                }

                // Attempt to delete the user
                IdentityResult resultDelete = await _userManager.DeleteAsync(user);
                if (resultDelete.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                // If deletion wasn't successful, handle errors here
                foreach (var error in resultDelete.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                // You could return to a view that shows these errors or logs them
                return RedirectToAction("Index", "Home");
            }
        }

            public async Task<IActionResult> Instructors()
        {
            var drivePalDbContext = _context.Instructors;
            return View(drivePalDbContext.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> EditInstructor(string id)
        {
         var user=_context.Instructors.Where(r=>r.Id == id).FirstOrDefault();
            ViewBag.id = id;

            var genders = Enum.GetValues(typeof(DrivePal.Models.Gender))
                                .Cast<Models.Gender>()
                                .Select(e => new SelectListItem
                                {
                                    Value = ((int)e).ToString(), // Storing the enum integer value as the value (optional)
                                    Text = e.GetDisplayName() // Assuming you use the same extension method to get the display name
                                })
                                .ToList();

            ViewBag.Gender = new SelectList(genders, "Value", "Text");

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
        public async Task<IActionResult> EditInstructor([Bind("Email, FirstName, LastName, Street, PostCode, DOB, City, LicenceNumber, isBlocked, isApproved, TotalRating")] InstructorEditOwnDetails model)
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
                        return RedirectToAction("Instructors");
                    }
                    // Handle the errors if update is not successful
                }
            }

            // If model state is not valid or update failed, show the form again with validation messages
            return View(model);
        }

        public async Task<IActionResult> Bookings()
        {
            var drivePalDbContext = _context.Bookings;
            return View(drivePalDbContext.ToList());
        }


        public IActionResult RegisterStaff()
        {
            ViewData["Title"] = "Create Staff Account";

            var workTypes = Enum.GetValues(typeof(DrivePal.Models.ViewModels.WorkType))
                                .Cast<Models.ViewModels.WorkType>()
                                .Select(e => new SelectListItem
                                {
                                    Value = ((int)e).ToString(), // Storing the enum integer value as the value (optional)
                                    Text = e.GetDisplayName() // Assuming you use the same extension method to get the display name
                                })
                                .ToList();

            ViewBag.WorkTypeSelectList = new SelectList(workTypes, "Value", "Text");
            var genders = Enum.GetValues(typeof(DrivePal.Models.Gender))
                              .Cast<Models.Gender>()
                              .Select(e => new SelectListItem
                              {
                                  Value = ((int)e).ToString(), // Storing the enum integer value as the value (optional)
                                  Text = e.GetDisplayName() // Assuming you use the same extension method to get the display name
                              })
                              .ToList();

            ViewBag.Gender = new SelectList(genders, "Value", "Text");

            return View(new RegisterStaffViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> RegisterStaff(RegisterStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validating the model
                var staff = Activator.CreateInstance<Staff>();

                // Set the additional properties
                staff.FirstName = model.FirstName;
                staff.LastName = model.LastName;
                staff.Street = model.Street;
                staff.City = model.City;
                staff.PostCode = model.PostCode;
                staff.DOB=model.DOB;
                staff.Email=model.Email;
                staff.Gender = model.Gender;

                // Setting the username for the user
                await _userStore.SetUserNameAsync(staff, model.Email, CancellationToken.None);

                // Creating the user with the provided password
                var result = await _userManager.CreateAsync(staff, model.Password);

                if (result.Succeeded)
                {
                    // If the user creation is successful

                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(staff);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new Learner account with password.");

                        // Confirm the email directly without sending a confirmation email

                        staff.EmailConfirmed = true;
                        var confirmResult = await _userManager.UpdateAsync(staff);

                        if (confirmResult.Succeeded)
                        {
                            _logger.LogInformation("User's email (Staff) confirmed successfully.");

                            // Add the user to the Learner role
                            var addToRoleResult = await _userManager.AddToRoleAsync(staff, "Staff");
                            if (addToRoleResult.Succeeded)
                            {
                                _logger.LogInformation("User added to Staff role successfully.");
                            }
                            else
                            {
                                foreach (var error in addToRoleResult.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                                // If adding to role failed, return the page to display the errors
                                return View();
                            }

                        }

                    }

                    // If user creation failed, add model errors to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

               
                return RedirectToAction("Index");
            }
            // If ModelState is invalid, redisplay the form
            return View();
        }
        public async Task<IActionResult> EditStaff(string id)
        {
            var user = _context.Staffs.Where(r => r.Id == id).FirstOrDefault();
            ViewBag.id = id;
            var workTypes = Enum.GetValues(typeof(DrivePal.Models.ViewModels.WorkType))
                               .Cast<Models.ViewModels.WorkType>()
                               .Select(e => new SelectListItem
                               {
                                   Value = ((int)e).ToString(), // Storing the enum integer value as the value (optional)
                                   Text = e.GetDisplayName() // Assuming you use the same extension method to get the display name
                               })
                               .ToList();

            var genders = Enum.GetValues(typeof(DrivePal.Models.Gender))
                              .Cast<Models.Gender>()
                              .Select(e => new SelectListItem
                              {
                                  Value = ((int)e).ToString(), // Storing the enum integer value as the value (optional)
                                  Text = e.GetDisplayName() // Assuming you use the same extension method to get the display name
                              })
                              .ToList();

            ViewBag.WorkTypeSelectList = new SelectList(workTypes, "Value", "Text");
            ViewBag.Gender = new SelectList(genders, "Value", "Text");


            if (user is Staff staff)
            {
                var viewModel = new EditStaffViewModel
                {
                    Email=user.Email,
                    FirstName =user.FirstName,
                    LastName = user.LastName,
                    Street = user.Street,
                    PostCode = user.PostCode,
                    DOB = user.DOB,
                    City = user.City,
                    WorkType= (Models.ViewModels.WorkType)user.WorkType,
                    Gender= (Gender)user.Gender
                   
                   
                };

                return View(viewModel);
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EditStaff([Bind("Email, FirstName, LastName, Street, PostCode, DOB, City,WorkType")] EditStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is Staff staff)
                {
                    // Map the properties from the ViewModel to the staff entity
                    staff.FirstName = model.FirstName;
                    staff.LastName = model.LastName;
                    staff.Street = model.Street;
                    staff.PostCode = model.PostCode;
                    staff.DOB = model.DOB;
                    staff.City = model.City;
                    staff.WorkType = (Models.WorkType)model.WorkType;
                    staff.Gender= (Gender)model.Gender;
                    
                   
                    // Map other properties as needed

                    // Update the staff entity
                    IdentityResult result = await _userManager.UpdateAsync(staff);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    // Handle the errors if update is not successful
                }
            }

            // If model state is not valid or update failed, show the form again with validation messages
            return View(model);
        }



    }
    
}
