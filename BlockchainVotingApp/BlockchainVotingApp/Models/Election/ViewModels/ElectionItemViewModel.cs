using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Models.Candidate;

namespace BlockchainVotingApp.Models.Election.ViewModels
{
    public class ElectionItemViewModel
    {
        public ElectionItemViewModel(Core.DomainModels.UserElection election)
        {
            Name = election.Name;
            Id = election.Id;
            State = election.State;
            County = election.County;
            ContractAddress = election.ContractAddress;
            StartDate = election.StartDate;
            EndDate = election.EndDate;
            Rules = election.Rules;
            HasVoted = election.HasVoted;
            NumberOfVotes = election.NumberOfVotes;
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
        public int NumberOfVotes { get; set; }
    }
}
