using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ASP_Net_CoreSubscriber.Models;
using Microsoft.EntityFrameworkCore;
using MassTransit.MultiBus;
using MassTransit;
using ASP_Net_CoreSubscriber.Consumer;
using GreenPipes;

namespace ASP_Net_CoreSubscriber
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


            services.AddMassTransit(x =>
            {
                x.AddConsumer<QueueMessageConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(provider);
                    cfg.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ReceiveEndpoint("employeequeue", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<QueueMessageConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();
            // ends here

            services.AddDbContext<mydbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("AppString"));
            });
            services.AddControllers().AddJsonOptions(options=> {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            // Register the Background Service
          //  services.AddHostedService<BackgroundQueueService>();
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
