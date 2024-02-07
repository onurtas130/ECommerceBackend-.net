// See https://aka.ms/new-console-template for more information


using RMQConsumer;

RabbitMQConsumerService rmq = new RabbitMQConsumerService();

rmq.DeclareExchanges();
rmq.DeclareQueues();
rmq.BindQueues();
rmq.ConsumeMessage();
Console.ReadLine();

