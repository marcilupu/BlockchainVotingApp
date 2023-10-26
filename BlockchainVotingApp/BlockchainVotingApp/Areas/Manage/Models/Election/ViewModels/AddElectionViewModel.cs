namespace BlockchainVotingApp.Areas.Manage.Models.Election.ViewModels
{
    public class AddElectionViewModel
    {
        public List<(string name, int id)> Counties { get; set; } = null!;
    }
}
