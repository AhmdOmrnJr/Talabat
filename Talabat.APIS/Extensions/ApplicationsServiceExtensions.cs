using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.HandleResponses;
using Talabat.Infrastructure.BasketRepository;
using Talabat.Infrastructure.Interfaces;
using Talabat.Infrastructure.Repositories;
using Talabat.services.CacheService;
using Talabat.services.Services.BasketService;
using Talabat.services.Services.BasketService.DTO;
using Talabat.services.Services.OrderService;
using Talabat.services.Services.OrderService.Dto;
using Talabat.services.Services.PaymentService;
using Talabat.services.Services.ProductSercice;
using Talabat.services.Services.ProductSercice.DTO;
using Talabat.services.Services.TokenService;
using Talabat.services.Services.UserService;

namespace Talabat.APIS.Extensions
{
    public static class ApplicationsServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            //services.AddScoped<IcacheService, CacheService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            //builder.Services.AddAutoMapper(x => x.AddProfile(new ProductProfiler()));


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                    .Where(model => model.Value.Errors.Count > 0)
                                    .SelectMany(model => model.Value.Errors)
                                    .Select(error => error.ErrorMessage).ToList();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddAutoMapper(typeof(ProductProfiler));
            services.AddAutoMapper(typeof(BasketProfiler));
            services.AddAutoMapper(typeof(OrderProfile));

            return services;
        }
    }
}
