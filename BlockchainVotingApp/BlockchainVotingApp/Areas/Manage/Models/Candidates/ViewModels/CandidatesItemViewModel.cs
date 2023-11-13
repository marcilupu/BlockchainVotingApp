using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels
{
    public class CandidatesItemViewModel
    {
        public CandidatesItemViewModel(Candidate item)
        {
            FullName = item.FullName;
            Organization = item.Organization;
            Biography = item.Biography;
            Id = item.Id;
            ElectionId = item.ElectionId;
            Election = item.Election;
        }

        public string FullName { get; set; } = null!;
        public string Organization { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public string Election { get; set; } = null!;
        public int ElectionId { get; set; }
        public int Id { get; set; }
    }
}
