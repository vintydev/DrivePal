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

namespace DrivePal.Controllers
{
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

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
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

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

       
    }
    
}
