using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Core.DomainModels
{
    public class Candidate
    {
        public Candidate() { }

        public Candidate(DbCandidate item)
        {
            FullName = $"{item.FirstName} {item.LastName}";
            Organization = item.Organization;
            Biography = item.Biography;
            ElectionId = item.ElectionId;
            Id = item.Id;
        }

        public string FullName { get; set; } = null!;
        public string Organization { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public string Election { get; set; } = null!;
        public int ElectionId { get; set; }
        public int Id { get; set; }
    }
}
