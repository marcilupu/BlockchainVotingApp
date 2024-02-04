using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Services
{
    internal class RegisterContractServiceFactory : IRegisterContractServiceFactory
    {
        private readonly IContractConfiguration _configuration;

        public RegisterContractServiceFactory(IContractConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IRegisterContractService Create(ContractMetadata metadata)
        {
            return new RegisterContractService(metadata, _configuration);
        }
    }
}
