using BlockchainVotingApp.Areas.Manage.Models.Candidates;
using BlockchainVotingApp.Areas.Manage.Models.Elections;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.AppCode.Extensions
{
    public static class DTOExtensions
    {
        public static DbCandidate ToDb(this AddCandidateModel addCandidateModel, DbCandidate? dbCandidate = null)
        {
            dbCandidate ??= new DbCandidate() {
                FirstName= addCandidateModel.FirstName,
                LastName= addCandidateModel.LastName,
                Biography= addCandidateModel.Biography,
                Organization= addCandidateModel.Organization,
                ElectionId= addCandidateModel.ElectionId,
            };

            return dbCandidate;
        }

        public static DbElection ToDb(this AddElectionModel electionModel, DbElection? election = null)
        {
            election ??= new DbElection()
            {
                Name = electionModel.Name,
                ContractAddress = electionModel.ContractAddress,
                StartDate = electionModel.StartDate,
                EndDate = electionModel.EndDate,
                Rules = electionModel.Rules,
                CountyId = electionModel.County,
                State = ElectionState.Upcoming
            };

            return election;
        }
    }
}
