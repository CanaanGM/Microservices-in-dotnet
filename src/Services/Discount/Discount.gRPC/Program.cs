using Discount.gRPC.Extensions;
using Discount.gRPC.Protos;
using Discount.gRPC.Repositories;
using Discount.gRPC.Services;

using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Discount.gRPC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            // Add services to the container.
            builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddGrpc();


            builder.WebHost.ConfigureKestrel(options =>
            {
                // Setup a HTTP/2 endpoint without TLS.
                options.ListenAnyIP(5004, o => o.Protocols = HttpProtocols.Http2);
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.


            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.MapGrpcService<DiscountService>();


            app.MigrateDatabase<Program>();

            app.Run();
        }
    }
}