using TemplumStudii.data;
using Microsoft.EntityFrameworkCore;
using templumStudii.repositories;
using templumStudii.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;
using TemplumStudii.repositories;
using TemplumStudii.converters;
Env.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();



builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
});

builder.Services.AddOpenApiDocument();
builder.Services.AddOpenApi();


var databaseUrl = builder.Configuration["DATABASE:URL"];

builder.Services.AddDbContext<TemplumStudiiContext>(options =>
    options.UseNpgsql(databaseUrl));
//builder.Services.AddDbContext<TemplumStudiiContext>(options =>
//    options.UseInMemoryDatabase("Templum"));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TimeRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TimeService>();
builder.Services.AddScoped<AuthServices>();

string? jwtKey = builder.Configuration["JWT:KEY"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey!)
                    ),

                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TemplumStudiiContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
