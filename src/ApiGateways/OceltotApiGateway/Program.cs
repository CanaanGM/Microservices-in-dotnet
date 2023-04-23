using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OceltotApiGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
            {
                //if (hostingContext.HostingEnvironment.IsEnvironment("Local"))
                //    config.AddJsonFile("ocelot.Local.json");

                config.AddJsonFile(
                    path: $"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", 
                    optional: true, 
                    reloadOnChange: true
                );
            });
            
            builder.WebHost.ConfigureLogging((hostingContext, loggingBuilder) => {
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });


            builder.Services.AddOcelot();



            var app = builder.Build();

            

            app.MapGet("/", () => "Hello World!");


            await app.UseOcelot();
            
            await app.RunAsync();
        }
    }
}