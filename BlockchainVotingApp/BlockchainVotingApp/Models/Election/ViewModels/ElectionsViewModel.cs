using BlockchainVotingApp.Areas.Manage.Models.Election.ViewModels;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Models.Election.ViewModels
{
    public class ElectionsViewModel
    {
        public ElectionsViewModel(List<DbElection> elections)
        {
            Items = elections.Select(item =>
            {
                return new ElectionItemViewModel(item);
            }).ToList();
        }
        public List<ElectionItemViewModel> Items { get; set; }
    }
}
