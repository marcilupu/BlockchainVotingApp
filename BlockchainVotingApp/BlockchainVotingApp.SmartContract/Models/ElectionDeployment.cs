using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Reflection;
using System.Text;

namespace BlockchainVotingApp.SmartContract.Models
{

    [FunctionOutput]
    public class Vote : IFunctionOutputDTO
    {
        [Parameter("int", "VoterId", 1)]
        public int VoterId { get; set; }

        [Parameter("int", "CandidateId", 2)]
        public int CandidateId { get; set; }
    }


    public class ElectionDeployment : ContractDeploymentMessage
    {
        //public static readonly string BYTECODE;
        //public static readonly string ABI;

        static ElectionDeployment()
        {
            //BYTECODE = GetResource("BlockchainVotingApp.SmartContract.Contracts.contracts_Election_sol_Election.bin").Trim('"');
            //ABI = GetResource("BlockchainVotingApp.SmartContract.Contracts.contracts_Election_sol_Election.abi");
        }

        public ElectionDeployment()  : base(string.Empty){ }

        public ElectionDeployment(string bytecode) : base(bytecode) { }
 
        #region Internals

        private static string GetResource(string resourceName)
        {
            using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            if (resourceStream != null)
            {
                StreamReader reader = new StreamReader(resourceStream, Encoding.UTF8);

                return reader.ReadToEnd();
            }
            else
            {
                throw new ApplicationException("Failed to fetch bytecode for smart contract");
            }
        }

        #endregion
    }
}
