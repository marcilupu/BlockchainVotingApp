using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Models.Candidate;

namespace BlockchainVotingApp.Models.Election.ViewModels
{
    public class ElectionItemViewModel
    {
        public ElectionItemViewModel(DbElection election)
        {
            Candidates = election.Candidates!.Select(item =>
            {
                return new CandidateItemViewModel()
                {
                    FullName = $"{item.FirstName} {item.LastName}",
                    Organization = item.Organization,
                    Biography = item.Biography,
                    ElectionId = item.ElectionId,
                    Id = item.Id
                };
            }).ToList();

            Name = election.Name;
            Id = election.Id;
            State = election.State;
            County = election.County?.Name;
            ContractAddress = election.ContractAddress;
            StartDate = election.StartDate;
            EndDate = election.EndDate;
            Rules = election.Rules;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContractAddress { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Rules { get; set; }
        public string? County { get; set; }
        public ElectionState State { get; set; }
        public bool HasVoted { get; set; }
        public List<CandidateItemViewModel> Candidates { get; set; }
    }
}
