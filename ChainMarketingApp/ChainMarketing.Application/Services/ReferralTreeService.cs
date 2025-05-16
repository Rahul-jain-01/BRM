using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Application.DTOs;
using ChainMarketing.Application.Interfaces;
using ChainMarketing.Domain.Interfaces;

namespace ChainMarketing.Application.Services
{
    public class ReferralTreeService : IReferralTreeService
    {
        private readonly IUserRepository _userRepository;

        public ReferralTreeService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ReferralTreeDto> GetReferralTreeAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var referralTreeDto = new ReferralTreeDto
            {
                UserId = user.Id
            };

            // Get Level 1 referrals (Direct Referrals)
            referralTreeDto.Level1 = await GetLevelReferralsAsync(user.Id, 1);

            // Get Level 2 referrals (Referrals of Level 1)
            referralTreeDto.Level2 = await GetLevelReferralsAsync(user.Id, 2);

            // Get Level 3 referrals (Referrals of Level 2)
            referralTreeDto.Level3 = await GetLevelReferralsAsync(user.Id, 3);

            return referralTreeDto;
        }
        private async Task<List<ReferralUserDto>> GetLevelReferralsAsync(int userId, int level)
        {
            var referrals = new List<ReferralUserDto>();
            var directReferrals = await _userRepository.GetDirectReferralsAsync(userId);

            foreach (var referral in directReferrals)
            {
                // Only process up to Level 3
                if (level == 1)
                {
                    referrals.Add(new ReferralUserDto
                    {
                        Id = referral.Id,
                        HasCoApplicant = referral.HasCoApplicant
                    });
                }
                else
                {
                    // Recursively get referrals for Level 2 and Level 3
                    var subLevelReferrals = await GetLevelReferralsAsync(referral.Id, level - 1);
                    referrals.AddRange(subLevelReferrals);
                }
            }
            return referrals;


        }
    }
}
