using System.Runtime.CompilerServices;

namespace RayoAuth.RabbitMQ
{
    public interface IRabbitPublisher
    {
        void SendStandings(StandingsModel model, [CallerArgumentExpression("model")] string message = "");
    }
}