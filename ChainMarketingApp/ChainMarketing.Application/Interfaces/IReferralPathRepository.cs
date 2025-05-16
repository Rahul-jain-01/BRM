using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Domain.Entities;

namespace ChainMarketing.Application.Interfaces
{
    public interface IReferralPathRepository
    {
        //Task AddReferralPathAsync(int referrerId, int userId, int level);
        //Task<List<ReferralPath>> GetReferralPathsAsync(int userId);
        Task AddReferralPathAsync(ReferralPath referralPath);
        Task<ReferralPath> GetReferralPathAsync(int userId);
        Task<List<ReferralPath>> GetReferralPathsAsync(int referrerId);
        Task<List<ReferralPath>> GetReferralPathsByLevelAsync(int referrerId, int level);
        Task<List<ReferralPath>> GetAllReferralPathsAsync();
        Task<List<User>> GetDirectReferralsAsync(int userId);
    }
}
