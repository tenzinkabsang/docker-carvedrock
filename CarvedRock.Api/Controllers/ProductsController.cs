using CarvedRock.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CarvedRock.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Product> GetProducts(string? category = "all")
        {
            Log.Information("Getting products for {category}", category);
            var products = productService.GetProductsForCategory(category);

            return products;
        }

        [HttpGet]
        [Route("api/products")]
        public async Task<IResult> GetProductsx(string? category = null)
        {
            return TypedResults.Ok(await productService.GetProductsAsync(category));
        }
    }
}
