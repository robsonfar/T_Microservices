using Microsoft.EntityFrameworkCore;

using T_Microservices.Services.ShoppingCartAPI.Models;

namespace T_Microservices.Services.ShoppingCartAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CartHeader> CartHeader { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }

    }
}
