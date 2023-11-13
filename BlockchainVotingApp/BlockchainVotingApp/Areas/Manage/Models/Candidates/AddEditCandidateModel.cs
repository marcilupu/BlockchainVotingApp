using System.ComponentModel.DataAnnotations;

namespace BlockchainVotingApp.Areas.Manage.Models.Candidates
{
    public class AddEditCandidateModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = null!;


        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Organization name is required")]
        public string Organization { get; set; } = null!;

        [Required(ErrorMessage = "Biography is required")]
        public string Biography { get; set; } = null!;

        [Required(ErrorMessage = "Election is required")]
        public int ElectionId { get; set; }
    }
}
