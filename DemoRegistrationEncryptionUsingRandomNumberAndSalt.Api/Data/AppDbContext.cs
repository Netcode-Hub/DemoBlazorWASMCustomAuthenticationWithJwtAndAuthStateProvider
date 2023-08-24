using DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<RegistrationModel>  Users { get; set; }
    }
}
