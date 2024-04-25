using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using DrivePal.Models.ViewModels;

namespace DrivePal.Controllers
{
    public class ReportsController : Controller
    {
        private readonly DrivePalDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportsController(DrivePalDbContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            var reports=_context.Reports.Where(r=>r.isProccessed==false).ToList();
            return View(reports);
        }
        [HttpGet]
        public IActionResult Report(int id)
        {
          
            var review = _context.Reviews.Where(r => r.ReviewId == id).Include(r=>r.Instructor).Include(r=>r.Learner).FirstOrDefault();

            var report = new ReportsViewModel()
            {
                ReviewMessage=review.ReviewMessage,
                Instructor= review.Instructor,
                Rating=review.Rating,
                Learner=review.Learner,
                DateCreated=review.DateCreated,
                Review=review,

            };



            return View(report);
        }
        [HttpPost]
        public IActionResult Report(ReportsViewModel model) 
        {
           
            var review = _context.Reviews.Where(r => r.ReviewId == model.Review.ReviewId).Include(r => r.Instructor).Include(r => r.Learner).FirstOrDefault();
            var reporterId = GetUserId();
            var report = new Report()
            {
                ReportMessage=model.ReportMessage,
                ReportReason=model.ReportReason,
                ReviewId=review.ReviewId,
                Review=review,
                Instructor=review.Instructor,
                ReporterId=reporterId,
                isProccessed=false,
                DateCreated=DateTime.Now,

            };
          
         
            _context.Reports.Add(report);
         
            _context.SaveChanges();

            return RedirectToAction("SeeAllInstructors","Home");
            
            
        }


        private string GetUserId() //Finds logged in userId.
        {
            // Access the current user's information from the HttpContext
            var principal = _httpContextAccessor.HttpContext.User;

            // Use the UserManager to get the user ID from the principal
            string userId = _userManager.GetUserId(principal);

            // Return the user ID
            return userId;
        }
    }
}
