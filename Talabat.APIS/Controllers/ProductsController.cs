using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.HandleResponses;
using Talabat.APIS.Helper;
using Talabat.Core.Entities;
using Talabat.Infrastructure.Interfaces;
using Talabat.Infrastructure.Specifications;
using Talabat.services.Helper;
using Talabat.services.Services.ProductSercice;
using Talabat.services.Services.ProductSercice.DTO;

namespace Talabat.APIS.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService) 
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductResultDTO>>> GetProducts([FromQuery] ProductSpecification specification)
        {
            var products = await _productService.GetProductsAsync(specification);
            return Ok(products);
        }

        [HttpGet("id")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Cache(500)]
        public async Task<ActionResult<ProductResultDTO>> GetProductById(int? id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product is null)
            
                return NotFound(new ApiResponse(404));

            return Ok(product);
        }

        [HttpGet("brand")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
            => Ok(await _productService.GetProductsBrandsAsync());

        [HttpGet("type")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
            => Ok(await _productService.GetProductsTypesAsync());
    }
}
