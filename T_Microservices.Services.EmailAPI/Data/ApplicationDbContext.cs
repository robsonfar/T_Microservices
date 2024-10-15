using Microsoft.EntityFrameworkCore;

using T_Microservices.Services.EmailAPI.Models;

namespace T_Microservices.Services.EmailAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<EmailLogger> EmailLogger { get; set; }

    }
}
