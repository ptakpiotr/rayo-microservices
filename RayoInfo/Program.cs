using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RayoInfo.GraphQL;
using RayoInfo.GraphQL.Types;
using RayoInfo.Hubs;
using RayoInfo.Middleware;
using RayoInfo.RabbitMQ;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddPooledDbContextFactory<AppDbContext>(opts =>
{
    opts.UseNpgsql(configuration.GetConnectionString("MainConn"));
});

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer((opts) =>
{
    opts.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

services.AddAuthorization();
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.AddSignalR();

services.AddHostedService<EventSubscriber>();

services.AddCors((opts) =>
{
    opts.AddPolicy("AllowAnyonePolicy", policyOpts =>
    {
        policyOpts.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:3000");
    });
});

services.AddGraphQLServer()
        .AddQueryType<Query>()
        .AddType<CommentType>()
        .AddType<NewsType>()
        .AddMutationType<Mutation>()
        .AddAuthorization()
        .AddSorting()
        .AddFiltering()
        .AddProjections();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowAnyonePolicy");

app.UseAuthMiddleware();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
app.MapGraphQLVoyager();
app.MapHub<MainHub>("/main");
app.Run();
