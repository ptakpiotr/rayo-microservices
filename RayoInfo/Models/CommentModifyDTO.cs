using System.Text.Json.Serialization;

namespace RayoInfo.Models
{
    public class CommentModifyDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("likes")]
        public int Likes { get; set; }

        [JsonPropertyName("dislikes")]
        public int Dislikes { get; set; }
    }
}
