// using BenchmarkDotNet.Running;
// using aspnet.webapi;
// BenchmarkRunner.Run<PaginationBenchmarks>();

using System.Text;
using aspnet.webapi.Data;
using aspnet.webapi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using aspnet.webapi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddScoped<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(builder.Configuration.GetConnectionString("database")!));

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("MyRedisConStr");
        options.InstanceName = "SampleInstance";
    });

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Jwt));

    builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value!)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
        };
    });

    builder.Services.AddAuthorization();

    // builder.Services.AddAuthorization(options =>
    // {
    //     options.AddPolicy("NOME_DA_POLICY", x => x.RequireRole("ROLE_OBRIGATORIO"));
    //     options.AddPolicy("Administrador", x => x.RequireRole("admin"));
    //     options.AddPolicy("Funcionario", x => x.RequireRole("employee"));
    // });

    builder.Services.AddDbContext<ApplicationDbContext>();

    builder.Services.AddControllers();

    builder.Services.AddTransient<TokenService>();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
    }

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseHttpsRedirection();

    app.MapControllers();
}

app.Run();
