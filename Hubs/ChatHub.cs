using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace DrivePal.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;

        public ChatHub(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task SendMessage(string message)
        {
            var user = await _userManager.GetUserAsync(Context.User);
            // Save message to database or perform other actions as needed
            await Clients.All.SendAsync("ReceiveMessage", user.UserName, message);
        }
    }

}
