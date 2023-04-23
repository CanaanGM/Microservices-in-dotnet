namespace OceltotApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureLogging((hostingContext, loggingBuilder) => {
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });


            var app = builder.Build();

            

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}