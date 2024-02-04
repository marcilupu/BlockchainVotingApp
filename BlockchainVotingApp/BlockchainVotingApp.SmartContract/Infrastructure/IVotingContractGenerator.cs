
using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface IVotingContractGenerator : IElectionContractGenerator
    {
        Task<ContractMetadata?> CreateSmartContractContext(string contextIdentifier, IReadOnlyCollection<int> userIds);

        Task<Proof?> GenerateProof(string contextIdentifier, int userId);
    }
}
