using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DrivePal.Helpers;

namespace DrivePal.Controllers
{
    public class DrivingClassController : Controller
    {
        // Database context for interacting with the application database
        private DrivePalDbContext _context;
        // User manager for managing user-related operations
        private UserManager<User> _userManager;
        // Sign-in manager for managing user authentication
        private SignInManager<User> _signInManager;
        // Role manager for managing user roles
        private RoleManager<IdentityRole> _roleManager;

        private ILogger<DrivingClassController> _logger;

        public DrivingClassController(ILogger<DrivingClassController> logger, DrivePalDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        [Authorize]
        public IActionResult Calendar()
        {
            var user = GetUserId();
            var drivingClasses = _context.DrivingClasses
                                        .Include(i => i.Instructor)
                                        .Include(d => d.Learner) // Include the Learner navigation property
                                        .Where(d => d.Instructor.Id == user ).ToList();

          
            
            ViewData["Lessons"] = JSONListHelper.GetDrivingClassListJSONString(drivingClasses);
            return View();
        
        }

        public IActionResult Availability(string id) // shows an instructors available lessons
        {
            
            var drivingClasses = _context.DrivingClasses
                                        .Include(i => i.Instructor)
                                        .Include(d => d.Learner) 
                                        .Where(d => d.Instructor.Id == id && d.IsReserved == false).ToList();



            ViewData["Lessons"] = JSONListHelper.GetDrivingClassListJSONString(drivingClasses);
            return View();

        }



        public async Task<IActionResult> OwnDrivingClasses()
        {
            // Assuming GetUserId() retrieves the current logged-in instructor's ID.
            string currentInstructorId = GetUserId();

            // Now, filter the driving classes to include only those that match the instructor ID.
            var drivingClasses = _context.DrivingClasses
                                        .Include(i => i.Instructor)
                                        .Include(d => d.Learner) // Include the Learner navigation property
                                        .Where(d => d.Instructor.Id == currentInstructorId);

            return View(await drivingClasses.ToListAsync());
        }

        public async Task<IActionResult> Index()
        {
            var drivingClasses = _context.DrivingClasses.Include(i => i.Instructor);
            return View(await drivingClasses.ToListAsync());

        }
        // GET: Reviews/Create
        [Authorize(Roles = "Instructor")]
        public IActionResult Create()
        {
            // Fetch the user ID of the currently logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Create and return the view model if needed, with the InstructorId set
            var viewModel = new DrivingClass { InstructorId = userId };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DrivingClassStart,DrivingClassEnd,Price,InstructorId")] DrivingClass drivingClass)
        {
            if (ModelState.IsValid)
            {
                // Set the InstructorId if not set from the form
                if (string.IsNullOrEmpty(drivingClass.InstructorId))
                {
                    drivingClass.InstructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                }

                // Set IsReserved to false when creating a new DrivingClass
                drivingClass.IsReserved = false;

                _context.Add(drivingClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(OwnDrivingClasses));
            }
            return View(drivingClass);
        }
        public IActionResult GenerateLessons()
        {
            return View();
        }
        [HttpPost]

        [HttpPost]
        public async Task<IActionResult> GenerateLessons(string[] workingDays, string startTime, string endTime, decimal price)
        {
            var instructorId=GetUserId();
            var startTimeSpan = TimeSpan.Parse(startTime);
            var endTimeSpan = TimeSpan.Parse(endTime);

            List<DrivingClass> lessons = new List<DrivingClass>();

            foreach (var day in workingDays)
            {
                var currentDate = DateTime.Today;
                // Find the next date that matches the day of the week
                while (currentDate.DayOfWeek.ToString() != day)
                {
                    currentDate = currentDate.AddDays(1);
                }

                var lessonStartTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, startTimeSpan.Hours, startTimeSpan.Minutes, 0);
                var lessonEndTime = lessonStartTime.AddHours(1);

                while (lessonEndTime.TimeOfDay <= endTimeSpan)
                {
                    lessons.Add(new DrivingClass
                    {
                        DrivingClassStart = lessonStartTime,
                        DrivingClassEnd = lessonEndTime,
                        Price = price,
                        IsReserved = false,
                        InstructorId = instructorId
                        
                    });

                    // Move to the next slot
                    lessonStartTime = lessonEndTime;
                    lessonEndTime = lessonStartTime.AddHours(1);
                }
            }

            // Process lessons list, e.g., save to database
            _context.AddRange(lessons);
             await _context.SaveChangesAsync();

            // Redirect or return view as appropriate
            return RedirectToAction("Calendar"); // or return View() with a confirmation message
        }



        // GET: DrivingClasses/Delete/5
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drivingClass = await _context.DrivingClasses
                .FirstOrDefaultAsync(m => m.DrivingClassId == id);
            if (drivingClass == null)
            {
                return NotFound();
            }

            return View(drivingClass);
        }

        // POST: DrivingClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drivingClass = await _context.DrivingClasses.FindAsync(id);
            if (drivingClass != null)
            {
                _context.DrivingClasses.Remove(drivingClass);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> ShowClassesForThisInstructor(string instructorId)
        {
            if (string.IsNullOrEmpty(instructorId))
            {
                return NotFound();
            }

            var drivingClasses = await _context.DrivingClasses
                                               .Where(d => d.InstructorId == instructorId && d.IsReserved == false)
                                               .Include(d => d.Instructor) // To access instructor's details if needed in the view
                                               .ToListAsync();

            return View(drivingClasses);
        }








    }
}
