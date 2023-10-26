using System.ComponentModel.DataAnnotations;

namespace BlockchainVotingApp.Models.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage = "The national ID is required"),
            StringLength(256, MinimumLength = 6, ErrorMessage = "The input has to be between 6 and 256 characters long")]
        public string NationalId { get; set; } = null!;

        [Required(ErrorMessage = "The password is required")]
        public string Password { get; set; } = null!;
    }
}
