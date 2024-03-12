using DrivePal.Data;
using Microsoft.AspNetCore.Mvc;

namespace DrivePal.Controllers
{
    public class MessagingController : Controller
    {
        private readonly DrivePalDbContext _context;

        public MessagingController(DrivePalDbContext context)
        {
            _context = context;
        }
        public IActionResult Inbox(string receiverId)
        {
            // Retrieve the receiver information from the database
            var receiver = _context.Users.Find(receiverId);

            // Pass the receiver information and receiverId to the view
            ViewData["ReceiverId"] = receiverId;
            ViewData["ReceiverName"] = receiver.FirstName + " " + receiver.LastName; // Assuming you have FirstName and LastName properties in the User model
            return View(receiver);
        }
    }
}
