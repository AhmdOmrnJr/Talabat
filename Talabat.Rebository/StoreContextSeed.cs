using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Context;
using Talabat.Core.Entities;

namespace Talabat.Infrastructure
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.ProductBrands != null && !context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Talabat.Rebository/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    if (brands is not null)
                    {
                        foreach (var brand in brands)
                        {
                            await context.ProductBrands.AddAsync(brand);

                        }
                        await context.SaveChangesAsync();
                    }
                }
                if (context.ProductTypes != null && !context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Talabat.Rebository/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types is not null)
                    {
                        foreach (var type in types)
                        {
                            await context.ProductTypes.AddAsync(type);

                        }
                        await context.SaveChangesAsync();
                    }
                }
                if (context.Products != null && !context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Talabat.Rebository/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (products is not null)
                    {
                        foreach (var product in products)
                        {
                            await context.Products.AddAsync(product);

                        }
                        await context.SaveChangesAsync();
                    }
                }
                if (context.DeliveryMethods != null && !context.DeliveryMethods.Any())
                {
                    var DeliveryMethodsData = File.ReadAllText("../Talabat.Rebository/SeedData/delivery.json");
                    var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                    if (DeliveryMethods is not null)
                    {
                        foreach (var DeliveryMethod in DeliveryMethods)
                        {
                            await context.DeliveryMethods.AddAsync(DeliveryMethod);

                        }
                        await context.SaveChangesAsync();
                    }
                }
               
            }
            catch (Exception ex) 
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
