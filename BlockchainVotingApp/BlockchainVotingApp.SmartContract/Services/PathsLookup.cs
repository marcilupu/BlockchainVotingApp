using BlockchainVotingApp.SmartContract.Infrastructure;

namespace BlockchainVotingApp.SmartContract.Services
{
    /// <summary>
    /// Contains all file system physical paths used by internal services.
    /// </summary>
    internal class PathsLookup
    {
        private readonly ISmartContractConfiguration _configuration;

        public PathsLookup(ISmartContractConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ContextPath(string contextIdentifier) => Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier);

        public string ContextVerifierProgramPath(string contextIdentifier) => Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier, "verifier", "Verifier.zok");

        public string ContextVerifierProofPath(string contextIdentifier, string proofIdentifier) => Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier, "verifier", proofIdentifier);

        public string ContextAbiPath(string contextIdentifier) => Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier, _configuration.ABI);

        public string ContextBytecodePath(string contextIdentifier) => Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier, _configuration.Bytecode);

        public string GeneratorTemplatePath() => Path.Combine(_configuration.GeneratorWorkspace, "template");

        public string CGeneratorBatPath() => Path.Combine(_configuration.GeneratorWorkspace, _configuration.ContextGenerator);
        
        public string PGeneratorBatPath() => Path.Combine(_configuration.GeneratorWorkspace, _configuration.ProofGenerator);
    }
}
