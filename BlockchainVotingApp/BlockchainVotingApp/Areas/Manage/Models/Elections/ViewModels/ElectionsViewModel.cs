using BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Data.Models;
using Org.BouncyCastle.Utilities;
using System.Diagnostics.Metrics;

namespace BlockchainVotingApp.Areas.Manage.Models.Elections.ViewModels
{
    public class ElectionsViewModel 
    {
        public ElectionsViewModel(List<Election> elections, List<DbCounty> counties)
        {
            Elections = elections.Select(item =>
            {
                return new ElectionsItemViewModel(item);
            }).ToList();

            Counties = counties.Select(county =>
            {
                return (county.Name, county.Id);
            }).ToList();

            States = Enum.GetValues<ElectionState>().Select(x =>
            {
                return (x.ToString(), (int)x);
            }).ToList();
        }

        public List<ElectionsItemViewModel> Elections { get; set; }
        public List<(string name, int id)> Counties { get; set; }
        public List<(string name, int id)> States { get; set; }
    }
}
