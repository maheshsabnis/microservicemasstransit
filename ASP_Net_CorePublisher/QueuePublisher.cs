using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Model;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using RabbitMQ.Client;

namespace ASP_Net_CorePublisher
{
    public class QueuePublisher
    {
        //public void Publish(Employee employee, IBus bus)
        //{
        //    // creating a conneciton factory
        //    var factory = new ConnectionFactory()
        //    {
        //       // Uri = new Uri("amqp://guest:guest@emessage-rabbit:5672")
        //       Uri = new Uri("amqp://guest:guest@localhost:5672")
        //    };

        //    // create a connection (we are using the default one)
        //    var connection = factory.CreateConnection();
        //    // create a channel
        //    var channel = connection.CreateModel();

        //    // time to live message 
        //    var ttl = new Dictionary<string, object>
        //    {
        //        { "x-message-ttl", 30000} // message will live for 30 seconds
        //    };


        //    //                      Exchange name           ,   Exchange type   , optional argument
        //    channel.ExchangeDeclare("message.exchange",
        //        ExchangeType.Topic, arguments: ttl);



        //    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(employee));
        //    // since the routing key is account.init, that matched with the pattern the messages will
        //    // received by queue
        //    channel.BasicPublish("message.exchange", "message.queue.*", null, body);


        //}


        //public async Task Publish(Employee employee, IBus bus)
        //{
        //    var endpoint = await bus.GetSendEndpoint(
        //        new Uri("amqp://guest:guest@localhost:5672"));
        //     await endpoint.Send(employee);
        //}
    }
}