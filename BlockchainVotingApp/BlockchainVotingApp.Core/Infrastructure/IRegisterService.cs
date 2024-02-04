using BlockchainVotingApp.Core.DomainModels;

namespace BlockchainVotingApp.Core.Infrastructure
{
    public interface IRegisterService
    {
        Task<bool> Register(AppUser user, int electionId, string proof);

        Task<byte[]?> GenerateProof(int electionId, int birthYear, int? countyId);
    }
}
