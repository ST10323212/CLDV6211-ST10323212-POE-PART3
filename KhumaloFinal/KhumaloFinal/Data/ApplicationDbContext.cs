using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KhumaloFinal.Models;

namespace KhumaloFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<KhumaloFinal.Models.OrderHistory> OrderHistory { get; set; } = default!;
        public DbSet<KhumaloFinal.Models.Processor> Processor { get; set; } = default!;
        public DbSet<KhumaloFinal.Models.Product> Product { get; set; } = default!;
        public DbSet<KhumaloFinal.Models.Transaction> Transaction { get; set; } = default!;
    }
}
