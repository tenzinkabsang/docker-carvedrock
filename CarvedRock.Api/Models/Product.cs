namespace CarvedRock.Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }


    public class Order
    {
        public Guid OrderId { get; set; }
        public int Quantity { get; internal set; }
        public int ProductId { get; internal set; }
    }
}
