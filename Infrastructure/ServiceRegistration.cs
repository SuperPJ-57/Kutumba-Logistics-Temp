
using Application.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories;
using System.Reflection;

namespace Infrastructure;

public class ServiceRegistration : IServicesRegistrationWithConfig
{
    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter(configurator: c =>
        {
            c.WithValidatorLifetime(ServiceLifetime.Scoped);
        });
        services.AddHttpContextAccessor();  // Needed to access HttpContext

        services.Configure<JwtTokenSetting>(configuration.GetSection("JwtSettings"));

        // Add Tenant Migration Service
        //services.ConfigureServices();
        RepositoryServiceRegistration.ConfigureServices(services);
        // Current tenant service with scoped lifetime (created per each request)

    }
}


public class DatabaseRegistration : IDbServiceRegistration
{
    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<MainDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(MainDbContext).Assembly.FullName));
        });

    }
}


public class IdentityRegistration : IIdentityServicesRegistration
{
    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddEntityFrameworkStores<MainDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthorization();
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddCookie(IdentityConstants.ApplicationScheme, o =>
        {
            o.LoginPath = new PathString("/Account/Login");
            o.Events = new CookieAuthenticationEvents
            {
                OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
            };
        })
        .AddCookie(IdentityConstants.ExternalScheme, o =>
        {
            o.Cookie.Name = IdentityConstants.ExternalScheme;
            o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        })
        .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o =>
        {
            o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
            o.Events = new CookieAuthenticationEvents
            {
                OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
            };
        })
        .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
        {
            o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
            o.Events = new CookieAuthenticationEvents
            {
                OnRedirectToReturnUrl = _ => Task.CompletedTask
            };
            o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
    }
}


public class RepositoryRegistration : IRepositoriesRegistration
{
    public void AddServices(IServiceCollection services)
    {

        //services.AddScoped<IAccountsRepository, AccountsRepository>();
        services.AddScoped<IDriversRepository, DriversRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<ITripRepository, TripsRepository>();
        services.AddScoped<IConsignmentRepository, ConsignmentDetailsRepository>();
        services.AddScoped<IFreightRepository, FreightsRepository>();
        services.AddScoped<ITripRequestRepository, TripRequestRepository>();

    }
}

public class ServicesRegistration : IServicesRegistration
{
    public void AddServices(IServiceCollection services)
    {
       
        services.AddTransient<ITokenService, TokenService>();
    }
}