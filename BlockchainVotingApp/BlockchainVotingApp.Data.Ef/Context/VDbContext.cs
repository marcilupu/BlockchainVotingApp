using BlockchainVotingApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlockchainVotingApp.Data.Ef.Context
{
    internal class VDbContext : IdentityDbContext<DbUser, DbUserRole, int>
    {
        public VDbContext(DbContextOptions<VDbContext> options) : base(options) { }

        public DbSet<DbElection> Elections { get; set; } = null!;
        public DbSet<DbCandidate> Candidates { get; set; } = null!;
        public DbSet<DbCounty> Counties { get; set; } = null!;

        public DbSet<DbUserVote> UserVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<DbUserRole>().ToTable("Roles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<DbUser>().ToTable("Users");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

            VDbInitializer.Seed(builder);
        }
    }
}
