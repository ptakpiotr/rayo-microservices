namespace RayoAuth.Jobs
{
    public static class FetchStandingsMiddlewareExts
    {
        public static void UseFetchStanding(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<FetchStandingsMiddleware>();
        }
    }
}
