using Microsoft.AspNetCore.Authorization;

namespace RayoAuth.Data.Auth
{
    public class AdminAuthHandler<AdminAuthRequirement> : AuthorizationHandler<IAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                if (context.User.IsInRole("Admin"))
                {
                    context.Succeed(requirement);
                }
            }

            context.Fail();

            return Task.CompletedTask;
        }
    }
}
