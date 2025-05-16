using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Application.DTOs;
using ChainMarketing.Application.Interfaces;
using ChainMarketing.Domain.Entities;
using ChainMarketing.Domain.Interfaces;

namespace ChainMarketing.Application.Services
{
    public class ReferralPathService
    {
        private readonly IReferralPathRepository _referralPathRepository;
        private readonly IUserRepository _userRepository;

        public ReferralPathService(IReferralPathRepository referralPathRepository, IUserRepository userRepository)
        {
            _referralPathRepository = referralPathRepository;
            _userRepository = userRepository;
        }

        public async Task AddReferralPathAsync(int userId, int referrerId)
        {
            // Ensure no duplicate referrals are added
            var existingReferralPath = await _referralPathRepository.GetReferralPathAsync(userId);
            if (existingReferralPath != null)
                throw new Exception("User has already been referred.");

            var referralPath = new ReferralPath
            {
                UserId = userId,
                ReferrerId = referrerId
            };

            await _referralPathRepository.AddReferralPathAsync(referralPath);
        }

        public async Task<List<ReferralPath>> GetReferralPathsForReferrerAsync(int referrerId)
        {
            return await _referralPathRepository.GetReferralPathsAsync(referrerId);
        }

        public async Task<ReferralTreeDto> GetReferralTreeFromPathTableAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var tree = new ReferralTreeDto
            {
                UserId = user.Id,
                Level1 = new List<ReferralUserDto>(),
                Level2 = new List<ReferralUserDto>(),
                Level3 = new List<ReferralUserDto>()
            };

            var visited = new HashSet<int> { user.Id };

            // Level 1
            var level1Paths = await _referralPathRepository.GetReferralPathsAsync(user.Id);
            foreach (var path in level1Paths)
            {
                if (!visited.Add(path.User.Id)) continue;

                tree.Level1.Add(new ReferralUserDto
                {
                    Id = path.User.Id,
                    HasCoApplicant = path.User.HasCoApplicant
                });
            }

            // Level 2
            foreach (var level1User in tree.Level1)
            {
                var level2Paths = await _referralPathRepository.GetReferralPathsAsync(level1User.Id);
                foreach (var path in level2Paths)
                {
                    if (!visited.Add(path.User.Id)) continue;

                    tree.Level2.Add(new ReferralUserDto
                    {
                        Id = path.User.Id,
                        HasCoApplicant = path.User.HasCoApplicant
                    });
                }
            }

            // Level 3
            foreach (var level2User in tree.Level2)
            {
                var level3Paths = await _referralPathRepository.GetReferralPathsAsync(level2User.Id);
                foreach (var path in level3Paths)
                {
                    if (!visited.Add(path.User.Id)) continue;

                    tree.Level3.Add(new ReferralUserDto
                    {
                        Id = path.User.Id,
                        HasCoApplicant = path.User.HasCoApplicant
                    });
                }
            }

            return tree;
        }




    }

}
