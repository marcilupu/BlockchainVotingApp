using BlockchainVotingApp.Areas.Manage.Models.Candidates;
using BlockchainVotingApp.Areas.Manage.Models.Elections;
using BlockchainVotingApp.Data.Models;

namespace BlockchainVotingApp.AppCode.Extensions
{
    public static class DTOExtensions
    {
        public static DbCandidate ToDb(this AddEditCandidateModel addCandidateModel, DbCandidate? dbCandidate = null)
        {
            dbCandidate ??= new DbCandidate()
            {
                ElectionId = addCandidateModel.ElectionId
            };

            if (!string.IsNullOrEmpty(addCandidateModel.FirstName))
                dbCandidate.FirstName = addCandidateModel.FirstName;

            if (!string.IsNullOrEmpty(addCandidateModel.LastName))
                dbCandidate.LastName = addCandidateModel.LastName;

            if (!string.IsNullOrEmpty(addCandidateModel.Biography))
                dbCandidate.Biography = addCandidateModel.Biography;

            if (!string.IsNullOrEmpty(addCandidateModel.Organization))
                dbCandidate.Organization = addCandidateModel.Organization;

            return dbCandidate;
        }

        public static DbElection ToDb(this AddEditElectionModel electionModel, DbElection? election = null)
        {
            election ??= new DbElection()
            {
                State = ElectionState.Registration,
                CreationDate = DateTime.Now,
            };

            election.Name = electionModel.Name;
            election.StartDate = electionModel.StartDate ?? DateTime.Now;
            election.EndDate = electionModel.EndDate ?? DateTime.Now.AddMonths(1);
            election.Rules = electionModel.Rules;
            election.State = electionModel.State;
            
            return election;
        }

        public static DbUserVote ToDb(int userId, int electionId, DbUserVote? userVote = null)
        {
            userVote ??= new DbUserVote();

            userVote.UserId = userId;
            userVote.ElectionId = electionId;

            return userVote;
        }
    }
}
