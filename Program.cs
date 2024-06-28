// using BenchmarkDotNet.Running;
// using aspnet.webapi;
// BenchmarkRunner.Run<PaginationBenchmarks>();

using System.Text;
using aspnet.webapi.Data;
using aspnet.webapi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:Key").Value!)),
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

    // builder.Services.AddDbContext<ApplicationDbContext>(options =>
    // {
    //     options.UseInMemoryDatabase("aspnet.webapi.database");
    // });

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
