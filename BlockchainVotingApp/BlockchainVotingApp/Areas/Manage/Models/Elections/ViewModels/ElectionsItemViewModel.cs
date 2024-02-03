using BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Areas.Manage.Models.Elections.ViewModels
{
    public class ElectionsItemViewModel
    {
        public ElectionsItemViewModel(Election election)
        {
            Candidates = election.Candidates!.Select(item =>
            {
                return new CandidatesItemViewModel(item);
            }).ToList();

            Name = election.Name;
            Id = election.Id;
            State = election.State;
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
        public ElectionState State { get; set; }
        public List<CandidatesItemViewModel> Candidates { get; set; }
    }
}
