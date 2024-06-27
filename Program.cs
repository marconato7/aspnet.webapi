// using BenchmarkDotNet.Running;
// using aspnet.webapi;
// BenchmarkRunner.Run<PaginationBenchmarks>();

using System.Text;
using aspnet.webapi.Data;
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
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration.GetSection("JwtSettings:Issuer").Value,
            ValidAudience = builder.Configuration.GetSection("JwtSettings:Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:Key").Value!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });

    builder.Services.AddAuthorization();

    // builder.Services
    //     .AddIdentityApiEndpoints<IdentityUser>()
    //     .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddDbContext<ApplicationDbContext>();
    // builder.Services.AddDbContext<ApplicationDbContext>(options =>
    // {
    //     options.UseInMemoryDatabase("aspnet.webapi.database");
    // });
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        // app.UseSwagger();
        // app.UseSwaggerUI();
    }

    // app.MapIdentityApi<IdentityUser>();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseAuthorization();

    app.MapControllers();
}

app.Run();
