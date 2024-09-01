using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.services.Services.UserService.Dto;

namespace Talabat.services.Services.UserService
{
    public interface IUserService
    {
        Task<UserDto> Register(RegisterDto registerDto);
        Task<UserDto> Login(LoginDto loginDto);
        //Task<UserDto> GetCurrentUser();
    }
}
