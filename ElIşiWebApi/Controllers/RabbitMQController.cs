using Application.RabbitMQ;
using ElIşiWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElIşiWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQController : ControllerBase
    {
        private readonly IRabbitMQProducerService _rmqService;
        public RabbitMQController(IRabbitMQProducerService rmq)
        {
            _rmqService = rmq;
        }    

        [HttpPost]
        public IActionResult SendMessage(RabbitMQSenderVM model)
        {
            _rmqService.PublishMessage(model.Message, model.ExchangeName, new Dictionary<string, object>()
            {
                { "color","white" }
            });
            return Ok();
        }
    }
}
