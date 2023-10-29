using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels
{
    public class CandidatesViewModel
    {
        public CandidatesViewModel(List<DbCandidate> candidates) {
            Items = candidates.Select(item =>
            {
                return new CandidatesItemViewModel()
                {
                    FullName = $"{item.FirstName} {item.LastName}",
                    Organization = item.Organization,
                    Biography= item.Biography,
                };
            }).ToList();
        }

        public List<CandidatesItemViewModel> Items { get; set; }
    }
}
