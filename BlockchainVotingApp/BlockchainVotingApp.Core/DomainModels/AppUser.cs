using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Core.DomainModels
{
    public class AppUser
    {
        public AppUser(DbUser user)
        {
            Id = user.Id;
            Name = $"{user.FirstName} {user.LastName}";
            Email = user.Email;
            Username = user.UserName;
            Gender = user.Gender;
            NationalId = user.NationaId;
   
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public bool HasVoted { get; set; }
    }
}
