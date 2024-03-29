﻿namespace BlockchainVotingApp.Models.Candidate
{
    public class CandidateItemViewModel
    {
        public CandidateItemViewModel(Core.DomainModels.Candidate item)
        {
            FullName = item.FullName;
            Organization = item.Organization;
            Biography = item.Biography;
            ElectionId = item.ElectionId;
            Id = item.Id;
        }


        public string FullName { get; set; } = null!;
        public string Organization { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public string Election { get; set; } = null!;
        public int ElectionId { get; set; }
        public int Id { get; set; }
    }
}
