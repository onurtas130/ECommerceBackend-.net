using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Consts
{
    public static class RabbitMQ
    {
        public static class Exchanges
        {
            public static string firstDirectExchange = "firstDirectExchange";

            public static string firstTopicExchange = "firstTopicExchange";

            public static string firstHeaderExchange = "firstHeaderExchange";
        }

        public static class Queues
        {
            public static string queue1 = "queue1";

            public static string queue2 = "queue2";

            public static string queue3 = "queue3";
        }

    }
}
