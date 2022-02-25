using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RayoAuth.Data.Auth;
using RayoAuth.Jobs;
using RayoAuth.Profiles;
using RayoAuth.RabbitMQ;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

// Add services to the container.
services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseNpgsql(configuration.GetConnectionString("MainConn"));
})
    .AddIdentity<IdentityUser, IdentityRole>(opts =>
    {
        opts.SignIn.RequireConfirmedEmail = true;
        opts.User.RequireUniqueEmail = true;    
    }).AddRoles<IdentityRole>()
    .AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

services.AddSingleton<ApiAccess>();
services.AddCors((opts) =>
{
    opts.AddPolicy("AllowAnyonePolicy", policyOpts =>
    {
        policyOpts.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:3000");
    });
});

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer =true,
        ValidateAudience=true,
        ValidateLifetime=true,
        ValidateIssuerSigningKey=true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

services.AddAuthorization(opts =>
{
    opts.AddPolicy("AdminPolicy", policyOpts =>
    {
        policyOpts.AddRequirements(new AdminAuthRequirement());
    });
});

services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddHangfire(opts =>
{
    opts.UsePostgreSqlStorage(configuration.GetConnectionString("HangConn"));
});

services.AddHangfireServer();

services.AddAutoMapper(typeof(MainProfile));
services.AddSingleton<IRabbitPublisher, RabbitPublisher>();
services.AddSingleton<IFetchStandingsJob, FetchStandingsJob>();

services.AddScoped<IAuthorizationHandler,AdminAuthHandler<AdminAuthRequirement>>();
services.AddScoped<IEmailSender, FluentEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAnyonePolicy");
app.UseRouting();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseFetchStanding();
app.MapControllers();
app.MapHangfireDashboard("/dashboard");

app.Run();
