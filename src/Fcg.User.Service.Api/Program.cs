using Asp.Versioning;
using Fcg.User.Service.Api.ApiConfigurations;
using Fcg.User.Service.Api.ApiConfigurations.LogsConfig;
using Fcg.User.Service.Api.Filters;
using Fcg.User.Service.Api.Middlewares;
using Fcg.User.Service.Application.ApiSettings;
using Fcg.User.Service.Application.AppServices;
using Fcg.User.Service.Application.ClientContracts.GamePurchase;
using Fcg.User.Service.Application.ClientContracts.Jogo;
using Fcg.User.Service.Application.Interfaces;
using Fcg.User.Service.Domain.Interfaces;
using Fcg.User.Service.Infra.Clients.GamePurchase;
using Fcg.User.Service.Infra.Clients.Jogo;
using Fcg.User.Service.Infra.Contexts;
using Fcg.User.Service.Infra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Http.BatchFormatters;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region database

builder.Services.AddDbContext<AppDbContext>(options => options
    .UseLazyLoadingProxies()
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateGuidQueryParamsFilter>();
});

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

#region DIs

/// Applications
builder.Services.AddAbstractValidations();
builder.Services.AddScoped<IUsuarioAppService, UsuarioAppService>();
builder.Services.AddScoped<IUsuarioAutenticadoAppService, UsuarioAutenticadoAppService>();
builder.Services.AddScoped<IContaAppService, ContaAppService>();
builder.Services.AddScoped<IBibliotecaAppService, BibliotecaAppService>();

/// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Clients
builder.Services.AddScoped<IGamePurchaseServiceClient, GamePurchaseServiceClient>();
builder.Services.AddScoped<IGamesServiceClient, GamesServiceClient>();

#endregion

#region Swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Serviço de Usuários e Autentição", Version = "v1", Description = "Este serviço é o \"dono\" da identidade do usuário. Ele gerencia quem são os usuários e como eles se autenticam." });
    c.EnableAnnotations();
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Insira o token JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            new[] { "Bearer" }
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});

#endregion

#region NewRelic
builder.Services.Configure<NewRelicSettings>(builder.Configuration.GetSection("NewRelicSettings"));

var newRelicSettings = builder.Configuration.GetSection("NewRelicSettings").Get<NewRelicSettings>() 
    ?? throw new InvalidOperationException("NewRelicSettings não foi configurado corretamente.");
var newRelicOptions = Options.Create(newRelicSettings);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown")
    .WriteTo.Console()
    .WriteTo.DurableHttpUsingFileSizeRolledBuffers(
        requestUri: "https://log-api.newrelic.com/log/v1",
        textFormatter: new NewRelicFormatter(),
        batchFormatter: new ArrayBatchFormatter(),
        httpClient: new NewRelicHttpClient(newRelicOptions))
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

#region Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
#endregion

#region Jwt
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<IJwtAppService, JwtAppService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.SecretKey)),
        RoleClaimType = ClaimTypes.Role,
    };
});

builder.Services.AddAuthorization();
#endregion

#region HttpClientFactories
builder.Services.AddHttpClient("GamePurchaseService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:GamePurchaseApiUrl"]!);
});

builder.Services.AddHttpClient("GamesService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:GamesApiUrl"]!);
});
#endregion

var app = builder.Build();

#region Migrations

await app.ApplyMigrationsWithSeedsAsync();

#endregion

#region Middlewares

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

#endregion

app.Run();

public partial class Program { }