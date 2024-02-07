using Application.Consts;
using Application.RabbitMQ.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RMQConsumer
{
    internal class RabbitMQConsumerService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        internal RabbitMQConsumerService()
        {
            _connection = CreateConnection();

            //create channel
            _channel = _connection.CreateModel();
        }

        internal IConnection CreateConnection()
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://user:producer1@localhost:5672/");
            return factory.CreateConnection();
        }

        internal void DeclareExchanges()
        {
            _channel.ExchangeDeclare(Application.Consts.RabbitMQ.Exchanges.firstDirectExchange, ExchangeType.Direct);
            _channel.ExchangeDeclare(Application.Consts.RabbitMQ.Exchanges.firstTopicExchange, ExchangeType.Topic);
            _channel.ExchangeDeclare(Application.Consts.RabbitMQ.Exchanges.firstHeaderExchange, ExchangeType.Headers);
        }

        internal void DeclareQueues()
        {
            //durable:if its true, queue will be persist and wont be deleted when rabbit mq restart or stop.
            //exclusive:if its true, only one consumer can use this queue.
            //autoDelete:if its true, queue automatically will be deleted when last consumer unsubscribe.
            _channel.QueueDeclare(Application.Consts.RabbitMQ.Queues.queue1, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueDeclare(Application.Consts.RabbitMQ.Queues.queue2, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueDeclare(Application.Consts.RabbitMQ.Queues.queue3, durable: false, exclusive: false, autoDelete: false);
        }

        internal void BindQueues()
        {
            _channel.QueueBind(Application.Consts.RabbitMQ.Queues.queue1, Application.Consts.RabbitMQ.Exchanges.firstDirectExchange, Application.Consts.RabbitMQ.Queues.queue1);

            _channel.QueueBind(Application.Consts.RabbitMQ.Queues.queue2, Application.Consts.RabbitMQ.Exchanges.firstTopicExchange, "*.car.*");

            // x-match can be all or any. if its all, all values must match. if its any, only one matched value is enough.
            _channel.QueueBind(queue: Application.Consts.RabbitMQ.Queues.queue3, exchange: Application.Consts.RabbitMQ.Exchanges.firstHeaderExchange, routingKey: string.Empty, arguments: new Dictionary<string, object>()
            {
                {"x-match", "all" },
                {"color", "white" },
                {"type", "cat" }
            });


        }

        internal void ConsumeMessages()
        {
            ConsumeQueue1();
        }

        internal void ConsumeQueue1()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                string str = Encoding.UTF8.GetString(body);
                var msg = JsonConvert.DeserializeObject<QueueModel>(str);

                Console.WriteLine($"1Queue message is: {msg.Name} and age:{msg.Age}");
            };

            string consumerTag = _channel.BasicConsume(Application.Consts.RabbitMQ.Queues.queue1, false, consumer);
        }

        /// <summary>
        /// destructor
        /// </summary>
        ~RabbitMQConsumerService()
        {
            _channel.Close();
            _connection.Close();
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
