namespace BlockchainVotingApp.Models.Login.ViewModels
{
    public class LoginViewModel
    {
        public List<(string name, int id)> Counties { get; set; } = null!;
    }
}
