using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Areas.Manage.Models.Elections.ViewModels
{
    public class AddElectionViewModel
    {
        public AddElectionViewModel(List<DbCounty> counties)
        {
            Counties = counties.Select(county =>
            {
                return (county.Name, county.Id);
            }).ToList();
        }
        public List<(string name, int id)> Counties { get; set; } = null!;
    }
}
