using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrivePal.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DrivePalDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ChatHub> _logger;



        public ChatHub(DrivePalDbContext context, UserManager<User> userManager, ILogger<ChatHub> logger, ChatService _chatService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }


        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var chatGroup = await _context.ChatGroups
                .Include(g => g.Messages)
                .FirstOrDefaultAsync(g => g.Name == groupName);

            if (chatGroup != null)
            {
                await Clients.Caller.SendAsync("ReceiveChatHistory", chatGroup.Messages);
            }
        }


        //public async Task JoinGroup(string groupName)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //}

        public async Task SendMessage(string recipientId, string message)
        {
            try 
            { 

                var currentUser = await _userManager.GetUserAsync(Context.User);
                var currentUserName = $"{currentUser.FirstName} {currentUser.LastName}";

                var newMessage = new ChatMessage
                {
                    SenderId = currentUser.Id,
                    SenderName = currentUserName,
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

                // Check if a UserChatGroup record for the sender already exists
                var senderUserChatGroup = _context.UserChatGroups
                    .FirstOrDefault(ucg => ucg.UserId == currentUser.Id && ucg.ChatGroupId == group.Id);
                if (senderUserChatGroup == null)
                {
                    // Add a new UserChatGroup record for the sender if it doesn't exist
                    senderUserChatGroup = new UserChatGroup
                    {
                        UserId = currentUser.Id,
                        ChatGroup = group
                    };
                    _context.UserChatGroups.Add(senderUserChatGroup);
                }

                // Check if a UserChatGroup record for the recipient already exists
                var recipientUserChatGroup = _context.UserChatGroups
                    .FirstOrDefault(ucg => ucg.UserId == recipientId && ucg.ChatGroupId == group.Id);
                if (recipientUserChatGroup == null)
                {
                    // Add a new UserChatGroup record for the recipient if it doesn't exist
                    recipientUserChatGroup = new UserChatGroup
                    {
                        UserId = recipientId,
                        ChatGroup = group
                    };
                    _context.UserChatGroups.Add(recipientUserChatGroup);
                }
                _context.ChatMessages.Add(newMessage);

                await _context.SaveChangesAsync();

                await Clients.Group(groupName).SendAsync("ReceiveMessage", currentUser.Id, currentUserName, message, DateTime.Now);
                await UpdateChatGroups(currentUser.Id);
                await UpdateChatGroups(recipientId);

                await Clients.User(currentUser.Id).SendAsync("UpdateChatGroupList");
                await Clients.User(recipientId).SendAsync("UpdateChatGroupList");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendMessage method.");
                throw;
            }
        }

        public async Task UpdateChatGroups(string userId)
        {
            try
            {
                var userGroups = _context.UserChatGroups
                    .Where(ucg => ucg.UserId == userId)
                    .Include(ucg => ucg.ChatGroup)
                        .ThenInclude(g => g.Messages)
                    .Select(ucg => new
                    {
                        Group = ucg.ChatGroup,
                        OtherUserChatGroup = ucg.ChatGroup.UserChatGroups.FirstOrDefault(ucg => ucg.UserId != userId)
                    })
                    .ToList();

                var userGroupsWithOtherParticipant = userGroups.Select(ug => new
                {
                    Group = ug.Group,
                    OtherParticipant = ug.OtherUserChatGroup != null ? ug.OtherUserChatGroup.User?.UserName : null
                }).ToList();


                await Clients.User(userId).SendAsync("UpdateChatGroups", userGroupsWithOtherParticipant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateChatGroups method.");
                throw;
            }
        }



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
