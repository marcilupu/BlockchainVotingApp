using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace BlockchainVotingApp.Data.Models
{
    public class DbUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Gender { get; set; }
        public string NationaId { get; set; } = null!;

        //AccountAddress - AspNetUserClaim

        //Roles - ADMIN/VOTER
    }
}
