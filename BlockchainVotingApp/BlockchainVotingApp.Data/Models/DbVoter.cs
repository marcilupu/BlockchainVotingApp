namespace BlockchainVotingApp.Data.Models
{
    public class DbVoter
    {
        public int Id { get; set; }
        public DbUser? User { get; set; }
        public string UserId { get; set; } = null!;
        public DbElection? Election { get; set; }
        public int ElectionId { get; set; }
    }
}
