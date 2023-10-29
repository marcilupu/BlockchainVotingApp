using BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels;
using BlockchainVotingApp.Data.Models;
using System.Reflection.Metadata.Ecma335;

namespace BlockchainVotingApp.Areas.Manage.Models.Elections.ViewModels
{
    public class ElectionsItemViewModel
    {
        public ElectionsItemViewModel(DbElection election)
        {
            Candidates = election.Candidates!.Select(item =>
            {
                return new CandidatesItemViewModel()
                {
                    FullName = $"{item.FirstName} {item.LastName}",
                    Organization= item.Organization,
                    Biography= item.Biography,
                    ElectionId= item.ElectionId,
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
        public List<CandidatesItemViewModel> Candidates { get; set; }
    }
}
