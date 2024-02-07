using Domain.models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            _channel.ExchangeDeclare("firstDirectExchange", ExchangeType.Direct);
            _channel.ExchangeDeclare("firstTopicExchange", ExchangeType.Topic);

            _channel.ExchangeDeclare("firstHeaderExchange", ExchangeType.Headers);
        }

        internal void DeclareQueues()
        {
            //durable:if its true, queue will be persist and wont be deleted when rabbit mq restart or stop.
            //exclusive:if its true, only one consumer can use this queue.
            //autoDelete:if its true, queue automatically will be deleted when last consumer unsubscribe.
            _channel.QueueDeclare("1Queue", durable: false, exclusive: false, autoDelete: false);
            _channel.QueueDeclare("2Queue", durable: false, exclusive: false, autoDelete: false);

            _channel.QueueDeclare("3Queue", durable: false, exclusive: false, autoDelete: false);
        }

        internal void BindQueues()
        {
            _channel.QueueBind("1Queue", "firstDirectExchange", "1Queue");
            _channel.QueueBind("2Queue", "firstTopicExchange", "*.car.*");

            // x-match can be all or any. if its all, all values must match. if its any, only one matched value is enough.
            _channel.QueueBind(queue: "3Queue", exchange: "firstHeaderExchange", routingKey: string.Empty, arguments: new Dictionary<string, object>()
            {
                {"x-match", "all" },
                {"color", "white" },
                {"type", "cat" }
            });

            _channel.QueueBind(queue: "2Queue", exchange: "firstHeaderExchange", routingKey: string.Empty, arguments: new Dictionary<string, object>()
            {
                {"x-match", "any" },
                {"color", "white" },
                {"type", "cat" }
            });
        }

        internal void ConsumeMessage()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                string str = Encoding.UTF8.GetString(body);
                var msg = JsonConvert.DeserializeObject<QueueModel>(str);

                Console.WriteLine($"1Queue message is: {msg.Name} and age:{msg.Age}");
            };

            string consumerTag = _channel.BasicConsume("1Queue", false, consumer);

            //2
            var consumer2 = new EventingBasicConsumer(_channel);

            consumer2.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                string str = Encoding.UTF8.GetString(body);
                var msg = JsonConvert.DeserializeObject<QueueModel>(str);

                Console.WriteLine($"2Queue message is: name:{msg.Name} and age:{msg.Age}");
            };

            string consumerTag2 = _channel.BasicConsume("2Queue", false, consumer2);

            //3
            var consumer3 = new EventingBasicConsumer(_channel);

            consumer3.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                string str = Encoding.UTF8.GetString(body);
                var msg = JsonConvert.DeserializeObject<QueueModel>(str);

                Console.WriteLine($"3Queue message is: name:{msg.Name} and age:{msg.Age}");
            };

            string consumerTag3 = _channel.BasicConsume("3Queue", false, consumer3);
        }

        internal void ConsumeTopicMessage()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                string msg = Encoding.UTF8.GetString(body);
                Console.WriteLine($"2Queue message is: {msg}");
            };

            string consumerTag = _channel.BasicConsume("2Queue", false, consumer);
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
