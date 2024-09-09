namespace CarvedRock.Api.Models
{
    public interface IOrderService
    {
        Task<Guid> PlaceOrder(Order order, int customerId);
    }

    public class OrderService(ILogger<OrderService> logger) : IOrderService
    {
        public Task<Guid> PlaceOrder(Order order, int customerId)
        {
            logger.LogInformation("Placing order and sending update for inventory...");

            // persist order to database

            // post `orderplaced` event to rabbitmq

            return Task.FromResult(Guid.NewGuid());
        }
    }
}
