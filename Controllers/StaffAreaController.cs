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
    [Authorize(Roles = "Staff,Admin")]
    public class StaffAreaController : Controller
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
        public StaffAreaController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DrivePalDbContext context, IUserStore<User> userStore, IEmailSender emailSender, ILogger<RegisterStaffViewModel> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _userStore = userStore;
            _logger = logger;
            _emailSender = emailSender;
        }
        public ActionResult Index()
        {
            var drivePalDbContext = _context.Instructors.Where(r=>r.isApproved.Equals(false)).ToList();
            var count= drivePalDbContext.Count();
            var reportCount = _context.Reports.Where(r => r.isProccessed.Equals(false)).Count();
            ViewBag.Count=count;
            ViewBag.ReportCount = reportCount;
            

            return View(drivePalDbContext.ToList());
        }
        public ActionResult AllInstructors()// view of all instructors
        {
            var drivePalDbContext = _context.Instructors.ToList();
            var nonApprovedCount = drivePalDbContext.Count();
            ViewBag.Count = nonApprovedCount;
            var reviews = _context.Reviews.Where(r => r.isFlagged.Equals(true)).Count();
            ViewBag.Reviews = reviews;

            return View(drivePalDbContext.ToList());
        }

        public ActionResult AllLeaners()// view of all instructors
        {
            var leaners= _context.Learners.ToList();
       
      

            return View(leaners);
        }

        // GET: StaffAreaController/Details/5
        public ActionResult ViewReviews()
        {
          
            
             var reports = _context.Reports.Where(r => r.isProccessed == false).ToList();
           
            
           
            var count = _context.Reports.Where(r => r.isProccessed.Equals(false)).Count();
            ViewBag.Count = count;

            return View(reports);
        }
        public ActionResult ReportDetails(int id)
        {


            var report = _context.Reports.Include(r=>r.Review).Include(r=>r.Instructor).Where(r => r.ReportId == id).FirstOrDefault();



           

            return View(report);
        }

        public ActionResult FlagReview(int reviewId, int reportId)
        {

            //gets the report and review and associated instructor through the route
            var report = _context.Reports.Include(r => r.Review).Where(r => r.ReportId == reportId).FirstOrDefault();
            var review = _context.Reviews.Where(r => r.ReviewId == reviewId).FirstOrDefault();
            var instructor=_context.Instructors.Where(r=>r.Id==review.InstructorId).FirstOrDefault();
           
            review.isFlagged = true; //changing the review to isflagged so no longer visible
            report.isProccessed=true;
            _context.Update(review);
            _context.Update(report);
            
            _context.SaveChanges();
            var newRating = UpdateInstructorRatingAfterReviewRemoval(instructor.Id); //removing the rating from the associated review
            //updating and saving changes
            instructor.TotalRating = newRating;
            _context.Update(instructor);
            _context.SaveChanges();
            


            return RedirectToAction("ViewReviews");
        }
        public ActionResult DismissReport(int id)
        {

            //gets report
            var report = _context.Reports.Include(r => r.Review).Where(r => r.ReportId == id).FirstOrDefault();
            
            //nothing wrong with the report so is dismissed.
          
            report.isProccessed = true;
          
            _context.Update(report);
            _context.SaveChanges();



            return RedirectToAction("ViewReviews");
        }



        // GET: StaffAreaController/Create
        [HttpGet]
        public async Task<IActionResult> ApproveInstructor(string? id)
        {
          
            var user = await _userManager.FindByIdAsync(id);

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
        public async Task<IActionResult> ApproveInstructor([Bind("Email, FirstName, LastName, Street, PostCode, DOB, City, LicenceNumber, isBlocked, isApproved, TotalRating")] InstructorEditOwnDetails model)
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
                    instructor.isBlocked = false;
                    instructor.isApproved = true;
                    instructor.TotalRating = model.TotalRating;

                    IdentityResult result = await _userManager.UpdateAsync(instructor);

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

        // POST: StaffAreaController/Create
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

        // GET: StaffAreaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StaffAreaController/Edit/5
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

        // GET: StaffAreaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StaffAreaController/Delete/5
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
        private decimal UpdateInstructorRatingAfterReviewRemoval(string instructorId)
        {
            var validReviews = _context.Reviews
                                       .Where(r => r.InstructorId == instructorId && !r.isFlagged)
                                       .ToList(); // Gets all unflagged reviews of the instructor.

            decimal totalRating = 0;
            foreach (var review in validReviews) // Loops through each valid review adding ratings together
            {
                totalRating += review.Rating;
            }

            decimal updatedAverageRating = 0;
            if (validReviews.Any()) // Check if there are any valid reviews left
            {
                updatedAverageRating = totalRating / validReviews.Count; // Calculates the new average rating
            }

            return updatedAverageRating; // If no reviews left, average rating remains 0
        }
    }
}
