using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Infrastructure.Interfaces;
using Talabat.Infrastructure.Specifications;
using Talabat.services.Helper;
using Talabat.services.Services.ProductSercice.DTO;

namespace Talabat.services.Services.ProductSercice
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ProductResultDTO> GetProductByIdAsync(int? id)
        {
            var specs = new ProductsWithTypesAndBrandsSpecifications(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecifictionAsync(specs);
            var mappedProduct = _mapper.Map<ProductResultDTO>(product);
            return mappedProduct;
        }

        public async Task<Pagination<ProductResultDTO>> GetProductsAsync(ProductSpecification specification)
        {
            var specs = new ProductsWithTypesAndBrandsSpecifications(specification);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecifictionAsync(specs);
            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(specs);
            var mappedProducts = _mapper.Map<IReadOnlyList<ProductResultDTO>>(products);
            return new Pagination<ProductResultDTO>(specification.PageIndex, specification.PageSize, totalItems, mappedProducts);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductsBrandsAsync()
            => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductType>> GetProductsTypesAsync()
            => await _unitOfWork.Repository<ProductType>().GetAllAsync();
    }
}
