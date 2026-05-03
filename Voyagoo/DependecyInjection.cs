using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Voyagoo.Authentication;
using Voyagoo.Entities;
using Voyagoo.Persistence;
using Voyagoo.Services;
using Voyagoo.Settings;

namespace Voyagoo
{
    public static class DependecyInjection
    {

        public static IServiceCollection AddDependences(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers();

            services.AddAuthConfig(configuration);


            var connectionStrings = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string NOT Found");

            services.AddDbContext<VoyagooDbContext>(options =>
            options.UseSqlServer(connectionStrings));




            services.AddSwaggerServices();

            services.AddMappsterConfig();

            services.AddFluentValidationConfig();

            //services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IBookingService, BookingService>();

            services.AddScoped<ITourGuideService, TourGuideService>();
            services.AddScoped<ITourGuideBookingService, TourGuideBookingService>();



            return services;

        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;

        }

        private static IServiceCollection AddMappsterConfig(this IServiceCollection services)
        {
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            return services;

        }

        private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {


            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();

            return services;

        }


        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<VoyagooDbContext>();

            services.AddSingleton<IJwtProvider, JwtProvider>();


            //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();


            var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();


            

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                        ValidIssuer = jwtSettings?.Issuer,
                        ValidAudience = jwtSettings?.Audience
                    };
                
                }
                );



            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                //options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });


            services.AddOptions<EmailSettings>()
                     .BindConfiguration(EmailSettings.SectionName)
                        .ValidateDataAnnotations()
                        .ValidateOnStart();

            services.AddScoped<IEmailSender, EmailSender>();



            return services;

        }


    }



}

