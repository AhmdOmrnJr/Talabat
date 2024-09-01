using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.IdentityEntities;

namespace Talabat.services.Services.TokenService
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
