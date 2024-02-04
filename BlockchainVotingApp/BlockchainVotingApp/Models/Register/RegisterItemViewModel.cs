using BlockchainVotingApp.Data.Models;
using Nethereum.Contracts.Standards.ENS.Registrar.ContractDefinition;

namespace BlockchainVotingApp.Models.Register
{
    public class RegisterItemViewModel
    {
        public RegisterItemViewModel(Core.DomainModels.UserElection election)
        {
            Name = election.Name;
            Id = election.Id;
            State = election.State;
            Rules = string.IsNullOrEmpty(election.Rules) ? "No rules yet" : election.Rules;
            HasRegistered = election.HasRegistered;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Rules { get; set; }
        public ElectionState State { get; set; }
        public bool HasRegistered { get; set; }
    }
}
