namespace RayoAuth.Jobs
{
    public interface IFetchStandingsJob
    {
        Task CallRemoteEndpoint();
        void ExecuteJob();
    }
}