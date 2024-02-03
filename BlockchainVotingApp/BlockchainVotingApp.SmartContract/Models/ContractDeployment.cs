using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace BlockchainVotingApp.SmartContract.Models
{

    [FunctionOutput]
    public class Vote : IFunctionOutputDTO
    {
        [Parameter("int", "CandidateId", 2)]
        public int CandidateId { get; set; }
    }

    [FunctionOutput]
    public class CandidateResult: IFunctionOutputDTO
    {
        [Parameter("int", "result", 2)]
        public int Result { get; set; }
    }

    public class ContractDeployment : ContractDeploymentMessage
    {

        static ContractDeployment()
        {
        }

        public ContractDeployment()  : base(string.Empty){ }

        public ContractDeployment(string bytecode) : base(bytecode) { }
       
    }
}
