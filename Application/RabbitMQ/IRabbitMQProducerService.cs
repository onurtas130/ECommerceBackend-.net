using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RabbitMQ
{
    public interface IRabbitMQProducerService
    {
        /// <summary>
        /// publish message to any direct, fanout or topic exchange.
        /// header exchange is not included, use another overloaded method for header exchange.
        /// </summary>
        /// <typeparam name="T">type of message(data)</typeparam>
        /// <param name="message">data</param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        public void PublishMessage<T>(T message, string exchangeName, string routingKey);

        /// <summary>
        /// publish message to any header exchange.
        /// this method is just for header exchanges.
        /// </summary>
        /// <typeparam name="T">type of message(data)</typeparam>
        /// <param name="message">data</param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="header"></param>
        public void PublishMessage<T>(T message, string exchangeName, Dictionary<string, object> header);
    }
}
