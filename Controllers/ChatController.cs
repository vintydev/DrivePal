using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DrivePal.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly DrivePalDbContext _context;

        public ChatController(UserManager<User> userManager, DrivePalDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByEmailAsync("learner@example.com"); // Replace with actual email logic
            var instructor = await _userManager.FindByEmailAsync("instructor@example.com"); // Replace with actual email logic

            if (currentUser == null || instructor == null)
            {
                // Handle error (e.g., user not found)
                return View(new List<ChatMessage>()); // Return an empty list
            }

            var chatMessages = _context.ChatMessages
                .Where(m => (m.SenderId == currentUser.Id && m.RecipientId == instructor.Id) ||
                            (m.SenderId == instructor.Id && m.RecipientId == currentUser.Id))
                .OrderBy(m => m.SentAt)
                .ToList();

            // Retrieve sender names
            var senderIds = chatMessages.Select(m => m.SenderId).Distinct();
            var senderNames = new Dictionary<string, string>(); // Store sender names
            foreach (var senderId in senderIds)
            {
                var sender = await _userManager.FindByIdAsync(senderId);
                senderNames[senderId] = $"{sender.FirstName} {sender.LastName}";
            }

            // Replace senderId with sender name in chatMessages
            foreach (var message in chatMessages)
            {
                message.SenderId = senderNames[message.SenderId];
            }

            return View(chatMessages);
        }
    }
}
