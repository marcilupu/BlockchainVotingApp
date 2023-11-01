using BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Data.Models;
using Org.BouncyCastle.Utilities;

namespace BlockchainVotingApp.Areas.Manage.Models.Elections.ViewModels
{
    public class ElectionsViewModel
    {
        public ElectionsViewModel(List<Election> elections)
        {
            Elections = elections.Select(item =>
            {
                return new ElectionsItemViewModel(item);
            }).ToList();     
        }

        public List<ElectionsItemViewModel> Elections { get; set; }
    }
}
