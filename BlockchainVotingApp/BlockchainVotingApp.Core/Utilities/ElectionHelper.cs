using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.SmartContract.Infrastructure;

namespace BlockchainVotingApp.AppCode.Utilities
{
    public static class ElectionHelper
    {
        public static string GetElectionContextIdentifier(int id, string name)
        {
            return $"{id}_{name}";
        }

        public static async Task<ISmartContractService> CreateSmartContractService(ISmartContractServiceFactory smartContractServiceFactory, ISmartContractGenerator smartContractGenerator, int electionId, string electioName)
        {
            string contextIdentifier = GetElectionContextIdentifier(electionId, electioName);
            var contextMetadata = await smartContractGenerator.GetSmartContractMetadata(contextIdentifier);

            if (contextMetadata != null)
                return smartContractServiceFactory.Create(contextMetadata);

            return null;
        }
    }
}
