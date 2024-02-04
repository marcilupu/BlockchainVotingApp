using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.Core.DomainModels
{
    public class Election
    {
        public Election(DbElection election)
        {
            Candidates = election.Candidates?.Select(item =>
            {
                return new Candidate(item);
            }).ToList() ?? new List<Candidate>();

            Name = election.Name;
            Id = election.Id;
            State = election.State;
            ContractAddress = election.ContractAddress;
            RegisterContractAddress = election.RegisterContractAddress;
            StartDate = election.StartDate;
            EndDate = election.EndDate;
            Rules = election.Rules;
            NumberOfVotes= election.NumberOfVotes;
          
        }

        
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string? ContractAddress { get; set; } 

        public string? RegisterContractAddress { get; set; }  

        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string? Rules { get; set; }

        public int NumberOfVotes { get; set; }
        
        public ElectionState State { get; set; }
        
        public List<Candidate> Candidates { get; set; }
    }

    public class UserElection : Election
    {
        public UserElection(DbElection election) : base(election)
        {
     
        }
        public bool HasRegistered { get; set; }
        public bool HasVoted { get; set; }
        public string SelectedCandidate { get; set; } = null!;

    }
}
