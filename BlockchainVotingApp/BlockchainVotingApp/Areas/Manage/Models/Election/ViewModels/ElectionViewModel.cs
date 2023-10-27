using BlockchainVotingApp.Areas.Manage.Models.Candidate.ViewModels;
using BlockchainVotingApp.Data.Models;
using Org.BouncyCastle.Utilities;

namespace BlockchainVotingApp.Areas.Manage.Models.Election.ViewModels
{
    public class ElectionViewModel
    {
        public ElectionViewModel(List<DbElection> elections)
        {
            Elections = elections.Select(item =>
            {
                return new ElectionItemViewModel(item);
            }).ToList();     
        }

        public List<ElectionItemViewModel> Elections { get; set; }
    }
}
