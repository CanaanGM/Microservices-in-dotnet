using EventBus.Messages.Common;
using EventBus.Messages.Events;

using MassTransit;

using Microsoft.AspNetCore.Hosting;

using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(Program));


            builder.Services.AddMassTransit(config => {

                config.AddConsumer<BasketCheckoutConsumer>();
                config.UsingRabbitMq((ctx, cfg) => {

                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue,
                        c => c.ConfigureConsumer<BasketCheckoutConsumer>(ctx)
                    );
                });
            });


            builder.Services.AddScoped<BasketCheckoutConsumer>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.MigrateDatabase<OrderContext>( (context, service) =>
            {
                var logger = service.GetService<ILogger<OrderContextSeed>>();
                OrderContextSeed
                    .SeedAsync(context, logger)
                    .Wait();
            } );


            app.Run();
        }
    }
}