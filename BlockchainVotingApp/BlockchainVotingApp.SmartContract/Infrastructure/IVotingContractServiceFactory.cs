using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface IVotingContractServiceFactory
    {
        public IVotingContractService Create(ContractMetadata metadata);
    }
}
