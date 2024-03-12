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

namespace DrivePal.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly DrivePalDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReviewsController(DrivePalDbContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var drivePalDbContext = _context.Instructors.Include(r => r.Reviews);
            return View(await drivePalDbContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> ViewReviews(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var instructor=_context.Instructors.Where(r=>r.Id==id).FirstOrDefault();
            ViewBag.Name=instructor.FirstName+" "+instructor.LastName;

            var reviews = _context.Reviews
            .Where(r => r.InstructorId == id)
            .Include(r => r.Learner)
            .Include(r => r.Instructor)
            .ToList();


            if (reviews == null)
            {
                return NotFound();
            }

            return View(reviews);
        }

        // GET: Reviews/Create
        [Authorize]
        public IActionResult Create(string? id)
        {
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Id");
            if (id == null)
            {
                return NotFound();
            }

            var instructor =  _context.Instructors.Where(r => r.Id == id).FirstOrDefault();
          
           
            ViewBag.Name= instructor.FirstName+" "+instructor.LastName;
            ViewBag.InstructorId=id;
            ViewBag.LearnerId = GetUserId();
            ViewBag.Instructor=instructor;

            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewId,Rating,ReviewMessage,DateCreated,InstructorId,LearnerId,Instructor")] Review review)
        {
            review.DateCreated = DateTime.Now;
            var instructor = _context.Instructors.Where(r => r.Id == review.InstructorId).FirstOrDefault();
            var learner = _context.Learners.Where(r => r.Id == review.LearnerId).FirstOrDefault();
            review.Instructor = instructor;
            review.Learner=learner;
            review.isFlagged = false;
            
            


            if (ModelState.IsValid)
            {
               
                _context.Add(review);
               
                await _context.SaveChangesAsync();
                instructor.TotalRating = GetTotalInstructorRating(instructor.Id, review.Rating);
                _context.Update(instructor);
                await _context.SaveChangesAsync();

                return RedirectToAction("SeeAllInstructors", "Home");
            }
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Id", review.InstructorId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Id", review.InstructorId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,Rating,ReviewMessage,DateCreated,InstructorId,LearnerId")] Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Id", review.InstructorId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Instructor)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private decimal GetTotalInstructorRating(string id, decimal rating) // Gets total rating of Instructor
        {
            var reviews =  _context.Reviews.Include(r => r.Instructor).Where(m => m.InstructorId == id).ToList(); //Gets all the reviews of instructor and adds to a list.

            decimal totalRating =0;

            foreach (var review in reviews) //Loops through each review adding rating together
            {
                totalRating += review.Rating;
            }
           

            if (totalRating == 0) { totalRating = rating; }
            else
            {
                totalRating=totalRating/(decimal)reviews.Count; //Gets the average rating
            }

            
            return totalRating;
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
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
