using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Talabat.Core.Context;
using Talabat.Core.IdentityEntities;
using Talabat.Infrastructure;

namespace Talabat.APIS.Helper
{
    public class ApplySeeding
    {
        public static async Task ApplySeedingAsync(WebApplication app)
        {
             using (var scope = app.Services.CreateScope()) 
             {
                var services = scope.ServiceProvider;
                var loggerFfactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<StoreDbContext>();
                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    await context.Database.MigrateAsync();
                    await StoreContextSeed.SeedAsync(context, loggerFfactory);
                    await AppIdentityContextSeed.SeedUserAsync(userManager);
                }
                catch (Exception ex) 
                { 
                    var logger  = loggerFfactory.CreateLogger<StoreContextSeed>();
                    logger.LogError(ex.Message);
                }
             }
        }
           

    }
}
