using BlockchainVotingApp.SmartContract.Infrastructure;

namespace BlockchainVotingApp.AppCode.Utilities
{
    public static class ElectionHelper
    {
        public static string CreateContextIdentifier(int id, string name) => $"{id}_{name}";

        public static async Task<IVotingContractService?> CreateSmartContractService(IVotingContractServiceFactory smartContractServiceFactory, IVotingContractGenerator smartContractGenerator, int electionId, string electioName)
        {
            var contextIdentifier = CreateContextIdentifier(electionId, electioName);

            var contextMetadata = await smartContractGenerator.GetSmartContractMetadata(contextIdentifier);

            if (contextMetadata != null)
            {
                return smartContractServiceFactory.Create(contextMetadata);
            }

            return null;
        }
    }
}
