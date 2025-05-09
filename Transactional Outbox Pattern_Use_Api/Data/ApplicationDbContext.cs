using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Transactional_Outbox_Pattern_Use_Api.Data.Entities;

namespace Transactional_Outbox_Pattern_Use_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(c => c.Id);
            modelBuilder.Entity<OutboxMessage>().HasKey(c => c.Id);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
    }
}
