using Serilog;

namespace CarvedRock.Api.Models
{
    public interface IProductService
    {
        IList<Product> GetProductsForCategory(string? category);

        Task<IList<Product>> GetProductsAsync(string? category);
    }

    public sealed class ProductService(IProductRepository productRepository) : IProductService
    {
        private static readonly string[] _validCategories = ["all", "boots", "earrings"];

        public async Task<IList<Product>> GetProductsAsync(string? category)
        {
            Log.Information("ProductService: Getting products for {category}", category);
            ArgumentNullException.ThrowIfNull(category, nameof(category));

            var products = await Task.FromResult(productRepository.GetAllProducts());

            return products;
        }

        public IList<Product> GetProductsForCategory(string? category)
        {
            Log.Information("ProductService: Getting products for {category}", category);

            if (!_validCategories.Any(c => c.Equals(category, StringComparison.InvariantCultureIgnoreCase)))
                throw new ApplicationException($"Unrecognized category: {category}. Valid categories are: [{string.Join(", ", _validCategories)}]");

            var allProducts = productRepository.GetAllProducts();

            if (string.IsNullOrEmpty(category))
                return allProducts;

            return allProducts.Where(p => p.Category.Equals(category, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
    }

}
