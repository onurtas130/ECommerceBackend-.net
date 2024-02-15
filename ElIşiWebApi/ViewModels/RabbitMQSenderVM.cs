using Application.RabbitMQ.Models;

namespace ElIşiWebApi.ViewModels
{
    public class RabbitMQSenderVM
    {
        public QueueModel Message { get; set; }
        public string ExchangeName { get; set; }
        //public Dictionary<string, object> Props { get; set; } 
    }
}
