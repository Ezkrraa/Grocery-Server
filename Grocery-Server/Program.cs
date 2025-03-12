using Grocery_Server.Models;
using Grocery_Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using SwaggerThemes;
using System.Text;


namespace Grocery_Server
{
    public enum RateLimiters
    {
        ReallySlow,
        Slow,
        Fast,
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DbContext>();

            builder
                .Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Bearer";
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                        ),
                    };
                });

            builder
                .Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DbContext>()
                .AddUserStore<UserStore<User, IdentityRole, DbContext>>();

            builder.Services.AddTransient<JwtService>();
            builder.Services.AddSingleton<DbCleanupService>();


            builder.Services.AddRateLimiter(options =>
            options.AddFixedWindowLimiter(
                policyName: nameof(RateLimiters.ReallySlow),
                settings =>
            {
                settings.PermitLimit = 10;
                settings.Window = TimeSpan.FromMinutes(30);
            }));

            builder.Services.AddRateLimiter(options =>
            options.AddFixedWindowLimiter(
                policyName: nameof(RateLimiters.Slow),
                settings =>
            {
                settings.PermitLimit = 15;
                settings.Window = TimeSpan.FromMinutes(10);
            }));

            builder.Services.AddRateLimiter(options =>
            options.AddFixedWindowLimiter(
                policyName: nameof(RateLimiters.Fast),
                settings =>
            {
                settings.PermitLimit = 10;
                settings.Window = TimeSpan.FromSeconds(30);
            }));


            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerThemes(Theme.UniversalDark);
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseRateLimiter();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
