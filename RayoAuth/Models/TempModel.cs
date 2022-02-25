using System.Text.Json.Serialization;

namespace RayoAuth.Models
{
    public class TempModel
    {

        [JsonPropertyName("competition")]
        public Competition Competition { get; set; }

        [JsonPropertyName("standings")]
        public List<Standing> Standings { get; set; }
    }
}
