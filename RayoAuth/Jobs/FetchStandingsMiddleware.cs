namespace RayoAuth.Jobs
{
    public class FetchStandingsMiddleware
    {
        private readonly RequestDelegate _next;

        public FetchStandingsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext ctx, IFetchStandingsJob job)
        {
            job.ExecuteJob();

            await _next(ctx);
        }
    }
}
