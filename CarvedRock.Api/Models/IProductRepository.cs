namespace CarvedRock.Api.Models
{
    public interface IProductRepository
    {
        IList<Product> GetAllProducts();
    }



    public class ProductRepository : IProductRepository
    {
        private static readonly IList<Product> Products = [
            new Product{ Id = 1, Name = "Diamond Earring", Description = "Description", Category = "Earrings", Price = 19.99m },
            new Product{ Id = 1, Name = "Golden Earring", Description = "Description", Category = "Earrings", Price = 12.99m },
            new Product{ Id = 1, Name = "Silver Earring", Description = "Description", Category = "Earrings", Price = 11.99m },
            new Product{ Id = 1, Name = "Bronze Earring", Description = "Description", Category = "Earrings", Price = 44.99m },
            new Product{ Id = 1, Name = "Amethyst Earring", Description = "Description", Category = "Earrings", Price = 6.99m },
            ];

        public IList<Product> GetAllProducts()
        {
            //string category = "";

            // var query = db.Products;

            // if(category is not null)
            //      query.where(p => p.Category == category)

            // return query.toList();

            return [.. Products];
        }
    }
}
