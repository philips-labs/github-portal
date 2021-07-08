using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crawler
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureHostConfiguration(configHost =>
            {
                configHost.AddEnvironmentVariables();
            }).ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuration = hostContext.Configuration;
                Config config = configuration.GetSection("App").Get<Config>();
                services.AddSingleton(config);

                services.AddHostedService<Crawler>();
            });
        }
    }
}
