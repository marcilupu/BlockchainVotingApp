using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface IElectionContractGenerator
    {
        Task<string> DeploySmartContract(string contextIdentifier, string adminAccountPrivateKey);

        Task<ContractMetadata?> GetSmartContractMetadata(string contextIdentifier);
    }
}
