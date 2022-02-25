using Hangfire;
using RayoAuth.RabbitMQ;

namespace RayoAuth.Jobs
{
    public class FetchStandingsJob : IFetchStandingsJob
    {
        private readonly IRecurringJobManager _man;
        private readonly IRabbitPublisher _pub;
        private readonly ApiAccess _api;

        public FetchStandingsJob(IRecurringJobManager man, IRabbitPublisher pub, ApiAccess api)
        {
            _man = man;
            _pub = pub;
            _api = api;
        }

        public async Task CallRemoteEndpoint()
        {
            StandingsModel sm = await _api.CallEndpoint();
            _pub.SendStandings(sm);

        }

        public void ExecuteJob()
        {
            _man.AddOrUpdate("CallRemoteEndpoint", () => CallRemoteEndpoint(), Cron.Daily);
        }
    }
}
