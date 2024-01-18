using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface ISmartContractServiceFactory
    {
        public ISmartContractService Create(ContractMetadata metadata);
    }
}
