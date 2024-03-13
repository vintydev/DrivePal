using DrivePal.Data;
using System;

namespace DrivePal.Models
{
    public class ChatService
    {
        private readonly DrivePalDbContext _context;

        public ChatService(DrivePalDbContext context)
        {
            _context = context;
        }

        public List<string> GetPreviousChats(string userId)
        {
            var previousChats = _context.ChatMessages
                .Where(m => m.SenderId == userId || m.RecipientId == userId)
                .Select(m => m.SenderId == userId ? m.RecipientId : m.SenderId)
                .Distinct()
                .ToList();

            return previousChats;
        }
    }
}
