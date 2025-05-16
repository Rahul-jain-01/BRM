using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Application.DTOs;
using ChainMarketing.Domain.Entities;

namespace ChainMarketing.Application.Interfaces
{
    public interface IAdminService
    {
        Task<List<User>> GetUserDetails();
        Task<AdminUserReferralDto> GetUserReferralCodeByEmailAsync(string email);
        Task<ReferralTreeDto> GetReferralTreeByEmailAsync(string email);

    }
}
