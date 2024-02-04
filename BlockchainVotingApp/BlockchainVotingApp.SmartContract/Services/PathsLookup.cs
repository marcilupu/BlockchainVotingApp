using BlockchainVotingApp.SmartContract.Infrastructure;

namespace BlockchainVotingApp.SmartContract.Services
{
    /// <summary>
    /// Contains all file system physical paths used by internal services.
    /// </summary>
    internal class PathsLookup
    {
        public const string VOTING = "voting";
        public const string REGISTRATION = "registration";


        private readonly IContractConfiguration _configuration;

        public PathsLookup(IContractConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ContextPath(string contextIdentifier, string contractType) => Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier, contractType);

        public string ContextVerifierProgramPath(string contextIdentifier, string contractType) => Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier, contractType, "verifier", "Verifier.zok");

        public string ContextVerifierProofPath(string contextIdentifier, string proofIdentifier, string contractType) => Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier, contractType, "verifier", proofIdentifier);

        public string ContextAbiPath(string contextIdentifier, string contractType) => Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier, contractType, _configuration.ABI);

        public string ContextBytecodePath(string contextIdentifier, string contractType) => Path.Combine(_configuration.GeneratorWorkspace, contextIdentifier, contractType, _configuration.Bytecode);

        public string GeneratorTemplatePath(string contractType) => Path.Combine(_configuration.GeneratorWorkspace, "template", contractType);

        public string CGeneratorBatPath(string contractType) => Path.Combine(_configuration.GeneratorWorkspace, "helpers", contractType, _configuration.ContextGenerator);

        public string PGeneratorBatPath(string contractType) => Path.Combine(_configuration.GeneratorWorkspace, "helpers", contractType, _configuration.ProofGenerator);
    }
}
