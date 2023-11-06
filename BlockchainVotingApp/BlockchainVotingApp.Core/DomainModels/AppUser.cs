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
            CountyId = user.CountyId;
            if (user.DateOfBirth.HasValue)
                DateOfBirth = user.DateOfBirth.Value;
            if(user.County != null)
                CountyName = user.County.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Username { get; set; } = null!;
        public int CountyId { get; set; }
        public string CountyName { get; set;}
    }
}
