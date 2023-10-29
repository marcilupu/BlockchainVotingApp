using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels
{
    public class CandidatesViewModel
    {
        public CandidatesViewModel(List<DbCandidate> candidates) {
            Items = candidates.Select(item =>
            {
                return new CandidatesItemViewModel(item);
            }).ToList();
        }

        public List<CandidatesItemViewModel> Items { get; set; }
    }
}
