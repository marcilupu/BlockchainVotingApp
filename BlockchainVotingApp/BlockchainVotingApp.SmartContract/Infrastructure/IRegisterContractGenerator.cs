
using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface IRegisterContractGenerator : IElectionContractGenerator
    {
        Task<ContractMetadata?> CreateSmartContractContext(string contextIdentifier, int? county);

        Task<Proof?> GenerateProof(string contextIdentifier, int? county, int birthYear);
    }
}
