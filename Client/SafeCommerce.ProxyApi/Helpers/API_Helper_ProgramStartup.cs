using Serilog;
using System.Text;
using AspNetCoreRateLimit;
using Microsoft.OpenApi.Models;
using System.Security.Authentication;
using Microsoft.IdentityModel.Tokens;
using SafeCommerce.ProxyApi.Helpers.Constants;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SafeCommerce.ProxyApi.Container.Services;
using SafeCommerce.ProxyApi.Container.Interfaces;
using SafeCommerce.Utilities.ConfigurationSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SafeCommerce.ProxyApi.Helpers;

internal static class API_Helper_ProgramStartup
{
    public static IServiceCollection InjectServices
    (
       this IServiceCollection Services,
       IConfiguration Configuration,
       IHostBuilder Host
    )
    {
        try
        {
            string? ClientName = Configuration.GetSection(API_Helper_Const_Request.ProxyClientName).Value;

            string? ClientBaseAddress = Configuration.GetSection(API_Helper_Const_Request.MainApiBaseAddr).Value;

            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(ClientName), ClientName),
                (nameof(ClientBaseAddress), ClientBaseAddress)
            );

            Services.AddControllers();
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();
            Services.AddMemoryCache();
            Services.AddHttpContextAccessor();

            Services.AddHttpClient(ClientName!, client =>
            {
                client.BaseAddress = new Uri(ClientBaseAddress!);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            ConfigureSerilog(Host);

            ConfigureTls(Services);

            ConfigureServices(Services);

            ConfigureCors(Services, Configuration);

            ConfigureSwagger(Services, Configuration);

            ConfigureIpRateLimiting(Services, Configuration);

            ConfigureAdditionalConfigurations(Services, Configuration);

            return Services;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static void
    ConfigureAdditionalConfigurations
    (
         IServiceCollection Services,
         IConfiguration Configuration
    )
    {
        Services.Configure<API_Helper_RequestHeaderSettings>(Configuration.GetSection(API_Helper_RequestHeaderSettings.SectionName));
    }

    private static void
    ConfigureServices
    (
        IServiceCollection Services
    )
    {
        Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

        Services.AddScoped<IItemProxyService, ItemProxyService>();
        Services.AddScoped<IShopProxyService, ShopProxyService>();
        Services.AddScoped<IProxyAuthentication, ProxyAuthentication>();
        Services.AddScoped<ICheckOutProxyService, CheckOutProxyService>();
        Services.AddScoped<IModerationProxyService, ModerationProxyService>();
        Services.AddScoped<IItemInvitationProxyService, ItemInvitationProxyService>();
        Services.AddScoped<IShopInvitationProxyService, ShopInvitationProxyService>();
        Services.AddScoped<IAccountManagmentProxyService, AccountManagmentProxyService>();
        Services.AddScoped<IRequestConfigurationProxyService, RequestConfigurationProxyService>();
    }

    private static void
    ConfigureCors
    (
        IServiceCollection Services,
        IConfiguration Configuration
    )
    {
        string? MainClientBaseAddr = Configuration.GetSection("MainClientBaseAddr").Value;

        string? AllowedMethodGet = Configuration.GetSection("RequestMethodsSettings:Get").Value;
        string? AllowedMethodPut = Configuration.GetSection("RequestMethodsSettings:Put").Value;
        string? AllowedMethodPost = Configuration.GetSection("RequestMethodsSettings:Post").Value;
        string? AllowedMethodDelete = Configuration.GetSection("RequestMethodsSettings:Delete").Value;

        API_Helper_ParamsStringChecking.CheckNullOrEmpty
        (
            (nameof(AllowedMethodGet), AllowedMethodGet),
            (nameof(AllowedMethodPut), AllowedMethodPut),
            (nameof(AllowedMethodPost), AllowedMethodPost),
            (nameof(MainClientBaseAddr), MainClientBaseAddr),
            (nameof(AllowedMethodDelete), AllowedMethodDelete)
        );

        Services.AddCors(options =>
        {
            options.AddPolicy(Configuration.GetSection("Cors:Policy:Name").Value!, builder =>
            {
                builder.WithOrigins(MainClientBaseAddr!)
                       .WithMethods
                        (
                            AllowedMethodGet!,
                            AllowedMethodPut!,
                            AllowedMethodPost!,
                            AllowedMethodDelete!
                        )
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
        });

    }

    private static void
    ConfigureIpRateLimiting
    (
        IServiceCollection Services,
        IConfiguration Configuration
    )
    {
        bool canParseLimitToInt = int.TryParse(Configuration.GetSection("IpRateLimitOptions:Limit").Value, out int Limit);

        if (canParseLimitToInt && Limit <= 0)
            throw new Exception("Api rate limiting can not be 0 or lower");

        bool canParsePeriodToInt = int.TryParse(Configuration.GetSection("IpRateLimitOptions:PeriodTimespan").Value, out int PeriodTimespan);

        if (canParsePeriodToInt && PeriodTimespan <= 0)
            throw new Exception("Api rate limiting Period Timespan can not be 0 or lower");

        Services.Configure<IpRateLimitOptions>(options =>
        {
            options.GeneralRules =
            [
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = Limit,
                    PeriodTimespan = TimeSpan.FromMinutes(PeriodTimespan)
                }
            ];
        });
    }

    private static void
    ConfigureSerilog
    (
        IHostBuilder Host
    )
    {
        Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
    }

    private static void
    ConfigureTls
    (
        IServiceCollection Services
    )
    {
        Services.Configure<KestrelServerOptions>(options =>
        {
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
            });
        });
    }

    private static void
    ConfigureSwagger
    (
        IServiceCollection Services,
        IConfiguration Configuration
    )
    {
        var jwtSetting = Configuration.GetSection(Util_JwtSettings.SectionName);

        var defaultTokanValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidIssuer = jwtSetting.GetSection("Issuer").Value,
            ValidAudience = jwtSetting.GetSection("Audience").Value,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.GetSection("Key").Value!)),
        };
        Services.AddSingleton(defaultTokanValidationParameters);
        Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        })
        .AddJwtBearer("Default", options =>
        {
            options.TokenValidationParameters = defaultTokanValidationParameters;
        })
        .AddJwtBearer("ConfirmLogin", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSetting.GetSection("Issuer").Value,
                ValidAudience = jwtSetting.GetSection("Audience").Value,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.GetSection("KeyConfrimLogin").Value!)),
            };
        });
        Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Default", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter 'Bearer' [Space] and then your token in the input field below.",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "Jwt"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Default"
                        },
                        Scheme = "0auth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            options.AddSecurityDefinition("ConfirmLogin", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter 'Bearer' [Space] and then your token in the input field below.",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "Jwt"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ConfirmLogin"
                        },
                        Scheme = "0auth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Name = "X-Api-Key",
                Description = "API Key authentication",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" },
                        Name = "X-Api-Key",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            options.SwaggerDoc(Configuration.GetSection("Swagger:Doc:Version").Value, new OpenApiInfo
            {
                Version = Configuration.GetSection("Swagger:Doc:Version").Value,
                Title = Configuration.GetSection("Swagger:Doc:Tittle").Value,
                License = new OpenApiLicense
                {
                    Name = Configuration.GetSection("Swagger:Doc:Licence:Name").Value,
                    Url = new Uri(Configuration.GetSection("Swagger:Doc:Licence:Url-Linkedin").Value!)
                }
            });

            //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            //options.IncludeXmlComments(xmlPath);
        });
    }
}