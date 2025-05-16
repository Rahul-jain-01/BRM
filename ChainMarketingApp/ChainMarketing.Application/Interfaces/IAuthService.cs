using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Application.DTOs;

namespace ChainMarketing.Application.Interfaces
{
    public interface IAuthService
    {
        Task<JwtResponse> RegisterAsync(RegisterRequest request);
        Task<JwtResponse> LoginAsync(LoginRequest request);
    }
}
