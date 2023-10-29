using BlockchainVotingApp.Models.Candidate;

namespace BlockchainVotingApp.Models.Election.ViewModels
{
    public class ElectionCandidatesViewModel : ElectionItemViewModel
    {
        public ElectionCandidatesViewModel(Core.DomainModels.UserElection election) : base(election)
        {
            Candidates = election.Candidates?.Select(item =>
            {
                return new CandidateItemViewModel(item);
            }).ToList() ?? new List<CandidateItemViewModel>();
        }

        public List<CandidateItemViewModel> Candidates { get; set; }
    }
}
