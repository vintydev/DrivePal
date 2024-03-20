using DrivePal.Data;
using DrivePal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DrivePal.Models.ViewModels;
using Stripe.Checkout;
using Google.Api;
using Microsoft.AspNetCore.Authorization;

namespace DrivePal.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly DrivePalDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckOutController(DrivePalDbContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult CheckOut()
        {
            return View();
        }

        [Authorize]
        //public async Task<Basket> GetUserBasket()
        //{
        //    // Get the userId from the method
        //    var userId = GetUserId();

        //    // Check if the userId is valid
        //    if (userId == null)
        //    {
        //        throw new Exception("Invalid UserId");
        //    }

        //    // Retrieve the basket for the specified userId, including related entities
        //    var basket = await context.Baskets.Include(a => a.BasketDetails)
        //        .ThenInclude(a => a.Product)
        //        .ThenInclude(a => a.Category)
        //        .Where(a => a.UserId == userId)
        //        .FirstOrDefaultAsync();

        //    // If the basket doesn't exist, create a new one
        //    if (basket is null)
        //    {
        //        BasketDetail basketDetail = new BasketDetail();
        //        basket = new Basket
        //        {
        //            UserId = userId,
        //        };
        //        _context.Baskets.Add(basket);
        //    }

        //    _context.SaveChanges();
        //}
        private string GetUserId()
        {
            // Access the current user's information from the HttpContext
            var principal = _httpContextAccessor.HttpContext.User;

            // Use the UserManager to get the user ID from the principal
            string userId = _userManager.GetUserId(principal);

            // Return the user ID
            return userId;
        }
    }
}
