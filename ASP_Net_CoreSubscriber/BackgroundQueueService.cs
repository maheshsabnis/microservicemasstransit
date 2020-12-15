using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Model;
using ASP_Net_CoreSubscriber.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ASP_Net_CoreSubscriber
{
    public class BackgroundQueueService : BackgroundService
    {
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly mydbContext context;

        public BackgroundQueueService(ILoggerFactory loggerFactory,IServiceProvider service)
        {
            this._logger = loggerFactory.CreateLogger<BackgroundQueueService>();

            var scope = service.CreateScope();
            context = scope.ServiceProvider.GetService<mydbContext>();

                InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            // create a factory
            var factory = new ConnectionFactory()
            {
              // Uri = new Uri("amqp://guest:guest@emessage-rabbit:5672")
                 Uri = new Uri("amqp://guest:guest@localhost:5672") 
            };

            // create a connection
            _connection = factory.CreateConnection();

            // create channel
            _channel = _connection.CreateModel();

            // channel exchange declare
            _channel.ExchangeDeclare("message.exchange", ExchangeType.Topic);
            _channel.QueueDeclare("message.queue",
                durable: false, exclusive: false, autoDelete: false, arguments:null);
            _channel.QueueBind("message.queue","message.exchange","message.queue.*",null);
            // prefetch size, prefetch count
            _channel.BasicQos(0,1,false);

            _connection.ConnectionShutdown += _connection_ConnectionShutdown;

        }

        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, message) =>
            {
                // received body
                var content = Encoding.UTF8.GetString(message.Body.ToArray());
                // handled the received Message
                HandleReceivedMessage(content);
                // acknowledge the message
                _channel.BasicAck(message.DeliveryTag,false);
            };

            consumer.Shutdown += Consumer_Shutdown;
            consumer.Registered += Consumer_Registered;
            consumer.Unregistered += Consumer_Unregistered;
            consumer.ConsumerCancelled += Consumer_ConsumerCancelled;

            _channel.BasicConsume("message.queue",false,consumer);

            return Task.CompletedTask;
        }


        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

        private void Consumer_ConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Unregistered(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Registered(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void HandleReceivedMessage(string content)
        {

            Employee emp = System.Text.Json.JsonSerializer.Deserialize<Employee>(content);
            context.Employee.Add(emp);
            context.SaveChanges();

            Debug.WriteLine($"Received Messsage {content}");
        }
    }
}
