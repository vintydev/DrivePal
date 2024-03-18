using DrivePal.Data;
using DrivePal.Hubs;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DrivePal.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly DrivePalDbContext _context;
        private readonly ChatService _chatService;

        public ChatController(UserManager<User> userManager, DrivePalDbContext context, ChatService chatService)
        {
            _userManager = userManager;
            _context = context;
            _chatService = chatService;
        }

        public async Task<IActionResult> Index(string recipientId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserName = $"{currentUser.FirstName} {currentUser.LastName}";
            var recipient = await _userManager.FindByIdAsync(recipientId);
            var recipientName = $"{recipient.FirstName} {recipient.LastName}";

            if (currentUser == null || recipient == null)
            {
                return NotFound();
            }

            var groupName = _chatService.GetGroupName(currentUser.Id, recipient.Id);

            var chatMessages = _context.ChatMessages
                .Where(m => m.Group.Name == groupName)
                .OrderBy(m => m.SentAt)
                .ToList();

            // Retrieve sender names
            var senderIds = chatMessages.Select(m => m.SenderId).Distinct();
            var senderNames = new Dictionary<string, string>(); // Store sender names
            foreach (var senderId in senderIds)
            {
                var sender = await _userManager.FindByIdAsync(senderId);
                if (sender != null)
                {
                    senderNames[senderId] = $"{sender.FirstName} {sender.LastName}";
                }
            }

            // Replace senderId with sender name in chatMessages
            foreach (var message in chatMessages)
            {
                message.SenderId = senderNames[message.SenderId];
            }

            // Create a new ChatViewModel
            var model = new ChatViewModel
            {
                CurrentUserName = currentUserName,
                OtherUserName = recipientName, // Replace with the actual other user's name
                RecipientId = recipient.Id, // Replace with the actual other user's ID
                GroupName = groupName,
                Messages = chatMessages,
                SenderNames = senderNames
            };

            return View(model);
        }


        //public async Task<IActionResult> Index(string recipientId)
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var recipient = await _userManager.FindByIdAsync(recipientId);

        //    if (currentUser == null || recipient == null)
        //    {
        //        return NotFound();
        //    }

        //    var groupName = _chatHub.GetGroupName(currentUser.Id, recipient.Id);

        //    var chatMessages = _context.ChatMessages
        //        .Where(m => m.Group.Name == groupName)
        //        .OrderBy(m => m.SentAt)
        //        .ToList();

        //    if (currentUser == null || recipient == null)
        //    {
        //        // Handle error (e.g., user not found)
        //        return NotFound();
        //    }


        //    var chatMessages = _context.ChatMessages
        //        .Where(m => (m.SenderId == currentUser.Id && m.RecipientId == recipient.Id) ||
        //                    (m.SenderId == recipient.Id && m.RecipientId == currentUser.Id))
        //        .OrderBy(m => m.SentAt)
        //        .ToList();

        //    // Retrieve sender names
        //    var senderIds = chatMessages.Select(m => m.SenderId).Distinct();
        //    var senderNames = new Dictionary<string, string>(); // Store sender names
        //    foreach (var senderId in senderIds)
        //    {
        //        var sender = await _userManager.FindByIdAsync(senderId);
        //        if (sender != null)
        //        {
        //            senderNames[senderId] = $"{sender.FirstName} {sender.LastName}";
        //        }
        //    }

        //    // Replace senderId with sender name in chatMessages
        //    foreach (var message in chatMessages)
        //    {
        //        message.SenderId = senderNames[message.SenderId];
        //    }

        //    // Create a new ChatViewModel
        //    var model = new ChatViewModel
        //    {
        //        OtherUserName = recipient.UserName, // Replace with the actual other user's name
        //        RecipientId = recipient.Id, // Replace with the actual other user's ID
        //        GroupName = groupName,
        //        Messages = chatMessages
        //    };

        //    return View(model);
        //}



    }
}