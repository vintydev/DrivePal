using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        public async Task<IActionResult> OwnDrivingClasses()
        {
            // Assuming GetUserId() retrieves the current logged-in instructor's ID.
            string currentInstructorId = GetUserId();

            // Now, filter the driving classes to include only those that match the instructor ID.
            var drivingClasses = _context.DrivingClasses
                                        .Include(i => i.Instructor)
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
        public async Task<IActionResult> Create([Bind("DrivingClassStart,DrivingClassEnd,Price,InstructorId,LearnerId")] DrivingClass drivingClass)
        {
            if (ModelState.IsValid)
            {
                // Set the InstructorId if not set from the form, as the user is authenticated and authorized as an Instructor
                if (string.IsNullOrEmpty(drivingClass.InstructorId))
                {
                    drivingClass.InstructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                }

                _context.Add(drivingClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(drivingClass);
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








    }
}
