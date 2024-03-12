using Google.Api;
using Google;
using Microsoft.AspNetCore.SignalR;
using DrivePal.Data;
using DrivePal.Models;


namespace DrivePal.Hubs
{
    public class MessagesHub : Hub
    {
        private readonly DrivePalDbContext _context;

        public MessagesHub(DrivePalDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string receiverId, string content)
        {
            var sender = _context.Users.Find(Context.User.Identity.Name);
            var receiver = _context.Users.Find(receiverId);

            if (sender != null && receiver != null)
            {
                var message = new Message
                {
                    Content = content,
                    SentAt = DateTime.UtcNow,
                    SenderId = sender.Id,
                    Sender = sender,
                    ReceiverId = receiver.Id,
                    Receiver = receiver
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                await Clients.User(receiverId).SendAsync("ReceiveMessage", sender.UserName, content);
                await Clients.Caller.SendAsync("ReceiveMessage", sender.UserName, content);
            }
        }
    }
}
