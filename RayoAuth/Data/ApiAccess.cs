using System.Text.Json;

namespace RayoAuth.Data
{
    public class ApiAccess
    {
        private const string url = "https://api.football-data.org/v2/competitions/PD/standings";
        private readonly IConfiguration _config;

        public ApiAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<StandingsModel> CallEndpoint()
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Auth-Token", _config["ApiKey"]);
            using (HttpResponseMessage resp = await client.GetAsync(url))
            {
                using (HttpContent content = resp.Content)
                {
                    string streamData = await content.ReadAsStringAsync();
                    TempModel sm = JsonSerializer.Deserialize<TempModel>(streamData);
                    Console.WriteLine(sm.Competition);
                    Console.WriteLine(sm.Standings);
                    StandingsModel data = new() { Competition = sm.Competition, Standings = sm.Standings };

                    return data;
                }
            }
        }
    }
}
