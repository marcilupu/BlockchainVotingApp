using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace BlockchainVotingApp.Data.Models
{
    public class DbUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string NationaId { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public DbCounty? County { get; set; }
        public int CountyId { get; set; } 

        //AccountAddress - AspNetUserClaim

        //Roles - ADMIN/VOTER
    }
}
