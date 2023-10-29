namespace BlockchainVotingApp.Models.Election.ViewModels
{
    public class ElectionsViewModel
    {
        public ElectionsViewModel(List<Core.DomainModels.UserElection> elections)
        {
            Items = elections.Select(item =>
            {
                return new ElectionItemViewModel(item);
            }).ToList();
        }
        public List<ElectionItemViewModel> Items { get; set; }
    }
}
