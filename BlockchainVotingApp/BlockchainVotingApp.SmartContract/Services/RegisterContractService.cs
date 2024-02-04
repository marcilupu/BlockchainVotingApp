using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;

namespace BlockchainVotingApp.SmartContract.Services
{
    internal class RegisterContractService : ElectionContractService, IRegisterContractService
    {
        public RegisterContractService(ContractMetadata metadata, IContractConfiguration configuration) : base(metadata, configuration)
        {
        }

        public async Task<ExecutionResult> Register(Proof registerProof, string contractAddress)
        {
            return (await Post(contractAddress, "register",
                registerProof.AX,
                registerProof.AY,
                registerProof.B0X,
                registerProof.B1X,
                registerProof.B0Y,
                registerProof.B1Y,
                registerProof.CX,
                registerProof.CY));
        }
    }
}
