using Microsoft.AspNetCore.Mvc;
using DrivePal.Data;
using DrivePal.Models;
using Google;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;


namespace DrivePal.Controllers
{
    public class AccountDataController : Controller
    {
        // Database context for interacting with the application database
        private DrivePalDbContext _context;
        // User manager for managing user-related operations
        private UserManager<User> _userManager;
        // Sign-in manager for managing user authentication
        private SignInManager<User> _signInManager;
        // Role manager for managing user roles
        private RoleManager<IdentityRole> _roleManager;

        private ILogger<AccountDataController> _logger;

        public AccountDataController(ILogger<AccountDataController> logger, DrivePalDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
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

    }
}
