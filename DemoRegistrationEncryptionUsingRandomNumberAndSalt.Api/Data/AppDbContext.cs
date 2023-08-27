using Microsoft.EntityFrameworkCore;
using SharedLogicLibrary.Models.Entities;

namespace DemoRegistrationEncryptionUsingRandomNumberAndSalt.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<RegistrationEntity> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

    }
}
