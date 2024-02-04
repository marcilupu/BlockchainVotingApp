using BlockchainVotingApp.AppCode.Utilities;
using BlockchainVotingApp.Core.DomainModels;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Models;
using BlockchainVotingApp.Data.Repositories;
using BlockchainVotingApp.SmartContract.Infrastructure;
using BlockchainVotingApp.SmartContract.Models;
using Nethereum.Contracts.Standards.ProofOfHumanity.ContractDefinition;
using Newtonsoft.Json;

namespace BlockchainVotingApp.Core.Services
{
    internal class RegisterService : IRegisterService
    {
        private readonly IUserVoteRepository _userVoteRepository;
        private readonly IElectionService _electionService;
        private readonly IElectionRepository _electionRepository;

        private readonly IQRGenerator _qrCodeGenerator;

        private readonly IRegisterContractGenerator _registerContractGenerator;
        private readonly IRegisterContractServiceFactory _registerContractServiceFactory;

        public RegisterService(IUserVoteRepository userVoteRepository,
                               IRegisterContractServiceFactory registerContractServiceFactory,
                               IElectionService electionService,
                               IRegisterContractGenerator registerContractGenerator,
                               IElectionRepository electionRepository,
                               IQRGenerator qrCodeGenerator)
        {
            _userVoteRepository = userVoteRepository;
            _registerContractServiceFactory = registerContractServiceFactory;
            _electionService = electionService;
            _registerContractGenerator = registerContractGenerator;
            _electionRepository = electionRepository;
            _qrCodeGenerator = qrCodeGenerator;
        }

        public async Task<bool> Register(AppUser user, int electionId, string proof)
        {
            // Check proof for the registration to the election
            var election = await _electionService.GetUserElection(electionId, user);

            if (election != null && !string.IsNullOrEmpty(election.RegisterContractAddress))
            {
                var contextIdentifier = ElectionHelper.CreateContextIdentifier(election.Id, election.Name);

                var contractMetadata = await _registerContractGenerator.GetSmartContractMetadata(contextIdentifier);

                if (contractMetadata != null)
                {
                    var registerContractService = _registerContractServiceFactory.Create(contractMetadata);

                    Proof voterProof = JsonConvert.DeserializeObject<Proof>(proof);

                    var result = await registerContractService.Register(voterProof, election.RegisterContractAddress);

                    if(result.IsSuccess)
                    {
                        var userVote = await _userVoteRepository.Get(user.Id, election.Id);
                        if(userVote!= null)
                        {
                            await _userVoteRepository.Update(user.Id, election.Id, false);
                        }
                        else
                        {
                            var newUserVote = new DbUserVote()
                            {
                                UserId = user.Id,
                                ElectionId = election.Id,
                                HasVoted = false,
                            };

                            await _userVoteRepository.Insert(newUserVote);
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<byte[]?> GenerateProof(int electionId, int birthYear, int? countyId)
        {
            // Check proof for the registration to the election
            var election = await _electionRepository.Get(electionId);

            if (election != null && !string.IsNullOrEmpty(election.RegisterContractAddress))
            {
                var contextIdentifier = ElectionHelper.CreateContextIdentifier(election.Id, election.Name);

                var proof = await _registerContractGenerator.GenerateProof(contextIdentifier, countyId, birthYear);

                if(proof != null)
                {
                    string proofToString = JsonConvert.SerializeObject(proof);

                    var imageBytes = _qrCodeGenerator.CreateCode(proofToString);

                    return imageBytes;
                }
            }
            return null;
        }
    }
}
