using System.ComponentModel.DataAnnotations;

namespace RayoAuth.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string? NewPassword { get; set; }

        [Compare(nameof(NewPassword))]
        public string? ConfirmNewPassword { get; set; }
    }
}
