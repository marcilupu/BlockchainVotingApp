namespace BlockchainVotingApp.Data.Models
{
    public class DbElection
    {   
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ContractAddress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public int NumberOfVotes { get; set; }
        public string? Rules { get; set; } //eg. only the voters within a county can vote
        public ElectionState State { get; set; }

        public DbCounty? County { get; set; }
        public int? CountyId { get; set; }

        public ICollection<DbCandidate>? Candidates { get; set; }
    }

    public enum ElectionState
    {
        Registration = 0, //the users register into the app, add users and candidates into db, set election metadatas
        Upcoming = 1, // the last steps of the elections are set, others information to the election cannot be added.
        Ongoing = 2,
        Completed = 3
    }
}
