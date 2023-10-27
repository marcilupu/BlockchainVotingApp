using BlockchainVotingApp.Areas.Manage.Models.Candidate.ViewModels;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Models.Home
{
    public class HomeViewModel
    {
        public HomeViewModel(List<DbCandidate> candidates)
        {
            Candidates = candidates.Select(item =>
            {
                return new CandidateItemViewModel()
                {
                    FullName = $"{item.FirstName} {item.LastName}",
                    Organization = item.Organization,
                    Biography = item.Biography,
                    Election = item.Election!.Name
                };
            }).ToList();
        }

        public List<CandidateItemViewModel> Candidates;
    }
}
