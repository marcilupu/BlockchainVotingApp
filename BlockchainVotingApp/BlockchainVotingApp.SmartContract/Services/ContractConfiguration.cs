using BlockchainVotingApp.SmartContract.Infrastructure;
using Microsoft.Extensions.Configuration;
 
namespace BlockchainVotingApp.SmartContract.Services
{
    internal class ContractConfiguration : IContractConfiguration
    {
        public ContractConfiguration(IConfiguration configuration)
        {
            BlockchainNetworkUrl = configuration.GetSection("SmartContract:Connection:BlockchainNetworkUrl").Value;
            AdminDefaultAccountAddress = configuration.GetSection("SmartContract:Connection:AdminDefaultAccountAddress").Value;
            AdminDefaultAccountPrivateKey = configuration.GetSection("SmartContract:Connection:AdminDefaultAccountPrivateKey").Value;
            GeneratorWorkspace = configuration.GetSection("SmartContract:Generation:Workspace").Value;
            ContextGenerator = configuration.GetSection("SmartContract:Generation:ContextGenerator").Value;
            ProofGenerator = configuration.GetSection("SmartContract:Generation:ProofGenerator").Value;
            ABI = configuration.GetSection("SmartContract:Generation:ABI").Value;
            Bytecode = configuration.GetSection("SmartContract:Generation:Bytecode").Value;
        }

        public string BlockchainNetworkUrl { get; init; }

        public string AdminDefaultAccountAddress { get; init; }

        public string AdminDefaultAccountPrivateKey { get; init; }

        public string GeneratorWorkspace { get; init; }

        public string ContextGenerator { get; init; }

        public string ProofGenerator { get; init; }
        public string ABI { get; init; }
        public string Bytecode { get; init; }
    }
}
