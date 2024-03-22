using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DrivePal.Models.ViewModels;
using Stripe.Checkout;
using DrivePal.Models.ServiceClasses;
using Stripe;



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
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private ILogger<BookingController> _logger;

        public BookingController(ILogger<BookingController> logger, DrivePalDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, EmailService emailService,IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _contextAccessor = contextAccessor;
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

        //[HttpPost]
        //public async Task<IActionResult> MakeBooking(int DrivingClassId)
        //{
        //    var drivingClass = await _context.DrivingClasses
        //        .FirstOrDefaultAsync(m => m.DrivingClassId == DrivingClassId);

        //    if (drivingClass == null || drivingClass.IsReserved == true)
        //    {
        //        return NotFound();
        //    }

        //    var learnerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the ID of the current user (learner).

        //    var booking = new Booking
        //    {
        //        BookingDate = DateTime.Now,
        //        Price = drivingClass.Price,
        //        DrivingClassId = drivingClass.DrivingClassId,
        //        InstructorId = drivingClass.InstructorId,
        //        LearnerId = learnerId,
        //    };

        //    // Set the IsReserved property of the DrivingClass to true.
        //    drivingClass.IsReserved = true;

        //    // Additionally, set the LearnerId for the DrivingClass.
        //    drivingClass.LearnerId = learnerId; // This will link the DrivingClass to the Learner who made the booking.

        //    _context.Bookings.Add(booking);
        //    await _context.SaveChangesAsync(); // This saves both the Booking and the updated DrivingClass.


        //    // Prepare email details
        //    string subject = "Your Booking Confirmation";
        //    string message = $"Hello,<br><br>" +
        //                     $"This is a confirmation for your booking on xxxxxxxx.<br>" +
        //                     $"Class details: random detalis.<br><br>" +
        //                     $"Best,<br>Your Driving School Team";

        //    // Send the email
        //    await _emailService.SendEmailAsync(booking.Learner.Email, subject, message);

        //    return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.BookingId });
        //}
        public async Task<IActionResult> CheckOut(int Id)
        {
            int lessonId = Id;
            var lesson = await _context.DrivingClasses
                .Include(dc => dc.Instructor).Include(dc => dc.Learner)
                .FirstOrDefaultAsync(m => m.DrivingClassId == Id);

            var learnerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Assuming you're using ASP.NET Core Identity
            var learner = await _context.Users
                .OfType<Learner>()
                .FirstOrDefaultAsync(u => u.Id == learnerId);

            if (learner == null || string.IsNullOrEmpty(learner.Email))
            {
                // Handle the case where the learner or email is null or empty
                _logger.LogError("Learner or email is null for user ID: {UserId}", learnerId);
                // Redirect to an error page or return an error view
                return View("Error");
            }
         


            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = Url.Action("MakeBooking", "Booking", new { DrivingClassId = lessonId}, Request.Scheme),
                CancelUrl = Url.Action("SelectClass", "Booking", new { Id = lessonId }, Request.Scheme),
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = learner.Email

            };
          
            long priceInPence = (long)(lesson.Price * 100); // Convert GBP to pence

            var sessionListItem = new Stripe.Checkout.SessionLineItemOptions
            {
                PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                {
                    Currency = "gbp",
                    UnitAmount = priceInPence,
                    ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Driving Lesson Booking",
                        Description= "Lesson starts at: "+lesson.DrivingClassStart.ToString()+" With "+lesson.Instructor.FirstName,
                        
                        
                    },
                },
                Quantity = 1,
            };
            options.LineItems.Add(sessionListItem);
            var service = new Stripe.Checkout.SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            HttpContext.Session.SetString("StripeSessionId", session.Id);
           


            return new StatusCodeResult(303);
        }


    
        public async Task<IActionResult> MakeBooking(int DrivingClassId)
        {
            var drivingClass = await _context.DrivingClasses
                .Include(dc => dc.Instructor)
                .FirstOrDefaultAsync(m => m.DrivingClassId == DrivingClassId);

            if (drivingClass == null)
            {
                return NotFound();
            }

            var learnerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Assuming you're using ASP.NET Core Identity
            var learner = await _context.Users
                .OfType<Learner>()
                .FirstOrDefaultAsync(u => u.Id == learnerId);

            if (learner == null || string.IsNullOrEmpty(learner.Email))
            {
                // Handle the case where the learner or email is null or empty
                _logger.LogError("Learner or email is null for user ID: {UserId}", learnerId);
                // Redirect to an error page or return an error view
                return View("Error");
            }


            var booking = new Booking
            {
                BookingDate = DateTime.Now,
                Price = drivingClass.Price,
                DrivingClassId = drivingClass.DrivingClassId,
                InstructorId = drivingClass.InstructorId,
                LearnerId = learnerId,
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            var stripeSessionId = HttpContext.Session.GetString("StripeSessionId");
            var payment = new Payment
            {
                StripeId = stripeSessionId,
                Amount = booking.Price,
                BookingId = booking.BookingId,
                PaymentDate = DateTime.Now,
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();


            // Prepare the email content
            string subject = "Your Booking Confirmation";
            string message = $"Hello {learner.FirstName},\n\n" +
                             $"This is a confirmation for your booking on {booking.BookingDate}.\n\n" +
                             $"Your driving lesson will start at {drivingClass.DrivingClassEnd} and will end at {drivingClass.DrivingClassEnd}.\n\n" +
                             $"Your instructor will be {drivingClass.Instructor.FirstName}\n\n" +
                             $"Best regards,\nYour DrivePal Team";

            // Send the email
            try
            {
                await _emailService.SendEmailAsync(learner.Email, subject, message);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Failed to send email to learner ID: {UserId}", learnerId);
                // Handle email sending failure, maybe queue it for a retry
            }

            string toPhoneNumber = learner.PhoneNumber;

            string fromPhoneNumber = "+447488899615";

            //Instantiate the SmsService
            SmsService smsService = new SmsService();

            //Sends the SMS
            await smsService.SendSmsAsync(toPhoneNumber, fromPhoneNumber, message);




            // Redirect to the BookingConfirmation action
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
