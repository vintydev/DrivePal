using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DrivePal.Models.ViewModels;


namespace DrivePal.Controllers
{
    public class BookingController : Controller
    {
        // Database context for interacting with the application database
        private DrivePalDbContext _context;
        // User manager for managing user-related operations
        private UserManager<User> _userManager;
        // Sign-in manager for managing user authentication
        private SignInManager<User> _signInManager;
        // Role manager for managing user roles
        private RoleManager<IdentityRole> _roleManager;

        private ILogger<BookingController> _logger;

        public BookingController(ILogger<BookingController> logger, DrivePalDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> SelectClass(int id)
        {
            var drivingClass = await _context.DrivingClasses
                .Include(dc => dc.Instructor) // Assuming you want to show instructor details
                .FirstOrDefaultAsync(m => m.DrivingClassId == id);

            if (drivingClass == null)
            {
                return NotFound();
            }

            return View(drivingClass);
        }

        [HttpPost]
        public async Task<IActionResult> MakeBooking(int DrivingClassId)
        {
            var drivingClass = await _context.DrivingClasses
                .FirstOrDefaultAsync(m => m.DrivingClassId == DrivingClassId);

            if (drivingClass == null || drivingClass.IsReserved == true)
            {
                return NotFound();
            }

            var learnerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the ID of the current user (learner).

            var booking = new Booking
            {
                BookingDate = DateTime.Now,
                Price = drivingClass.Price,
                DrivingClassId = drivingClass.DrivingClassId,
                InstructorId = drivingClass.InstructorId,
                LearnerId = learnerId,
            };

            // Set the IsReserved property of the DrivingClass to true.
            drivingClass.IsReserved = true;

            // Additionally, set the LearnerId for the DrivingClass.
            drivingClass.LearnerId = learnerId; // This will link the DrivingClass to the Learner who made the booking.

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(); // This saves both the Booking and the updated DrivingClass.

            return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.BookingId });
        }


        public async Task<IActionResult> BookingConfirmation(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Instructor) // Include the Instructor
                .Include(b => b.Learner) // Include the Learner
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            // You don't need to fetch the learner separately if you've included it above
            var viewModel = new BookingConfirmationViewModel
            {
                Booking = booking,
                Learner = booking.Learner, // Directly assign the included Learner
                Instructor = booking.Instructor // Directly assign the included Instructor
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ShowOwnBookings()
        {
            var learnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookings = await _context.Bookings
                .Where(b => b.LearnerId == learnerId)
                .Include(b => b.DrivingClass) // Include DrivingClass to access its properties
                .Include(b => b.Instructor) // Include Instructor to access its properties
                .Select(b => new BookingDetail
                {
                    BookingId = b.BookingId,
                    BookingDate = b.BookingDate,
                    Price = b.Price,
                    DrivingClassStart = b.DrivingClass.DrivingClassStart,
                    DrivingClassEnd = b.DrivingClass.DrivingClassEnd,
                    InstructorLastName = b.Instructor.LastName // Projecting the instructor's last name
                })
                .ToListAsync();

            var viewModel = new LearnerBookingsViewModel
            {
                Bookings = bookings
            };

            return View(viewModel);
        }


        public async Task<IActionResult> ShowBookedClasses()
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookedClasses = await _context.Bookings
                .Where(b => b.InstructorId == instructorId)
                .Include(b => b.DrivingClass) // Include DrivingClass to access its properties
                .Include(b => b.Learner) // Include Learner to access its properties
                .Select(b => new BookingDetail
                {
                    BookingId = b.BookingId,
                    BookingDate = b.BookingDate,
                    Price = b.Price,
                    DrivingClassStart = b.DrivingClass.DrivingClassStart,
                    DrivingClassEnd = b.DrivingClass.DrivingClassEnd,
                    LearnerLastName = b.Learner.LastName // Assuming Learner has FirstName and LastName
                })
                .ToListAsync();

            var viewModel = new LearnerBookingsViewModel // Assuming you create this ViewModel
            {
                Bookings = bookedClasses
            };

            return View(viewModel);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
