using EvaluateItEasily.Core;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.Entities;
using EvaluateItEasily.Infrastructure.Data;
using EvaluateItEasily.Infrastructure.Middlewares;
using EvaluateItEasily.Infrastructure.Options;
using EvaluateItEasily.Infrastructure.Services;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using System.Text;

namespace EvaluateItEasily.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDatabase(configuration)
                .AddIdentity()
                .AddRepositories()
                .AddServices()
                .AddAuthentication(configuration)
                .AddValidators()
                .AddMapping()
                .AddOptions( configuration);
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<JwtProvider>();

            services.AddHttpContextAccessor();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            return services;
        }

        private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName).ValidateDataAnnotations().ValidateOnStart();
            return services;
        }

        // -----------------------------------------------

        private static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string cs = configuration.GetSection("constr").Value!;
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(cs,b=>b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            return services;
        }

        // -----------------------------------------------

        private static IServiceCollection AddIdentity(
            this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }

        // -----------------------------------------------

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        // -----------------------------------------------

        private static IServiceCollection AddServices(
            this IServiceCollection services)
        {
            

            return services;
        }

        // -----------------------------------------------

        private static IServiceCollection AddAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            var settings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings!.key)),
                    ValidIssuer = settings.issuer,
                    ValidAudience = settings.audience,
                };
            });

            return services;
        }

        // -----------------------------------------------

        private static IServiceCollection AddValidators(
            this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
            return services;
        }

        // -----------------------------------------------

        private static IServiceCollection AddMapping(this IServiceCollection services)
        {
            services.AddMapster();

            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton<IMapper>(new Mapper(TypeAdapterConfig.GlobalSettings));

            return services;
        }
    }
}
