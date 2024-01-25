using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainVotingApp.Data.Models
{
    public class DbUserVote
    {
        public int Id { get; set; }

        public DbUser? User { get; set; }
        public int UserId { get; set; }

        public DbElection? Election { get; set; }
        public int ElectionId { get; set; }

        public bool HasVoted { get; set; }
        public string? Message { get; set; }
    }
}
