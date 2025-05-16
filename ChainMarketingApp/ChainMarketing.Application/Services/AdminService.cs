using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Application.DTOs;
using ChainMarketing.Application.Interfaces;
using ChainMarketing.Domain.Entities;
using ChainMarketing.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ChainMarketing.Application.Services
{
    public class AdminService:IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly ReferralPathService _referralPathService;

        public AdminService(IUserRepository userRepository, ReferralPathService referralPathService)
        {
            _userRepository = userRepository;
            _referralPathService = referralPathService;
        }

        public async Task<AdminUserReferralDto> GetUserReferralCodeByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) throw new Exception("User not found");

            return new AdminUserReferralDto
            {
                UserId = user.Id,
                Email = user.Email,
                ReferralCode = user.ReferralCode
            };
        }

        public async Task<ReferralTreeDto> GetReferralTreeByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return await _referralPathService.GetReferralTreeFromPathTableAsync(user.Id);
        }

        public async Task<List<User>> GetUserDetails()
        {
            var user =await _userRepository.GetAllUserAync();

            return user;            



        }
    }
}
