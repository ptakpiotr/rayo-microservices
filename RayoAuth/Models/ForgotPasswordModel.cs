using System.ComponentModel.DataAnnotations;

namespace RayoAuth.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Compare(nameof(Email))]
        public string? ConfirmEmail { get; set; }
    }
}
