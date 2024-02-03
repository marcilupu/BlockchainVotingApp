
using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface IVotingContractGenerator : IElectionContractGenerator
    {
        Task<ContractMetadata?> CreateSmartContractContext(string contextIdentifier, IReadOnlyCollection<int> userIds);

        Task<VotingProof?> GenerateProof(string contextIdentifier, int userId);
    }
}
