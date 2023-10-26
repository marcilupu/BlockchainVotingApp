using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Areas.Manage.Models.Election.ViewModels
{
    public class ElectionViewModel
    {
        public ElectionViewModel(List<DbElection> elections)
        {
            Elections = elections.Select(item =>
            {
                return new ElectionItemViewModel()
                {
                    Name= item.Name,
                    Id= item.Id,
                    State = item.State,
                    County = item.County?.Name,
                    ContractAddress= item.ContractAddress,
                    StartDate= item.StartDate,
                    EndDate= item.EndDate,
                    Rules = item.Rules
                };
            }).ToList();
        }

        public List<ElectionItemViewModel> Elections { get; set; } = null!;
    }

    public class ElectionItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContractAddress { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Rules { get; set; }
        public string? County { get; set; }
        public ElectionState State { get; set; }
    }
}
