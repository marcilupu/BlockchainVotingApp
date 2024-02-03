using BlockchainVotingApp.SmartContract.Extensions;
using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using BlockchainVotingApp.SmartContract.Utilities;
using System.Diagnostics;

namespace BlockchainVotingApp.SmartContract.Services
{
    internal sealed class VotingContractGenerator : ElectionContractGenerator, IVotingContractGenerator
    {
        public VotingContractGenerator(IContractConfiguration configuration, PathsLookup pathsLookup) : base(configuration, pathsLookup)
        {
        }

        protected override string Type => PathsLookup.VOTING;

        /// <summary>
        /// Generate the election context : election context, verifier zok file, zokrates context
        /// </summary>
        /// <param name="contextIdentifier"></param>
        /// <param name="usersIds"></param>
        /// <returns>A new instance of <see cref="ContractMetadata"/></returns>
        public async Task<ContractMetadata?> CreateSmartContractContext(string contextIdentifier, IReadOnlyCollection<int> usersIds)
        {
            // 1. Create election smart contract context setup
            string templatePath = _pathsLookup.GeneratorTemplatePath(Type);
            string contextPath = _pathsLookup.ContextPath(contextIdentifier, Type);
            string verifierZokPath = _pathsLookup.ContextVerifierProgramPath(contextIdentifier, Type);


            // 2. Create verifier zok file
            var verifierProgram = VotingProgramCreator.New(usersIds).Generate();


            if (!string.IsNullOrEmpty(verifierProgram))
            {
                // Attempt to copy the template directory content to election path directory.
                if (templatePath.TryCopyTo(contextPath))
                {
                    File.WriteAllText(verifierZokPath, verifierProgram);

                    // 3. Setup and run context generator. 
                    string generatorBat = _pathsLookup.CGeneratorBatPath();

                    var response = await new Process().InvokeBat(generatorBat, contextPath);

                    if (response != null)
                    {
                        // 4. Get contract metadata (abi, bytecode)
                        var contractMetadata = await GetSCMetadataInternal(contextIdentifier);

                        return contractMetadata;
                    }
                }
            }

            return null;
        }

        public async Task<VotingProof?> GenerateProof(string contextIdentifier, int userId)
        {
            // Setup the required path variables. 
            string generatorBat = _pathsLookup.PGeneratorBatPath(Type);
            string contextPath = _pathsLookup.ContextPath(contextIdentifier, Type);

            // Generate a new unique identifier for proof.
            string proofId = Guid.NewGuid().ToString();

            // Execute the bat and extract the proof from file.
            var response = await new Process().InvokeBat(generatorBat, contextPath, proofId, userId.ToString());

            if (response != null)
            {
                var proofPath = _pathsLookup.ContextVerifierProofPath(contextIdentifier, proofId, Type);

                if (VotingProof.TryRead(proofPath, out var proof))
                {
                    // TODO: Delete generated files after the proof was create...
                    // At the moment this will be commented for testing dev
                    //if (File.Exists(proofPath) && File.Exists(Path.Combine(proofPath + ".witness")))
                    //{
                    //    File.Delete(proofPath);
                    //    File.Delete(proofPath + ".witness");
                    //}

                    return proof;
                }
            }

            return null;
        }
    }
}
