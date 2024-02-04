using BlockchainVotingApp.SmartContract.Extensions;
using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using BlockchainVotingApp.SmartContract.Utilities;
using System.Diagnostics;

namespace BlockchainVotingApp.SmartContract.Services
{
    internal sealed class RegisterContractGenerator : ElectionContractGenerator, IRegisterContractGenerator
    {
        public RegisterContractGenerator(IContractConfiguration contractConfiguration, PathsLookup pathsLookup) : base(contractConfiguration, pathsLookup)
        {
        }

        protected override string Type => PathsLookup.REGISTRATION;

        public async Task<ContractMetadata?> CreateSmartContractContext(string contextIdentifier, int county)
        {
            // 1. Create election smart contract context setup
            string registrationPath = _pathsLookup.GeneratorTemplatePath(Type);
            string contextPath = _pathsLookup.ContextPath(contextIdentifier, Type);
            string verifierZokPath = _pathsLookup.ContextVerifierProgramPath(contextIdentifier, Type);

            // 2. Create verifier zok file
            var verifierProgram = RegistrationProgramCreator.New(county).Generate();

            if (!string.IsNullOrEmpty(verifierProgram))
            {
                // Attempt to copy the template directory content to election path directory.
                if (registrationPath.TryCopyTo(contextPath))
                {
                    File.WriteAllText(verifierZokPath, verifierProgram);

                    // 3. Setup and run context generator. 
                    string generatorBat = _pathsLookup.CGeneratorBatPath(Type);

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

        public async Task<Proof?> GenerateProof(string contextIdentifier, int county, int birthYear)
        {
            // Setup the required path variables. 
            string generatorBat = _pathsLookup.PGeneratorBatPath(Type);
            string contextPath = _pathsLookup.ContextPath(contextIdentifier, Type);

            // Generate a new unique identifier for proof.
            string proofId = Guid.NewGuid().ToString();

            // Execute the bat and extract the proof from file.
            var response = await new Process().InvokeBat(generatorBat, contextPath, proofId, birthYear.ToString(), county.ToString());

            if (response != null)
            {
                var proofPath = _pathsLookup.ContextVerifierProofPath(contextIdentifier, proofId, Type);

                if (Proof.TryRead(proofPath, out var proof))
                {
                    return proof;
                }
            }

            return null;
        }
    }
}
