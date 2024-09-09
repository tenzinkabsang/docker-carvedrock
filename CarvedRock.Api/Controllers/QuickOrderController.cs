using CarvedRock.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarvedRock.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuickOrderController(IOrderService orderService, ILogger<QuickOrderController> logger): ControllerBase
    {
        [HttpPost]
        public Task<Guid> SubmitOrder(Order order)
        {
            logger.LogInformation($"Submitting order for {order.Quantity} of {order.ProductId}");
            return orderService.PlaceOrder(order, customerId: 1234);
        }
    }
}
