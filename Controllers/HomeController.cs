using DrivePal.Models;
using Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DrivePal.Data;
using DrivePal.Models.ViewModels;
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



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
