using BlockchainVotingApp.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace BlockchainVotingApp.Areas.Manage.Models.Elections
{
    public class AddEditElectionModel
    {

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        public DateTime? StartDate { get; set; }
         
        public DateTime? EndDate { get; set; }
        
        public string? Rules { get; set; }
        
        public int? County { get; set; }
        
        public ElectionState State { get; set; }
    }
}
