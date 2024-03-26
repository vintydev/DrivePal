using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Plugins;
using Message = DrivePal.Models.Message;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly DrivePalDbContext _context;

        public ChatHub(UserManager<User> userManager, DrivePalDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SendMessage(string user, string receiverId, string message)
        {
            
            var senderId = Context.UserIdentifier;

            var msg = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Text = message,
                Timestamp = DateTime.UtcNow,
                Read = false
            };
            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            var formattedTime = msg.Timestamp.ToString("HH:mm");

            await Clients.User(receiverId).SendAsync("ReceiveMessage", user, message, formattedTime, msg.Read, msg.Id);
            await Clients.Caller.SendAsync("ReceiveMessage", user, message, formattedTime, msg.Read, msg.Id);
        }

        public async Task ReadMessage(int messageId)
        {
            var message = _context.Messages.Find(messageId);
            if (message != null && message.ReceiverId == Context.UserIdentifier)
            {
                message.Read = true;
                _context.Messages.Update(message);
                await _context.SaveChangesAsync();

                await MarkAsRead(messageId);
            }
        }

        public async Task MarkAsRead(int messageId)
        {
            var message = _context.Messages.Find(messageId);
            if (message != null && message.Read)
            {
                await Clients.User(message.SenderId).SendAsync("MessageRead", messageId);
            }
        }



    }
}
