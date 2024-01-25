using BlockchainVotingApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlockchainVotingApp.Data.Ef.Context
{
    internal class VDbInitializer
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbCounty>().HasData(
                new DbCounty() { Id = 1, Name = "Alba" },
                new DbCounty() { Id = 2, Name = "Arad" },
                new DbCounty() { Id = 3, Name = "Arges" },
                new DbCounty() { Id = 4, Name = "Bihor" },
                new DbCounty() { Id = 5, Name = "Brasov" },
                new DbCounty() { Id = 6, Name = "Braila" },
                new DbCounty() { Id = 7, Name = "Bucuresti" },
                new DbCounty() { Id = 8, Name = "Cluj" },
                new DbCounty() { Id = 9, Name = "Constanta" },
                new DbCounty() { Id = 10, Name = "Covasna" },
                new DbCounty() { Id = 11, Name = "Dolj" },
                new DbCounty() { Id = 12, Name = "Galati" },
                new DbCounty() { Id = 13, Name = "Gorj" },
                new DbCounty() { Id = 14, Name = "Giurgiu" },
                new DbCounty() { Id = 15, Name = "Hunedoara" },
                new DbCounty() { Id = 16, Name = "Ialomita" },
                new DbCounty() { Id = 17, Name = "Iasi" }
            );

            modelBuilder.Entity<DbUserRole>().HasData(
                new DbUserRole { Id = 1, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "1" },
                new DbUserRole { Id = 2, Name = "Voter", NormalizedName = "VOTER", ConcurrencyStamp = "2" }
            );
        }
    }
}
