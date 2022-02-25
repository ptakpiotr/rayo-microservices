using System.ComponentModel.DataAnnotations;

namespace RayoInfo.Models
{
    public class NewsCreateDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Url]
        public string PhotoUrl { get; set; }

        [EmailAddress]
        public string Author { get; set; }
    }
}
