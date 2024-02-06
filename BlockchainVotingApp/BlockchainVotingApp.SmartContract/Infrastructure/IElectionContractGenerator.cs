using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface IElectionContractGenerator
    {
        Task<string> DeploySmartContract(string contextIdentifier, string adminAccountPrivateKey);

        Task<ContractMetadata?> GetSmartContractMetadata(string contextIdentifier);

        /// <summary>
        /// Remove all data associated with the given context.
        /// </summary>
        bool Cleanup(string contextIdentifier);
    }
}
