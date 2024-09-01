using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Infrastructure.Specifications;
using Talabat.services.Helper;
using Talabat.services.Services.ProductSercice.DTO;

namespace Talabat.services.Services.ProductSercice
{
    public interface IProductService
    {
        Task<ProductResultDTO> GetProductByIdAsync(int? id);
        Task<Pagination<ProductResultDTO>> GetProductsAsync(ProductSpecification specification);
        Task<IReadOnlyList<ProductBrand>> GetProductsBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductsTypesAsync();
    }
}
