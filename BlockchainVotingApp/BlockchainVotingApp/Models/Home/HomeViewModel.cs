using System.Linq;

namespace BlockchainVotingApp.Models.Home
{
    public class HomeViewModel
    {
        public HomeViewModel(List<Core.DomainModels.Election> elections)
        {
            Elections = elections.ToDictionary(item => item.Name, item => item.Id);
        }

        public IDictionary<string, int> Elections { get; set; }
    }
}
