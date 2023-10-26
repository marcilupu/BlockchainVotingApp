namespace BlockchainVotingApp.Data.Models
{
    public class DbCandidate
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!; 
        public string LastName { get; set; } = null!;
        public string Organization { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public int ElectionId { get; set; }
        public DbElection? Election { get; set; }
    }
}
