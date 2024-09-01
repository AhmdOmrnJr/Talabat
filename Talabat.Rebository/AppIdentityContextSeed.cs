using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.IdentityEntities;

namespace Talabat.Infrastructure
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed",
                    Email = "aomran@gmail.com",
                    UserName = "Ahmed_Omran",
                    Address = new Address()
                    {
                        FirstName = "Ahmed",
                        LastName = "Omran",
                        Street = "77",
                        State = "maadi",
                        City = "cairo",
                        ZipCode = "16645"
                    }
                };

                await userManager.CreateAsync(user, "Password123!");
            }
        }
    }
}
