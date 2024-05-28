// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace RealEstateApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
    }
}

