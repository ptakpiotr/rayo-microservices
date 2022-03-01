using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace RayoInfo.RabbitMQ
{
    public class EventSubscriber : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly ILogger<EventSubscriber> _logger;

        public AppDbContext _ctx { get; }

        public EventSubscriber(IConfiguration config, ILogger<EventSubscriber> logger, IDbContextFactory<AppDbContext> ctx)
        {
            _logger = logger;
            _ctx = ctx.CreateDbContext();
            try
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(config["Rabbit:Uri"])

                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();


                _channel.ExchangeDeclare(config["Rabbit:Exchange"], ExchangeType.Fanout, true);
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(_queueName, config["Rabbit:Exchange"], "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += Consumer_Received;

                _channel.BasicConsume(_queueName, true, consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Task.CompletedTask;
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            StandingsModel data = JsonSerializer.Deserialize<StandingsModel>(e.Body.ToArray());

            _ctx.Standings.Add(data);
            _ctx.SaveChanges();
        }
    }
}
