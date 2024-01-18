namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface ISmartContractConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public string BlockchainNetworkUrl { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public string AdminDefaultAccountAddress { get; }

        /// <summary>
        /// 
        /// </summary>
        public string AdminDefaultAccountPrivateKey { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public string GeneratorWorkspace { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ContextGenerator { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ProofGenerator { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ABI { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Bytecode { get; }
    }
}
