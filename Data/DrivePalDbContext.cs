using DrivePal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DrivePal.Data
{
    public class DrivePalDbContext : IdentityDbContext
    {
        public DrivePalDbContext(DbContextOptions<DrivePalDbContext> options)
            : base(options)
        {
        }
        public DbSet<Review>Reviews { get; set; }
    }
}
