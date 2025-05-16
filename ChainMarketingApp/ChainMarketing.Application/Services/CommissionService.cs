using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Application.Interfaces;
using ChainMarketing.Domain.Entities;

namespace ChainMarketing.Application.Services
{
    public class CommissionService:ICommissionService
    {
        private readonly IReferralPathRepository _referralPathRepository;

        public CommissionService(IReferralPathRepository referralPathRepository)
        {
            _referralPathRepository = referralPathRepository;
        }

        public async Task<decimal> CalculateCommissionAsync(int userId)
        {
            const decimal firstTwoBonus = 10000m;
            const decimal perReferralBonus = 1000m;

            var level1 = await _referralPathRepository.GetReferralPathsAsync(userId);
            var validLevel1 = level1.Where(u => !u.User.HasCoApplicant).ToList();

            decimal commission = 0;

            if (validLevel1.Count >= 2)
            {
                // First two = ₹10,000 flat
                commission += 10000;

                // Remaining referrals after first two
                commission += validLevel1.Skip(2).Count() * 1000;
            }
            else
            {
                // If only 0 or 1 valid referral(s), no ₹10,000
                commission += validLevel1.Count * 0; // or handle as 0
            }

            var level2 = new List<ReferralPath>();
            var level3 = new List<ReferralPath>();

            foreach (var user in validLevel1)
            {
                var l2 = await _referralPathRepository.GetReferralPathsAsync(user.UserId);
                var validL2 = l2.Where(u => !u.User.HasCoApplicant).ToList();
                level2.AddRange(validL2);

                foreach (var user2 in validL2)
                {
                    var l3 = await _referralPathRepository.GetReferralPathsAsync(user2.UserId);
                    var validL3 = l3.Where(u => !u.User.HasCoApplicant).ToList();
                    level3.AddRange(validL3);
                }
            }

            commission += level2.Count * perReferralBonus;
            commission += level3.Count * perReferralBonus;

            return commission;
        }
    }
}
