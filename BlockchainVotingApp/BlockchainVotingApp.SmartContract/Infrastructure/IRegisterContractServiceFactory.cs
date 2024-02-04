using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface IRegisterContractServiceFactory
    {
        public IRegisterContractService Create(ContractMetadata metadata);
    }
}
