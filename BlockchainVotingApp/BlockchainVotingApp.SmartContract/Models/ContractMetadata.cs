namespace BlockchainVotingApp.SmartContract.Models
{
    /// <summary>
    /// Represents the metadata (abi and bytecode) of the smartcontract.
    /// </summary>
    public class ContractMetadata
    {
        public string Bytecode { get; init; }

        public string Abi { get; init; }


        public ContractMetadata(string bytecode, string abi)
        {
            Bytecode = bytecode;
            Abi = abi;
        }

    }
}
