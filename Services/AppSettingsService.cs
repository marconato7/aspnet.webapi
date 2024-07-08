// using aspnet.webapi.Models;

// namespace aspnet.webapi.Services;

// public static class AppSettingsService
// {
//     public static JwtSettings JwtSettings { get; }

//     static AppSettingsService()
//     {
//         IConfigurationRoot configuration = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json")
//             .Build();

//         JwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();
//     }
// }
