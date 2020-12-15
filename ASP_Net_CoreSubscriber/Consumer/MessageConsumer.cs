using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ASP_Net_CoreSubscriber.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Application.Model;

namespace ASP_Net_CoreSubscriber.Consumer
{
    public class QueueMessageConsumer : IConsumer<Employee>
    {
        //private readonly mydbContext dbctx;
        //public MessageConsumer(IServiceProvider service)
        //{
        //    var scope = service.CreateScope();
        //    dbctx = scope.ServiceProvider.GetService<mydbContext>();
        //}
        public async Task Consume(ConsumeContext<Employee> context)
        {
            var emp = context.Message;
            Debug.WriteLine(JsonConvert.SerializeObject(emp));
            //dbctx.Employee.Add(emp);
            //await dbctx.SaveChangesAsync();
        }
    }
}
