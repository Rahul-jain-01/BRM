using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Application.Interfaces;
using ChainMarketing.Domain.Entities;
using ChainMarketing.Domain.Interfaces;

namespace ChainMarketing.Application.Services
{
    public class CommissionService:ICommissionService
    {
        private readonly IReferralPathRepository _referralPathRepository;
        private readonly IUserRepository _userRepository;
        public CommissionService(IReferralPathRepository referralPathRepository,IUserRepository userRepository)
        {
            _referralPathRepository = referralPathRepository;
            _userRepository = userRepository;
        }

        public async Task<decimal> CalculateCommissionAsync(int userId)
        {
            const decimal firstTwoBonus = 10000m;
            const decimal perReferralBonusWithCoApplicant = 1000m;
            const decimal perReferralBonusWithoutCoApplicant = 500m;

            decimal commission = 0;

            // Get the top user (to know their co-applicant)
            var topUser = await _userRepository.GetByIdAsync(userId);
            var topCoApplicantId = topUser.CoApplicantId;

            // Get level 1 referrals (direct referrals of top user)
            var level1 = await _referralPathRepository.GetReferralPathsAsync(userId);

            // Identify co-applicant IDs on level 1:
            // Include the top user's co-applicant and also co-applicant IDs from referrals
            var coApplicantIds = new HashSet<int>();
            if (topCoApplicantId.HasValue)
                coApplicantIds.Add(topCoApplicantId.Value);

            // Also add co-applicants from referrals if you have that info:
            foreach (var referral in level1)
            {
                if (referral.User.CoApplicantId.HasValue)
                    coApplicantIds.Add(referral.User.CoApplicantId.Value);
            }

            // Filter level 1 referrals to exclude co-applicants themselves
            var mainLevel1 = level1.Where(r => !coApplicantIds.Contains(r.User.Id)).ToList();

            // Commission for first two main referrals (flat ₹10,000)
            if (mainLevel1.Count >= 2)
            {
                commission += firstTwoBonus;
            }

            // For remaining level 1 referrals after first two, pay per referral bonus based on whether they have co-applicant or not
            foreach (var referral in mainLevel1.Skip(2))
            {
                commission += referral.User.HasCoApplicant
                    ? perReferralBonusWithCoApplicant
                    : perReferralBonusWithoutCoApplicant;
            }

            // Now process level 2 referrals of each level 1 main referral
            var level2 = new List<ReferralPath>();
            foreach (var referral in mainLevel1)
            {
                var subLevel2 = await _referralPathRepository.GetReferralPathsAsync(referral.User.Id);
                level2.AddRange(subLevel2);
            }

            // Filter level 2 to exclude co-applicants
            var mainLevel2 = level2.Where(r => !coApplicantIds.Contains(r.User.Id)).ToList();

            // Pay commission for level 2 referrals
            foreach (var referral in mainLevel2)
            {
                commission += referral.User.HasCoApplicant
                    ? perReferralBonusWithCoApplicant
                    : perReferralBonusWithoutCoApplicant;
            }

            // Now process level 3 referrals of each level 2 main referral
            var level3 = new List<ReferralPath>();
            foreach (var referral in mainLevel2)
            {
                var subLevel3 = await _referralPathRepository.GetReferralPathsAsync(referral.User.Id);
                level3.AddRange(subLevel3);
            }

            // Filter level 3 to exclude co-applicants
            var mainLevel3 = level3.Where(r => !coApplicantIds.Contains(r.User.Id)).ToList();

            // Pay commission for level 3 referrals
            foreach (var referral in mainLevel3)
            {
                commission += referral.User.HasCoApplicant
                    ? perReferralBonusWithCoApplicant
                    : perReferralBonusWithoutCoApplicant;
            }

            return commission;
        }

    }
}
