using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace BiddingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly ConnectionFactory _factory;

        public AuctionController()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
        }

        [HttpPost("enter-room")]
        public IActionResult EnterRoom()
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "bidding_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "User entered bidding room";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "bidding_queue",
                                     basicProperties: null,
                                     body: body);
                return Ok(new { Message = "User entered bidding room" });
            }
        }

        [HttpPost("submit-bid")]
        public IActionResult SubmitBid([FromBody] string bid)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "bidding_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(bid);

                channel.BasicPublish(exchange: "",
                                     routingKey: "bidding_queue",
                                     basicProperties: null,
                                     body: body);
                return Ok(new { Message = "Bid submitted", Bid = bid });
            }
        }

        [HttpGet("highest-bid")]
        public IActionResult GetHighestBid()
        {
            // Logic to fetch the current highest bid
            string highestBid = "1000"; // Placeholder
            return Ok(new { HighestBid = highestBid });
        }
    }
}

