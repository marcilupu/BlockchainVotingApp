using BlockchainVotingApp.Core.DomainModels;

namespace BlockchainVotingApp.Areas.Manage.Models.Candidates.ViewModels
{
    public class CandidatesViewModel
    {
        public CandidatesViewModel(List<Candidate> candidates) {
            Items = candidates.Select(item =>
            {
                return new CandidatesItemViewModel(item);
            }).ToList();
        }

        public List<CandidatesItemViewModel> Items { get; set; }
    }
}
