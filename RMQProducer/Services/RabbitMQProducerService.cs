using Application.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RMQProducer.Services
{
    public class RabbitMQProducerService : IRabbitMQProducerService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        /// <summary>
        /// constructor
        /// </summary>
        public RabbitMQProducerService()
        {
            _connection = CreateConnection();

            //create channel
            _channel = _connection.CreateModel();
        }

        /// <summary>
        /// creates rabbitMQ connection
        /// </summary>
        /// <returns></returns>
        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://user:producer1@localhost:5672/");

            return factory.CreateConnection();
        }

        /// <inheritdoc cref="PublishMessage{T}(T, string, string)" />
        public void PublishMessage<T>(T message, string exchangeName, string routingKey)
        {
            var json = JsonConvert.SerializeObject(message);
            var bodyAsByte = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchangeName, routingKey, body: bodyAsByte);
        }

        /// <inheritdoc cref="PublishMessage{T}(T, string, Dictionary{string, object})"/>
        public void PublishMessage<T>(T message, string exchangeName, Dictionary<string, object> header)
        {
            var json = JsonConvert.SerializeObject(message);
            var bodyAsByte = Encoding.UTF8.GetBytes(json);
            var prop = _channel.CreateBasicProperties();
            prop.Headers = header;

            _channel.BasicPublish(exchangeName, string.Empty, prop, bodyAsByte);
        }

        /// <summary>
        /// destructor
        /// </summary>
        ~RabbitMQProducerService()
        {
            _channel.Close();
            _connection.Close();
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
