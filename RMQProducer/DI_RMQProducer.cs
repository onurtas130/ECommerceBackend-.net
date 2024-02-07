using Application.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using RMQProducer.Services;

namespace RMQProducer
{
    public static class DI_RMQProducer
    {
        public static void AddRMQProducerServices(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQProducerService, RabbitMQProducerService>();
        }
    }
}
