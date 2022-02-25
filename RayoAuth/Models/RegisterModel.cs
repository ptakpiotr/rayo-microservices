using System.ComponentModel.DataAnnotations;

namespace RayoAuth.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }
}
