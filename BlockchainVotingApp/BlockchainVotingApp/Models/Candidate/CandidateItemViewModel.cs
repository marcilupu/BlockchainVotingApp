namespace BlockchainVotingApp.Models.Candidate
{
    public class CandidateItemViewModel
    {
        public string FullName { get; set; } = null!;
        public string Organization { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public string Election { get; set; } = null!;
        public int ElectionId { get; set; }
        public int Id { get; set; }
    }
}
