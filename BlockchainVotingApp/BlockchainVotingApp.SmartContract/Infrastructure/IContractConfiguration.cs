namespace BlockchainVotingApp.SmartContract.Infrastructure
{
    public interface IContractConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public string BlockchainNetworkUrl { get; }

        [Obsolete("Asta trebuie sters la final")]
        /// <summary>
        /// 
        /// </summary>
        public string AdminDefaultAccountAddress { get; }

        [Obsolete("Asta trebuie sters la final")]
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
