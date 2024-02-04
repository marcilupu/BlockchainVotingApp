using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Areas.Manage.Models.Register
{
    public class RegisterViewModel
    {
        public RegisterViewModel(List<DbCounty> counties, List<Election> elections)
        {
            Counties = counties.Select(county =>
            {
                return (county.Name, county.Id);
            }).ToList();

            Elections = elections.ToDictionary(x => x.Name, x => x.Id);
        }
        public List<(string name, int id)> Counties { get; set; }
        public IDictionary<string, int> Elections { get;set; }
    }
}
