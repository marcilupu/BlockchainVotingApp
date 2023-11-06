using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Data.Models;
using static System.Collections.Specialized.BitVector32;

namespace BlockchainVotingApp.Models.Election.ViewModels
{
    public class VotesViewModel
    {
        public VotesViewModel(List<UserElection> elections) {
            VotesList = elections.Select(item => {
                return new VoteItemViewModel(item);
            }).ToList();
        }

        public List<VoteItemViewModel> VotesList { get; set; }
    }

    public class VoteItemViewModel
    {
        public VoteItemViewModel(UserElection election)
        {
            ElectionId = election.Id;
            ElectionName = election.Name;
            CandidateName = election.SelectedCandidate;
            ElectionState = election.State;
            NumberOfVotes = election.NumberOfVotes;
        }
        public int ElectionId { get; set; }
        public string ElectionName { get; set; }
        public string CandidateName { get; set; }
        public ElectionState ElectionState { get; set; }
        public int NumberOfVotes { get; set; }
    }
}
