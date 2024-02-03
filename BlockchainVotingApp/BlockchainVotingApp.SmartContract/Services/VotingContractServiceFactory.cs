using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
 
namespace BlockchainVotingApp.SmartContract.Services
{
    internal class VotingContractServiceFactory : IVotingContractServiceFactory
    {

        private readonly IContractConfiguration _configuration;

        public VotingContractServiceFactory(IContractConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IVotingContractService Create(ContractMetadata metadata)
        {
            return new VotingContractService(metadata, _configuration);
        }
    }
}
