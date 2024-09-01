using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Infrastructure.Specifications
{
    public class ProductsWithFiltersForCountsSpecifications: BaseSpecification<Product>
    {
        public ProductsWithFiltersForCountsSpecifications(ProductSpecification specification)
            : base(x =>
                        (string.IsNullOrEmpty(specification.Search) || x.Name.Trim().ToLower().Contains(specification.Search)) &&
                        (!specification.BrandId.HasValue || x.ProductBrandId == specification.BrandId) &&
                        (!specification.TypeId.HasValue || x.ProductTypeId == specification.TypeId))
        {

        }
    }
}
