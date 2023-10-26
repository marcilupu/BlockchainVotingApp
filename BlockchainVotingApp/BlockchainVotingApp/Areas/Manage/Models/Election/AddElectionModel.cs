using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Areas.Manage.Models.Election
{
    public class AddElectionModel
    {
        public string Name { get; set; } = null!;
        public string ContractAddress { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Rules { get; set; }
        public int? County { get; set; }
    }
}
