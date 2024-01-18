using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
 
namespace BlockchainVotingApp.SmartContract.Services
{
    internal class SmartContractServiceFactory : ISmartContractServiceFactory
    {

        private readonly ISmartContractConfiguration _configuration;

        public SmartContractServiceFactory(ISmartContractConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ISmartContractService Create(ContractMetadata metadata)
        {
            return new SmartContractService(metadata, _configuration);
        }
    }
}
