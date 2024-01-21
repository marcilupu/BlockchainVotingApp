using BlockchainVotingApp.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace BlockchainVotingApp.Areas.Manage.Models.Elections
{
    public class AddEditElectionModel
    {

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Contract address is required")]
        public string ContractAddress { get; set; } = null!;

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }
        public string? Rules { get; set; }
        public int? County { get; set; }
        public ElectionState State { get; set; }
    }
}
