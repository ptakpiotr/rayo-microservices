using RabbitMQ.Client;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace RayoAuth.RabbitMQ
{
    public class RabbitPublisher : IRabbitPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeName;

        public RabbitPublisher(IConfiguration configuration)
        {

            var factory = new ConnectionFactory()
            {
                Uri=new Uri(configuration["Rabbit:Uri"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _exchangeName = configuration["Rabbit:ExchangeName"];
                _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, true);
            }
            catch
            {

            }
        }

        //some args of course --> using callerargumentexpr to give more detail on what might have gone wrong
        //to do
        public void SendStandings(StandingsModel model, [CallerArgumentExpression("model")] string message = "")
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model));
                var props = _channel.CreateBasicProperties();

                _channel.BasicPublish(_exchangeName, "", null, data);
            }catch(ArgumentException ex)
            {
                throw new ArgumentException(message);

            }catch(Exception ex)
            {
                
            }
        }
    }
}
