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
        private readonly ChatService _chatService;



        public ChatHub(DrivePalDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessage(string recipientId, string message)
        {
            var currentUser = await _userManager.GetUserAsync(Context.User);
            var currentUserName = $"{currentUser.FirstName} {currentUser.LastName}";

            var newMessage = new ChatMessage
            {
                SenderId = currentUser.Id,
                RecipientId = recipientId,
                Content = message,
                SentAt = DateTime.Now
            };

            var groupName = GetGroupName(currentUser.Id, recipientId);

            // Check if the group exists
            var group = _context.ChatGroups.FirstOrDefault(g => g.Name == groupName);
            if (group == null)
            {
                // Create the group if it doesn't exist
                group = new ChatGroup { Name = groupName };
                _context.ChatGroups.Add(group);
            }

            // Associate the message with the group
            newMessage.Group = group;

            _context.ChatMessages.Add(newMessage);
            await _context.SaveChangesAsync();

            await Clients.Group(groupName).SendAsync("ReceiveMessage", currentUser.Id, currentUserName, message, DateTime.Now);
        }



        //public async Task SendMessage(string recipientId, string message)
        //{
        //    var currentUser = await _userManager.GetUserAsync(Context.User);

        //    var newMessage = new ChatMessage
        //    {
        //        SenderId = currentUser.Id,
        //        RecipientId = recipientId,
        //        Content = message,
        //        SentAt = DateTime.Now
        //    };

        //    _context.ChatMessages.Add(newMessage);
        //    await _context.SaveChangesAsync();

        //    await Clients.User(recipientId).SendAsync("ReceiveMessage", currentUser.Id, $"{currentUser.FirstName} {currentUser.LastName}", message, DateTime.Now);
        //    //await Clients.User(currentUser.Id).SendAsync("ReceiveMessage", currentUser.FirstName, message);
        //}
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var recipientId = httpContext.Request.Query["recipientId"];
            var currentUserId = _userManager.GetUserId(httpContext.User);

            var groupName = GetGroupName(currentUserId, recipientId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await base.OnConnectedAsync();
        }
        public string GetGroupName(string userId1, string userId2)
        {
            var stringCompare = string.Compare(userId1, userId2);
            return stringCompare < 0
                ? $"{userId1}-{userId2}"
                : $"{userId2}-{userId1}";
        }

    }

}
