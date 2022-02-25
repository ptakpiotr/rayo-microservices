namespace RayoInfo.Middleware
{
    public static class AuthMiddlewareExts
    {
        public static void UseAuthMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
