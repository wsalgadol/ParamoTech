using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Services;

namespace Sat.Recruitment.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IValidationService, ValidationService>();
                    services.AddTransient<IUserService, UserService>();
                    
                });
    }
}
