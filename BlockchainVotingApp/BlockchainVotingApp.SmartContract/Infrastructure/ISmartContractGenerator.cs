
using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface ISmartContractGenerator
    {
        Task<string> DeploySmartContract(string contextIdentifier, string adminAccountPrivateKey);

        Task<ContractMetadata?> CreateSmartContractContext(string contextIdentifier, IReadOnlyCollection<int> userIds);

        Task<ContractMetadata?> GetSmartContractContext(string contextIdentifier);

        Task GenerateProof(string contextIdentifier, int userId);
    }
}
