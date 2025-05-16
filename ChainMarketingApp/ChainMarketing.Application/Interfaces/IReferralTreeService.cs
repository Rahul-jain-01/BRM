using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Application.DTOs;

namespace ChainMarketing.Application.Interfaces
{
    public interface IReferralTreeService
    {
        Task<ReferralTreeDto> GetReferralTreeAsync(int userId);
    }
}
