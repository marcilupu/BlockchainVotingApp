using BlockchainVotingApp.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlockchainVotingApp.Data.Ef.Context
{
    internal class VDbContext : IdentityDbContext<DbUser>
    {
        public VDbContext() : base() { }
        public VDbContext(DbContextOptions<VDbContext> options) : base(options) { }

        public DbSet<DbElection> Elections { get; set; } = null!;
        public DbSet<DbCandidate> Candidates { get; set; } = null!;
        public DbSet<DbCounty> Counties { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            VDbInitializer.Seed(builder);
        }
    }
}
