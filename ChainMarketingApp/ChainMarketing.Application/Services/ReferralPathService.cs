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

        public async Task<int> GetActualLevel1ReferralCountAsync(int topUserId)
        {
            var directReferrals = await _referralPathRepository.GetReferralPathsAsync(topUserId);

            var topUser = await _userRepository.GetByIdAsync(topUserId);
            var coApplicantId = topUser?.CoApplicantId;

            // Exclude the co-applicant from the count
            var level1Count = directReferrals.Count(r => r.User.Id != coApplicantId);

            return level1Count;
        }


        public async Task<ReferralTreeDto> GetReferralTreeFromPathTableAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var tree = new ReferralTreeDto
            {
                UserId = user.Id,
                coApplicantId = user.CoApplicantId,
                Level1 = new List<ReferralUserDto>(),
                Level2 = new List<ReferralUserDto>(),
                Level3 = new List<ReferralUserDto>()
            };

            var visited = new HashSet<int> { user.Id };

            // ----- LEVEL 1 -----
            var level1Paths = await _referralPathRepository.GetReferralPathsAsync(user.Id);
            foreach (var path in level1Paths)
            {
                var referredUser = path.User;

                // Skip if already visited or is the top user's co-applicant
                if (!visited.Add(referredUser.Id) || referredUser.Id == user.CoApplicantId)
                    continue;

                tree.Level1.Add(new ReferralUserDto
                {
                    Id = referredUser.Id,
                    HasCoApplicant = referredUser.HasCoApplicant,
                    IsCoApplicant = false
                });

                // Add their co-applicant (if any and not visited yet)
                if (referredUser.CoApplicantId.HasValue && visited.Add(referredUser.CoApplicantId.Value))
                {
                    var coApp = await _userRepository.GetByIdAsync(referredUser.CoApplicantId.Value);
                    if (coApp != null)
                    {
                        tree.Level1.Add(new ReferralUserDto
                        {
                            Id = coApp.Id,
                            HasCoApplicant = false,
                            IsCoApplicant = true
                        });
                    }
                }
            }

            // ----- LEVEL 2 -----
            foreach (var level1User in tree.Level1.Where(u => !u.IsCoApplicant))
            {
                var level2Paths = await _referralPathRepository.GetReferralPathsAsync(level1User.Id);
                foreach (var path in level2Paths)
                {
                    var referredUser = path.User;

                    if (!visited.Add(referredUser.Id)) continue;

                    tree.Level2.Add(new ReferralUserDto
                    {
                        Id = referredUser.Id,
                        HasCoApplicant = referredUser.HasCoApplicant,
                        IsCoApplicant = false
                    });

                    if (referredUser.CoApplicantId.HasValue && visited.Add(referredUser.CoApplicantId.Value))
                    {
                        var coApp = await _userRepository.GetByIdAsync(referredUser.CoApplicantId.Value);
                        if (coApp != null)
                        {
                            tree.Level2.Add(new ReferralUserDto
                            {
                                Id = coApp.Id,
                                HasCoApplicant = false,
                                IsCoApplicant = true
                            });
                        }
                    }
                }
            }

            // ----- LEVEL 3 -----
            foreach (var level2User in tree.Level2.Where(u => !u.IsCoApplicant))
            {
                var level3Paths = await _referralPathRepository.GetReferralPathsAsync(level2User.Id);
                foreach (var path in level3Paths)
                {
                    var referredUser = path.User;

                    if (!visited.Add(referredUser.Id)) continue;

                    tree.Level3.Add(new ReferralUserDto
                    {
                        Id = referredUser.Id,
                        HasCoApplicant = referredUser.HasCoApplicant,
                        IsCoApplicant = false
                    });

                    if (referredUser.CoApplicantId.HasValue && visited.Add(referredUser.CoApplicantId.Value))
                    {
                        var coApp = await _userRepository.GetByIdAsync(referredUser.CoApplicantId.Value);
                        if (coApp != null)
                        {
                            tree.Level3.Add(new ReferralUserDto
                            {
                                Id = coApp.Id,
                                HasCoApplicant = false,
                                IsCoApplicant = true
                            });
                        }
                    }
                }
            }

            return tree;
        }






    }

}
