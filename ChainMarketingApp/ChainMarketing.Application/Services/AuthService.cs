using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Application.DTOs;
using ChainMarketing.Application.Interfaces;
using ChainMarketing.Domain.Entities;
using ChainMarketing.Domain.Enums;
using ChainMarketing.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ChainMarketing.Application.Services
{
    public class AuthService : IAuthService

    {
            
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IReferralPathRepository _referralPathRepository;

        public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IReferralPathRepository referralPathRepository)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = new PasswordHasher<User>();
            _referralPathRepository = referralPathRepository;
        }

        public async Task<JwtResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Invalid credentials.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid credentials.");

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new JwtResponse
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<JwtResponse> RegisterAsync(RegisterRequest request)
        {
            // Check if the user already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            // 1. Create and hash password
            var user = new User
            {
                
                Email = request.Email,
                ReferralCode = GenerateReferralCode(),
                Role=UserRole.User,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);


            // 2. Check referral code
            if (!string.IsNullOrEmpty(request.ReferralCode))
            {
                var referrer = await _userRepository.GetByReferralCodeAsync(request.ReferralCode);
                if (referrer == null)
                    throw new Exception("Invalid referral code.");

                user.ReferredById = referrer.Id;

                

                if (referrer != null)
                {
                    // Check if the referrer has direct referrals and assign co-applicant
                    var directReferrals = await _userRepository.GetDirectReferralsAsync(referrer.Id);

                    // Separate co-applicant from regular referrals
                    var regularReferrals = directReferrals
                        .Where(r => r.Id != referrer.CoApplicantId)
                        .ToList();

                    if (regularReferrals.Count >= 6)
                    {
                        throw new Exception("This user has already referred 6 users.");
                    }

                    await _userRepository.AddAsync(user);

                    // Auto-assign 3rd referral as co-applicant
                    if (!referrer.HasCoApplicant && regularReferrals.Count == 2)
                    {
                        // This new user will be the 3rd
                        //user.HasCoApplicant = true;
                        //referrer.HasCoApplicant = true;
                        referrer.CoApplicantId = user.Id; // Assign new user as co-applicant of referrer

                        await _userRepository.UpdateAsync(referrer);
                    }
                    //// Track referral path for this user at each level
                    //await _referralPathRepository.AddReferralPathAsync(referrer.Id, user.Id, 1);

                    //// Now, you would need to track the referral paths recursively for each level
                    //var level1Referrals = await _userRepository.GetDirectReferralsAsync(referrer.Id);
                    //foreach (var referral in level1Referrals)
                    //{
                    //    await _referralPathRepository.AddReferralPathAsync(referrer.Id, referral.Id, 2);
                    //    // Continue recursively for further levels if needed
                    //}

                }
                
            }
            else
            {
                // No referral code, so just save the user
                await _userRepository.AddAsync(user);
            }

            // After saving the user, update the referral path
            // Save referral path for up to 3 levels
            if (user.ReferredById.HasValue)
            {
                var level1 = await _userRepository.GetByIdAsync(user.ReferredById.Value);
                if (level1 != null)
                {
                    await _referralPathRepository.AddReferralPathAsync(new ReferralPath
                    {
                        UserId = user.Id,
                        ReferrerId = level1.Id,
                        Level=1,
                        CreatedAt = DateTime.UtcNow
                    });

                    if (level1.ReferredById.HasValue)
                    {
                        var level2 = await _userRepository.GetByIdAsync(level1.ReferredById.Value);
                        if (level2 != null)
                        {
                            await _referralPathRepository.AddReferralPathAsync(new ReferralPath
                            {
                                UserId = user.Id,
                                ReferrerId = level2.Id,
                                Level=2,
                                CreatedAt = DateTime.UtcNow
                            });

                            if (level2.ReferredById.HasValue)
                            {
                                var level3 = await _userRepository.GetByIdAsync(level2.ReferredById.Value);
                                if (level3 != null)
                                {
                                    await _referralPathRepository.AddReferralPathAsync(new ReferralPath
                                    {
                                        UserId = user.Id,
                                        ReferrerId = level3.Id,
                                        Level=3,
                                        CreatedAt = DateTime.UtcNow
                                    });
                                }
                            }
                        }
                    }
                }
            }



            var token = _jwtTokenGenerator.GenerateToken(user);

            return new JwtResponse  
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }
        private string GenerateReferralCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }
       
    }
}
