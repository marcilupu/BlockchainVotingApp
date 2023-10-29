using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels
{
    public class CandidatesItemViewModel
    {
        public CandidatesItemViewModel(DbCandidate item)
        {
            FullName = $"{item.FirstName} {item.LastName}";
            Organization = item.Organization;
            Biography = item.Biography;
        }

        public string FullName { get; set; } = null!;
        public string Organization { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public string Election { get; set; } = null!;
        public int ElectionId { get; set; }
        public int Id { get; set; }
    }
}
