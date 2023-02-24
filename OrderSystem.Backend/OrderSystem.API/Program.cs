using Microsoft.EntityFrameworkCore;
using OrderSystem.Core.Services;
using OrderSystem.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OrderSystem.API.Settings;
using Microsoft.OpenApi.Models;
using OrderSystem.Core.Settings;
using OrderSystem.API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Settings
var configurationBuilder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
                                                     .AddJsonFile("appsettings.json", false, true)
                                                     .AddJsonFile("secure.json", false, true)
                                                     .AddEnvironmentVariables();
var configuration = configurationBuilder.Build();
builder.Configuration.AddConfiguration(configuration);

var hashServiceSettings = configuration.GetSection(nameof(HashServiceSettings)).Get<HashServiceSettings>();
builder.Services.AddScoped(_ => hashServiceSettings);

var jwtSettings = configuration.GetSection(nameof(JwtBearerSettings)).Get<JwtBearerSettings>();
builder.Services.AddScoped(_ => jwtSettings);

// Authentication and Authorization
builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtSettings.GetSymmetricSecurityKey()
                    };
                });
builder.Services.AddAuthorization();

// Databases
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));

// Custom services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<HashService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserAuthenticationService>();

// ASP.NET Core Services
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", builder => builder.AllowAnyOrigin().AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "OrderSystem.API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Enter a valid jwt token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.OperationFilter<AuthenticationRequirementsOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();