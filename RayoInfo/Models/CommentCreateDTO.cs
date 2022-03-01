using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RayoInfo.Models
{
    public class CommentCreateDTO
    {
        [EmailAddress]
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [Required]
        [MaxLength(150)]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("newsId")]
        public string NewsId { get; set; }

    }
}
