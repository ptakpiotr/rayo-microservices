using System.ComponentModel.DataAnnotations;

namespace RayoInfo.Models
{
    public class CommentCreateDTO
    {
        [EmailAddress]
        public string Author { get; set; }

        [Required]
        [MaxLength(150)]
        public string Content { get; set; }
        public int NewsId { get; set; }

    }
}
