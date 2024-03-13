using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace DrivePal.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DrivePalDbContext _context;
        private readonly UserManager<User> _userManager;

        public ChatHub(DrivePalDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SendMessage(string message)
        {
            var currentUser = await _userManager.GetUserAsync(Context.User);
            var previousMessage = _context.ChatMessages
                .OrderByDescending(m => m.SentAt)
                .FirstOrDefault();

            var recipientId = currentUser.Id == previousMessage?.SenderId ? previousMessage.RecipientId : previousMessage.SenderId;

            var newMessage = new ChatMessage
            {
                SenderId = currentUser.Id,
                RecipientId = recipientId,
                Content = message,
                SentAt = DateTime.Now
            };

            _context.ChatMessages.Add(newMessage);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", currentUser.FirstName, message);
        }



        //public async Task SendMessage(string message)
        //{
        //    var currentUser = await _userManager.GetUserAsync(Context.User);
        //    var previousMessage = _context.ChatMessages
        //        .OrderByDescending(m => m.SentAt)
        //        .FirstOrDefault();

        //    var newMessage = new ChatMessage
        //    {
        //        SenderId = currentUser.Id,
        //        RecipientId = previousMessage?.RecipientId, // Assuming the recipient is the same as the previous message
        //        Content = message,
        //        SentAt = DateTime.Now
        //    };

        //    _context.ChatMessages.Add(newMessage);
        //    await _context.SaveChangesAsync();

        //    await Clients.All.SendAsync("ReceiveMessage", currentUser.FirstName, message);
        //}
    }

}
