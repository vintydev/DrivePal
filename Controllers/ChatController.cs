using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

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

        public async Task<IActionResult> IndexAsync(string id, string text) // Add 'text' parameter
        {
            var user = await _userManager.GetUserAsync(User);
            var fullName = user.FirstName + " " + user.LastName;
            var receiver = await _userManager.FindByIdAsync(id);
            var receiverName = receiver.FirstName + " " + receiver.LastName;

            ViewBag.ReceiverName = receiverName;

            var messages = _context.Messages
        .Where(m => (m.SenderId == user.Id && m.ReceiverId == id) || (m.SenderId == id && m.ReceiverId == user.Id))
        .Select(m => new Message
        {
            Id = m.Id,
            SenderId = m.SenderId,
            SenderName = _userManager.Users.Where(u => u.Id == m.SenderId).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault(),
            ReceiverId = m.ReceiverId,
            Text = m.Text,
            Timestamp = m.Timestamp,
            Read = m.Read
        })
        .OrderBy(m => m.Timestamp)
        .ToList();

            if (messages == null)
            {
                // Create a new message
                var newMessage = new Message
                {
                    SenderId = user.Id,
                    ReceiverId = id,
                    Text = text, // Set the 'Text' property to the input text
                    Timestamp = DateTime.Now
                };

                _context.Messages.Add(newMessage);
                await _context.SaveChangesAsync();

                messages = new List<Message> { newMessage };
            }

            // Mark all messages from the other user as read
            foreach (var message in messages.Where(m => m.ReceiverId == user.Id && !m.Read))
            {
                message.Read = true;
            }
            await _context.SaveChangesAsync();

            ViewBag.FullName = fullName;
            ViewBag.ReceiverId = id;
            ViewBag.Messages = messages;
            ViewBag.UserId = user.Id;

            return View();
        }

        public async Task<IActionResult> Chats()
        {
            var user = await _userManager.GetUserAsync(User);

            var chatUserIds = _context.Messages
                .Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id)
                .Select(m => m.SenderId == user.Id ? m.ReceiverId : m.SenderId)
                .Distinct()
                .ToList();

            var chats = new List<Chat>();

            foreach (var chatUserId in chatUserIds)
            {
                var messages = _context.Messages
                    .Where(m => (m.SenderId == user.Id && m.ReceiverId == chatUserId) || (m.SenderId == chatUserId && m.ReceiverId == user.Id))
                    .OrderByDescending(m => m.Timestamp)
                    .ToList();

                var chat = new Chat
                {
                    UserId = chatUserId,
                    UserName = _userManager.Users.FirstOrDefault(u => u.Id == chatUserId)?.UserName ?? string.Empty,
                    FirstName = _userManager.Users.FirstOrDefault(u => u.Id == chatUserId)?.FirstName ?? string.Empty,
                    LastName = _userManager.Users.FirstOrDefault(u => u.Id == chatUserId)?.LastName ?? string.Empty,
                    LastMessage = messages.FirstOrDefault()?.Text,
                    UnreadCount = messages.Count(m => !m.Read && m.ReceiverId == user.Id)
                };

                chats.Add(chat);
            }

            return View(chats);
        }

    }
}
