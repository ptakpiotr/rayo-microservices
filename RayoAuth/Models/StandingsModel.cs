using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RayoAuth.Models
{

    public partial class StandingsModel
    {
        [Key]
        public int Id { get; set; }

        [JsonPropertyName("competition")]
        public Competition Competition { get; set; }

        [JsonPropertyName("Standings")]
        public List<Standing> Standings { get; set; }
    }

    public partial class Competition
    {
        [Key]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("plan")]
        public string Plan { get; set; }
    }

    public partial class Standing
    {
        [Key]
        public int Id{ get; set; }

        [JsonPropertyName("stage")]
        public string Stage { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
  
        [JsonPropertyName("table")]

        public List<Table> Table { get; set; }
    }

    public partial class Table
    {
        [Key]
        public int Id { get; set; }

        [JsonPropertyName("position")]
        public long Position { get; set; }

        [JsonPropertyName("team")]
        public Team Team { get; set; }

        [JsonPropertyName("playedGames")]
        public long PlayedGames { get; set; }

        [JsonPropertyName("won")]
        public long Won { get; set; }

        [JsonPropertyName("draw")]

        public long Draw { get; set; }

       [JsonPropertyName("lost")]

        public long Lost { get; set; }

        [JsonPropertyName("points")]

        public long Points { get; set; }
    }

    public partial class Team
    {
        [Key]
        public long Id { get; set; }

        [JsonPropertyName("name")]

        public string Name { get; set; }

        [JsonPropertyName("crestUrl")]

        public Uri CrestUrl { get; set; }
    }
}
