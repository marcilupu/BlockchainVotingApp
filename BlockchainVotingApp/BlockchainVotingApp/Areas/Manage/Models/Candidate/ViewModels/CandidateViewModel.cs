using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Areas.Manage.Models.Candidate.ViewModels
{
    public class CandidateViewModel
    {
        public CandidateViewModel(List<DbCandidate> candidates) {
            Items = candidates.Select(item =>
            {
                return new CandidateItemViewModel()
                {
                    FullName = $"{item.FirstName} {item.LastName}",
                    Organization = item.Organization,
                    Biography= item.Biography,
                };
            }).ToList();
        }

        public List<CandidateItemViewModel> Items { get; set; }
    }
}
