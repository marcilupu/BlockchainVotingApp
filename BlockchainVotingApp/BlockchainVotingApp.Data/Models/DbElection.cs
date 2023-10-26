namespace BlockchainVotingApp.Data.Models
{
    public class DbElection
    {   
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContractAddress { get; set; } = null!; 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Rules { get; set; } //eg. only the voters within a county can vote
        public ElectionState State { get; set; }

        public DbCounty? County { get; set; }
        public int? CountyId { get; set; }
    }

    public enum ElectionState
    {
        Upcoming = 0,
        Ongoing = 1,
        Completed = 2
    }
}
