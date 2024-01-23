using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Models.Candidate;

namespace BlockchainVotingApp.Models.Election.ViewModels
{
    public class VoteModalViewModel
    {
        public VoteModalViewModel(IReadOnlyCollection<Core.DomainModels.Candidate> candidates)
        {
            Candidates = candidates.Select(c => new CandidateItemViewModel(c)).ToList();
        }

        public IReadOnlyCollection<CandidateItemViewModel> Candidates { get; set; }

    }
}
