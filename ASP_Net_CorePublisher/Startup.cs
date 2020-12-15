using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ASP_Net_CorePublisher
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // adding the MassTransit Service
            //Adds the MassTransit Service to the ASP.NET Core Service Container.
            services.AddMassTransit(tx=> {
                //Creates a new Service Bus using RabbitMQ. Here we pass paramteres
                //like the host url, username and password.
                tx.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(cfg=>
                {
                    cfg.UseHealthCheck(bus);
                    cfg.Host(new Uri("rabbitmq://localhost"), host=>
                    {
                        host.Username("guest");
                        host.Password("guest");

                    });
                }));
            });
            services.AddMassTransitHostedService();
            // ends here
            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
           // services.AddScoped<QueuePublisher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
