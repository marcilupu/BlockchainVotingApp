using BlockchainVotingApp.Core.DomainModels;

namespace BlockchainVotingApp.Models.Election.ViewModels
{
    public class VoteViewModel : ElectionCandidatesViewModel
    {
        public VoteViewModel(UserElection election, string selectedCandidate) : base(election)
        {
            SelectedCandidate = selectedCandidate;
        }

        public string SelectedCandidate { get; set; }
    }
}
