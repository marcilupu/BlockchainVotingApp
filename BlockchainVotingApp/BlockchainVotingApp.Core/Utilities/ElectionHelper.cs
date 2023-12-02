using BlockchainVotingApp.SmartContract.Infrastructure;

namespace BlockchainVotingApp.AppCode.Utilities
{
    public static class ElectionHelper
    {
        public static async Task<int> GetElectionVotes(ISmartContractService smartContractService, string electionContractAddress)
        {
            int numberOfVotes = await smartContractService.GetTotalNumberOfVotes(electionContractAddress);

            return numberOfVotes;
        }
    }
}
