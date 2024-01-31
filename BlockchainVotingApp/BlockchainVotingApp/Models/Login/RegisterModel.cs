using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockchainVotingApp.Models.Login
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "County is required")]
        public int County { get; set; }

        [Required(ErrorMessage = "The national ID is required"),
    StringLength(256, MinimumLength = 6, ErrorMessage = "The input has to be between 6 and 256 characters long")]
        public string NationalId { get; set; } = null!;

        [Required(ErrorMessage = "The password is required")]
        public string Password { get; set; } = null!;
    }
}
