using DrivePal.Data;
using DrivePal.Hubs;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(UserManager<User> userManager, DrivePalDbContext context, ChatService chatService,IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _context = context;
            _chatService = chatService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index(string recipientId, bool isGroup = false)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserName = currentUser != null ? $"{currentUser.FirstName} {currentUser.LastName}" : "Unknown User";

            string recipientName;
            string groupName;

            if (isGroup)
            {
                if (!int.TryParse(recipientId, out var groupId))
                {
                    return BadRequest("Invalid group ID");
                }

                var group = _context.ChatGroups.Find(groupId);
                if (group == null)
                {
                    return NotFound();
                }
                recipientName = group.Name;
                groupName = group.Name;
            }
            else
            {
                var recipient = await _userManager.FindByIdAsync(recipientId);
                if (recipient == null)
                {
                    return NotFound();
                }
                recipientName = $"{recipient.FirstName} {recipient.LastName}";
                groupName = _chatService.GetGroupName(currentUser.Id, recipient.Id);
            }

            await _hubContext.Clients.All.SendAsync("JoinGroup", groupName); // Join the group when loading the chat page

            var userGroups = _context.UserChatGroups
                .Where(ucg => ucg.UserId == currentUser.Id)
                .Include(ucg => ucg.ChatGroup)
                    .ThenInclude(g => g.Messages)
                .Select(ucg => ucg.ChatGroup)
                .ToList();

            userGroups = userGroups
                .OrderByDescending(g => g.Messages.Max(m => m.SentAt))
                .ToList();

            var chatMessages = _context.ChatMessages
                .Where(m => m.Group.Name == groupName)
                .OrderBy(m => m.SentAt)
                .ToList();

            // Retrieve sender names
            var senderIds = chatMessages.Select(m => m.SenderId).Distinct();
            var senderNames = new Dictionary<string, string>(); // Store sender names

            if (!senderNames.ContainsKey(currentUser.Id))
            {
                senderNames[currentUser.Id] = currentUserName;
            }

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
                UserGroups = userGroups,
                CurrentUserName = currentUserName,
                OtherUserName = recipientName,
                RecipientId = recipientId,
                GroupName = groupName,
                Messages = chatMessages,
                SenderNames = senderNames
            };

            return View(model);
        }


        //working code

        //public async Task<IActionResult> Index(string recipientId)
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var currentUserName = currentUser != null ? $"{currentUser.FirstName} {currentUser.LastName}" : "Unknown User";
        //    var recipient = await _userManager.FindByIdAsync(recipientId);
        //    var recipientName = $"{recipient.FirstName} {recipient.LastName}";

        //    var groupName = _chatService.GetGroupName(currentUser.Id, recipient.Id);

        //    await _hubContext.Clients.All.SendAsync("JoinGroup", groupName); // Join the group when loading the chat page


        //    if (currentUser == null || recipient == null)
        //    {
        //        return NotFound();
        //    }


        //    var userGroups = _context.UserChatGroups
        //        .Where(ucg => ucg.UserId == currentUser.Id)
        //        .Include(ucg => ucg.ChatGroup)
        //            .ThenInclude(g => g.Messages)
        //        .Select(ucg => ucg.ChatGroup)
        //        .ToList();

        //    userGroups = userGroups
        //        .OrderByDescending(g => g.Messages.Max(m => m.SentAt))
        //        .ToList();


        //    var chatMessages = _context.ChatMessages
        //        .Where(m => m.Group.Name == groupName)
        //        .OrderBy(m => m.SentAt)
        //        .ToList();

        //    // Retrieve sender names
        //    var senderIds = chatMessages.Select(m => m.SenderId).Distinct();
        //    var senderNames = new Dictionary<string, string>(); // Store sender names

        //    if (!senderNames.ContainsKey(currentUser.Id))
        //    {
        //        senderNames[currentUser.Id] = currentUserName;
        //    }

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
        //        UserGroups = userGroups,
        //        CurrentUserName = currentUserName,
        //        OtherUserName = recipientName, // Replace with the actual other user's name
        //        RecipientId = recipient.Id, // Replace with the actual other user's ID
        //        GroupName = groupName,
        //        Messages = chatMessages,
        //        SenderNames = senderNames
        //    };

        //    return View(model);
        //}

        [HttpGet]
        public async Task<IActionResult> GetChatGroups()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var userGroups = _context.UserChatGroups
                .Where(ucg => ucg.UserId == currentUser.Id)
                .Include(ucg => ucg.ChatGroup)
                    .ThenInclude(g => g.Messages)
                .Select(ucg => ucg.ChatGroup)
                .ToList();

            userGroups = userGroups
                .OrderByDescending(g => g.Messages.Max(m => m.SentAt))
                .ToList();

            return Json(userGroups);
        }

        public async Task<IActionResult> ChatGroups()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userGroups = _context.UserChatGroups
                .Where(ucg => ucg.UserId == currentUser.Id)
                .Include(ucg => ucg.ChatGroup)
                .Select(ucg => ucg.ChatGroup)
                .ToList();

            return View(userGroups);
        }


    }
}