using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.Extensions.Configuration;
 
namespace BlockchainVotingApp.SmartContract.Services
{
    internal class SmartContractConfiguration : ISmartContractConfiguration
    {
        public SmartContractConfiguration(IConfiguration configuration)
        {
            BlockchainNetworkUrl = configuration.GetSection("SmartContract:Connection:AdminDefaultAccountPrivateKey").Value;
            AdminDefaultAccountAddress = configuration.GetSection("SmartContract:Connection:AdminDefaultAccountPrivateKey").Value;
            AdminDefaultAccountPrivateKey = configuration.GetSection("SmartContract:Connection:AdminDefaultAccountPrivateKey").Value;
            GeneratorWorkspace = configuration.GetSection("SmartContract:Generation:Workspace").Value;
            ContextGenerator = configuration.GetSection("SmartContract:Generation:ContextGenerator").Value;
            ProofGenerator = configuration.GetSection("SmartContract:Generation:ProofGenerator").Value;
        }

        public string BlockchainNetworkUrl { get; init; }

        public string AdminDefaultAccountAddress { get; init; }

        public string AdminDefaultAccountPrivateKey { get; init; }

        public string GeneratorWorkspace { get; init; }

        public string ContextGenerator { get; init; }

        public string ProofGenerator { get; init; }
    }
}
