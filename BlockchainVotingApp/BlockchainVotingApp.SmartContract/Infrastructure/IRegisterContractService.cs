using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface IRegisterContractService : IElectionContractService
    {
        Task<ExecutionResult> Register(Proof voterProof, string contractAddress);
    }
}
