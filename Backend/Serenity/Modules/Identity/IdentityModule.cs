using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serenity.Common.Interfaces;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Identity;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<DataContext>()
            .AddUserManager<UserManager<User>>()
            .AddSignInManager<SignInManager<User>>();

        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
            options.User.RequireUniqueEmail = true;

            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;

            options.SignIn.RequireConfirmedAccount = false;
        });

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            string secret = configuration.GetValue<string>("JwtAccessSecret");
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = key
            };
        });

        return services;
    }
}