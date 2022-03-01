namespace RayoInfo.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.Contains("main"))
            {
                string reqString = context.Request.QueryString.Value;
                if (reqString.Contains("id"))
                {
                    context.Request.Headers.Remove("Authorization");
                    string token = reqString.Substring(reqString.LastIndexOf("=") + 1);
                    context.Request.Headers.Add("Authorization", $"Bearer {token}");
                }
            }

            await _next(context);
        }
    }
}
