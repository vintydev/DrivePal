using DrivePal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DrivePal.Data
{
    public class DrivePalDbContext : IdentityDbContext<User>
    {
        public DrivePalDbContext(DbContextOptions<DrivePalDbContext> options)
            : base(options)
        {
        }

        public DbSet<Review>Reviews { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Learner> Learners { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Payment> Payments { get; set; }

    }
}
